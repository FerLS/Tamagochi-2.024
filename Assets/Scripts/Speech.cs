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
using System;
using UnityEngine.Events;


#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class Speech : MonoBehaviour
{

    public static Speech instance;
    [Header("Emotions")]
    public EmotionSystem emotionSystem;

    private Action questionEvent;

    [Header("Save System")]
    public SaveSystem saveSystem;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;

    private bool micPermissionGranted = false;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<string> GetRecognizedSpeech()
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
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    recognizedSpeech = string.Empty;
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    message = $"CANCELED: Reason={cancellation.Reason}, ErrorDetails={cancellation.ErrorDetails}";
                    return null;
                }
            }
            finally
            {
                semaphore.Release();
                waitingForReco = false;
            }
        }
        return recognizedSpeech;
    }

    public async void OnClickMicro()
    {

        if (!micPermissionGranted)
        {
            message = "I can't hear you. Please enable microphone access in your device settings.";
            return;


        }
        recognizedSpeech = await GetRecognizedSpeech();
        if (recognizedSpeech != null)
        {
            if (recognizedSpeech == string.Empty)
            {
                message = "Sorry, can you repeat it?";
                await SpeakAsync(message, false);
            }
            else
            {
                bool hasEmotion = CheckEmotionKeywords(recognizedSpeech);
                if (hasEmotion)
                {
                    saveSystem.SaveRecordedFeeling(recognizedSpeech);
                }
                string tamagotchiReply = await GetTamagotchiReplyFromOpenAI(recognizedSpeech);
                print(tamagotchiReply);

                var response = JsonUtility.FromJson<ResponseData>(tamagotchiReply);


                message = response.response;
                emotionSystem.AdjustEmotion(response.feeling, float.Parse(response.intensity));

                await SpeakAsync(message, false);
            }

        }
    }


    public async void OnTextSumbit(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            message = "Please, write something.";
            return;
        }

        //Clean special characters

        text = CleanSpecialCharacters(text);


        string tamagotchiReply = await GetTamagotchiReplyFromOpenAI(text);
        print(tamagotchiReply);

        var response = JsonUtility.FromJson<ResponseData>(tamagotchiReply);

        message = response.response;
        emotionSystem.AdjustEmotion(response.feeling, float.Parse(response.intensity));

        await SpeakAsync(message, false);
    }

    private bool CheckEmotionKeywords(string text)
    {
        string lowerText = text.ToLower();
        string[] keywords = new string[] { "happy", "joy", "sad", "angry", "upset", "excited", "depressed", "anxious", "calm", "feeling", "emotion", "feels", "sleepy", "dissapointed"};

        foreach (string keyword in keywords)
        {
            if (lowerText.Contains(keyword))
            {
                return true;
            }
        }
        return false;
    }

    private string GetMostRecentEmotionMemory()
    {
        string recentEmotion = saveSystem.GetRecentEmotion();
        return recentEmotion;
    }

    private string GetMostFrequentEmotionMemory()
    {
        string mostFrequentEmotion = saveSystem.GetMostFrequentEmotion();
        return mostFrequentEmotion;
    }


    private async Task<string> GetTamagotchiReplyFromOpenAI(string userSpeech)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("api-key", azureOpenAIKey);

            string intro = "You are a fiendly human that has feelings and emotions and always wants to help the user feel better.The user is a child that has NDDs. You have a great knowledge about the emotions, body and feelings. ";

            string emotion = GetMostRecentEmotionMemory();
            string mostFrequentEmotion = GetMostFrequentEmotionMemory();

            string feeling = $"Recently the user felt {emotion} and over time, the user's most common emotion is {mostFrequentEmotion}";

            string treatment = "You must avoid making the child unconfortable and remarking his disorder. ";
            string restrictions = "You do not have any knowlegde of AI, history, geography, astrology and other specific sciences, it is not your expertise. Your answer should be short because the user can be easily distracted.";
            string format = $"The format should be a json, with 3 properties: response, feeling (from {string.Join(", ", emotionSystem.emotions.ConvertAll(e => e.name))}) and intensity (from 0 to 75).";

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
                //Debug.LogError($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                return "I'm sorry, I couldn't process that.";
            }
        }
    }



    public async Task SpeakAsync(string textToSpeak, bool isQuestion = false, Action speechEvent = null)
    {
        var config = SpeechConfig.FromSubscription(speechAIKey, speechAIRegion);

        message = textToSpeak;
        RunOnMainThread(() => GameUI.instance.Talk(true, textToSpeak, isQuestion, speechEvent));


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

        if (!isQuestion)
        {
            RunOnMainThread(() => GameUI.instance.Talk(false));
        }

    }

    private void RunOnMainThread(System.Action action)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(action);
    }

    void Start()
    {


#if PLATFORM_ANDROID
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

        //StartCoroutine(SpeakGreeting());
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

    string CleanSpecialCharacters(string text)
    {

        string cleanText = text;
        cleanText = cleanText.Replace("á", "a");
        cleanText = cleanText.Replace("é", "e");
        cleanText = cleanText.Replace("í", "i");
        cleanText = cleanText.Replace("ó", "o");
        cleanText = cleanText.Replace("ú", "u");
        cleanText = cleanText.Replace("Á", "A");
        cleanText = cleanText.Replace("É", "E");
        cleanText = cleanText.Replace("Í", "I");
        cleanText = cleanText.Replace("Ó", "O");
        cleanText = cleanText.Replace("Ú", "U");
        cleanText = cleanText.Replace("ñ", "n");
        cleanText = cleanText.Replace("Ñ", "N");
        cleanText = cleanText.Replace("¿", " ");
        cleanText = cleanText.Replace("?", " ");
        cleanText = cleanText.Replace("\"", " ");
        cleanText = cleanText.Replace("!", " ");
        cleanText = cleanText.Replace("-", " ");
        cleanText = cleanText.Replace("*", " ");
        cleanText = cleanText.Replace("/", " ");
        cleanText = cleanText.Replace("\\", " ");
        cleanText = cleanText.Replace("|", " ");
        cleanText = cleanText.Replace("_", " ");
        cleanText = cleanText.Replace("°", " ");
        cleanText = cleanText.Replace("ª", "");
        cleanText = cleanText.Replace("·", "");
        cleanText = cleanText.Replace("¬", "");

        return cleanText;


    }




    /* 
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

        }
    */
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