using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using TMPro;
using System.Net.Http;
using System.Text;
using System.Linq;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class Speech : MonoBehaviour
{
    public TextMeshProUGUI outputText;

    public Button startRecoButton;

    public EmotionSystem emotionSystem;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;

    private bool micPermissionGranted = false;

    private GlobalUi globalUI;

    private string recognizedSpeech;

    private const string speechAIKey = "6mqkdKBT3AeTqyc1UBcniDEXdqQSFubYfHIxNwdPVDZQXAVBO5xQJQQJ99ALACYeBjFXJ3w3AAAYACOGhLrh";
    private const string speechAIRegion = "eastus";

    private const string azureOpenAIEndpoint = "https://11078-m3z4gxr9-eastus2.cognitiveservices.azure.com/openai/deployments/gpt-4/chat/completions?api-version=2024-08-01-preview";
    private const string azureOpenAIKey = "FbYZnPzs6qjYPhWOBgYlmTMVwNByahpOD8qjPrjXAhhK7ckLdNWkJQQJ99AKACHYHv6XJ3w3AAAAACOGiZ5N";
    private const string openAIModel = "gpt-4";


    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    [System.Serializable]
    public class ResponseData
    {
        public string response;
        public string feeling;
        public string intensity;
    }


    public async void ButtonClick()
    {
        var config = SpeechConfig.FromSubscription(speechAIKey, speechAIRegion);

        using (var recognizer = new SpeechRecognizer(config))
        {
            waitingForReco = true;

            await semaphore.WaitAsync();
            try
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedSpeech = result.Text;

                    string tamagotchiReply = await GetTamagotchiReplyFromOpenAI(recognizedSpeech);


                    print(tamagotchiReply);

                    var response = JsonUtility.FromJson<ResponseData>(tamagotchiReply);





                    message = response.response;
                    emotionSystem.AdjustEmotion(response.feeling, float.Parse(response.intensity));

                    await SpeakAsync(message);
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    recognizedSpeech = string.Empty;
                    message = "Sorry, can you repeat it?";
                    await SpeakAsync(message);
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    message = $"CANCELED: Reason={cancellation.Reason}, ErrorDetails={cancellation.ErrorDetails}";
                }
            }
            finally
            {
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

            string intro = "You are a fiendly human that has feelings and emotions and always wants to help the user feel better.The user is a child that has NDDs. You have a great knowledge about the emotions, body and feelings. ";
            string feeling = "Right now you are feeling happy. ";
            string treatment = "You must avoid making the child unconfortable and remarking his disorder. ";
            string restrictions = "You do not have any knowlegde of AI, history, geography, astrology and other specific sciences, it is not your expertise. Your answer should be short because the user can be easily distracted.";
            string format = $"The format should be a json, with 3 properties: response, feeling (from {string.Join(", ", emotionSystem.emotions.ConvertAll(e => e.name))}) and intensity (from 0 to 100).";

            string prompt = intro + feeling + treatment + restrictions + format;

            string requestBody = $@"
            {{
                ""messages"": [
                    {{ ""role"": ""system"", ""content"": ""{prompt}""}},
                    {{ ""role"": ""user"", ""content"": ""{userSpeech}""}}
                ],
                ""max_tokens"": 50
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

        RunOnMainThread(() => globalUI?.SetSpeechBubble(true));

        using (var synthesizer = new SpeechSynthesizer(config))
        {
            string ssml = $@"
            <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xmlns:mstts='http://www.w3.org/2001/mstts' xml:lang='en-US'>
                <voice name='en-US-BlueNeural'>
                    <prosody pitch='+18%' rate='1.1'>
                        {textToSpeak}
                    </prosody>
                </voice>
            </speak>";

            var result = await synthesizer.SpeakSsmlAsync(ssml).ConfigureAwait(false);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Debug.Log("Speech synthesized: " + textToSpeak);
            }
            else
            {
                Debug.LogError("Speech synthesis failed.");
            }
        }

        RunOnMainThread(() => globalUI?.SetSpeechBubble(false));
    }

    private void RunOnMainThread(System.Action action)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(action);
    }

    void Start()
    {
        globalUI = FindObjectOfType<GlobalUi>();

        if (globalUI == null)
        {
            Debug.LogError("GlobalUI component not found in the scene.");
        }

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
#if PLATFORM_ANDROID
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
#endif
            startRecoButton.onClick.AddListener(ButtonClick);

            StartCoroutine(SpeakGreeting());
        }
    }

    private IEnumerator SpeakGreeting()
    {
        yield return new WaitForSeconds(1f);

        string greeting = "Hi! Let's spend some time together.\nHow are you feeling today?";
        message = greeting;

        var speakTask = SpeakAsync(greeting);

        while (!speakTask.IsCompleted)
        {
            yield return null;
        }

        if (speakTask.Exception != null)
        {
            Debug.LogError("Error during speech synthesis: " + speakTask.Exception.Message);
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