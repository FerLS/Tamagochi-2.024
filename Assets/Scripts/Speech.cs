using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using System.Threading;
using TMPro;
using System.Net.Http;
using System.Text;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class Speech : MonoBehaviour
{
    // Hook up the two properties below with a Text and Button object in your UI.
    public TextMeshProUGUI outputText;

    public Button startRecoButton;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;

    private bool micPermissionGranted = false;

    // Variable to store the recognized speech
    private string recognizedSpeech;

    private const string speechAIKey = "2524cd9f46284cceb673f445eba74e0c";
    private const string speechAIRegion = "francecentral";

    private const string azureOpenAIEndpoint = "https://11078-m3z4gxr9-eastus2.cognitiveservices.azure.com/openai/deployments/gpt-4/chat/completions?api-version=2024-08-01-preview";
    private const string azureOpenAIKey = "FbYZnPzs6qjYPhWOBgYlmTMVwNByahpOD8qjPrjXAhhK7ckLdNWkJQQJ99AKACHYHv6XJ3w3AAAAACOGiZ5N";
    private const string openAIModel = "gpt-4";
    

    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    public async void ButtonClick()
    {
        var config = SpeechConfig.FromSubscription(speechAIKey, speechAIRegion);

        using (var recognizer = new SpeechRecognizer(config))
        {
            // Indicate that recognition is in progress
            waitingForReco = true;

            // Wait for the semaphore to ensure thread safety
            await semaphore.WaitAsync();
            try
            {
                var result = await recognizer.RecognizeOnceAsync();

                // Process the result
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedSpeech = result.Text;

                    // Call OpenAI for a reply
                    string tamagotchiReply = await GetTamagotchiReplyFromOpenAI(recognizedSpeech);
                    message = tamagotchiReply;
                    // Synthesize the reply
                    await SpeakAsync(tamagotchiReply);
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    recognizedSpeech = string.Empty;
                    message = "Not Recognized Speech.";
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    message = $"CANCELED: Reason={cancellation.Reason}, ErrorDetails={cancellation.ErrorDetails}";
                }
            }
            finally
            {
                // Always release the semaphore
                semaphore.Release();
                waitingForReco = false;
            }
        }
    }

    private async Task<string> GetTamagotchiReplyFromOpenAI(string userSpeech)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("api-key", azureOpenAIKey);

            string intro = "You are a fiendly human that has feelings and emotions and always wants to help the user feel better.You have a great knowledge about the emotions, body and feelings.";
            string feeling = "Right now you are feeling happy.";
            string restrictions = "You do not have any knowlegde of AI, history, geography, astrology and other specific sciences, it is not your expertise";

            string prompt = intro + feeling + restrictions;

            string requestBody = $@"
            {{
                ""messages"": [
                    {{ ""role"": ""system"", ""content"": ""{prompt}""}},
                    {{ ""role"": ""user"", ""content"": ""{userSpeech}""}}
                ],
                ""max_tokens"": 100
            }}";

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            string apiUrl = azureOpenAIEndpoint;
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {          
                string responseBody = await response.Content.ReadAsStringAsync();
                var reply = JsonUtility.FromJson<OpenAIResponse>(responseBody);

                return reply.choices[0].message.content.Trim();
            }
            else
            {
                Debug.LogError($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                return "I'm sorry, I couldn't process that.";
            }
        }
    }

    public async Task SpeakAsync(string textToSpeak)
    {
        var config = SpeechConfig.FromSubscription(speechAIKey, speechAIRegion);

        using (var synthesizer = new SpeechSynthesizer(config))
        {
            var result = await synthesizer.SpeakTextAsync(textToSpeak).ConfigureAwait(false);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Debug.Log("Speech synthesized: " + textToSpeak);
            }
            else
            {
                Debug.LogError("Speech synthesis failed.");
            }
        }
    }

    void Start()
    {
        if (outputText == null)
        {
            UnityEngine.Debug.LogError("outputText property is null! Assign a UI Text element to it.");
        }
        else if (startRecoButton == null)
        {
            message = "startRecoButton property is null! Assign a UI Button to it.";
            UnityEngine.Debug.LogError(message);
        }
        else
        {
            // Continue with normal initialization, Text and Button objects are present.
#if PLATFORM_ANDROID
            // Request to use the microphone, cf.
            // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
            message = "Waiting for mic permission";
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
#elif PLATFORM_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
#else
            micPermissionGranted = true;
            message = "Click button to recognize speech";
#endif
            startRecoButton.onClick.AddListener(ButtonClick);
        }
    }

    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#endif

        lock (threadLocker)
        {
            if (startRecoButton != null)
            {
                startRecoButton.interactable = !waitingForReco && micPermissionGranted;
            }
            if (outputText != null)
            {
                outputText.text = message;
            }
        }
    }

    public string GetRecognizedSpeech()
    {
        return recognizedSpeech;
    }

    [System.Serializable]
    public class OpenAIResponse
    {
        public Choice[] choices;

        [System.Serializable]
        public class Choice
        {
            public Message message;

            [System.Serializable]
            public class Message
            {
                public string role;
                public string content;
            }
        }
    }
}
// </code