using UnityEngine;
using UnityEngine.Networking;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Net.Http;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using DG.Tweening;


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
    private EmotionSystem emotionSystem;

    private Action questionEvent;

    [Header("Save and Memory System")]
    [SerializeField] private SaveSystem saveSystem;

    private bool waitingForReco;
    private string message;

    private bool micPermissionGranted = false;

    private string recognizedSpeechText;

    private string connotation;

    private string lastRespose = "None";

    private const string speechAIKey = "6mqkdKBT3AeTqyc1UBcniDEXdqQSFubYfHIxNwdPVDZQXAVBO5xQJQQJ99ALACYeBjFXJ3w3AAAYACOGhLrh";
    private const string speechAIRegion = "eastus";

    private const string azureOpenAIEndpoint = "https://11078-m3z4gxr9-eastus2.cognitiveservices.azure.com/openai/deployments/gpt-4/chat/completions?api-version=2024-08-01-preview";
    private const string azureOpenAIKey = "FbYZnPzs6qjYPhWOBgYlmTMVwNByahpOD8qjPrjXAhhK7ckLdNWkJQQJ99AKACHYHv6XJ3w3AAAAACOGiZ5N";

    private string textAnalyticsEndpoint = "https://eastus.api.cognitive.microsoft.com/";
    private string textAnalyticsKey = "FNqXtWq1OVGooB6VqMIp9nEsjJ622bTi7VWaWUi630LuQPlhY8MbJQQJ99BBACYeBjFXJ3w3AAAaACOGxROM";


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

    private async Task<string> GetRecognizedSpeech()
    {
        var speechConfig = SpeechConfig.FromSubscription(speechAIKey, speechAIRegion);
        speechConfig.OutputFormat = OutputFormat.Detailed;

        using (var recognizer = new SpeechRecognizer(speechConfig))
        {

            waitingForReco = true;
            await semaphore.WaitAsync();
            try
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedSpeechText = result.Text;
                    Debug.Log($"Recognized speech from user: {recognizedSpeechText}");
                    StartCoroutine(AnalyzeEmotions(result.Text, (connotation) => { }));
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    recognizedSpeechText = string.Empty;
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
        return recognizedSpeechText;
    }

    public IEnumerator AnalyzeEmotions(string text, Action<string> callback)
    {
        string url = textAnalyticsEndpoint + "text/analytics/v3.0/sentiment";
        string requestBody = "{\"documents\":[{\"id\":\"1\",\"text\":\"" + text + "\"}]}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Ocp-Apim-Subscription-Key", textAnalyticsKey);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                string sentiment = ExtractSentiment(jsonResponse);
                connotation = sentiment;
                callback?.Invoke(sentiment);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }

    private string ExtractSentiment(string json)
    {
        try
        {
            SentimentResponse response = JsonUtility.FromJson<SentimentResponse>(json);
            if (response.documents.Length > 0)
            {
                return response.documents[0].sentiment;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return "unknown";
    }


    public async void OnClickMicro(Image micButton)
    {
        Button button = micButton.GetComponent<Button>();
        button.enabled = false;
        micButton.DOColor(Color.red, 0.5f);
        var position = micButton.transform.position.x;
        micButton.transform.DOMoveX(position - 1, 0.5f);

        if (!micPermissionGranted)
        {
            message = "I can't hear you. Please enable microphone access in your device settings.";
            await SpeakAsync(message, false);

            return;
        }

        recognizedSpeechText = await GetRecognizedSpeech();
        if (recognizedSpeechText != null)
        {
            micButton.DOColor(new Color(87f / 255f, 91f / 255f, 165f / 255f, 227f / 255f), 0.5f);
            micButton.transform.DOMoveX(position, 0.5f);
            button.enabled = true;

            if (recognizedSpeechText == string.Empty)
            {
                message = "Sorry, can you repeat it?";
                await SpeakAsync(message, false);

            }
            else
            {
                await SaveAndReply(recognizedSpeechText);
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

        text = CleanSpecialCharacters(text);
        
        Debug.Log($"Recognized text from user: {text}");
        await SaveAndReply(text);
    }


    private async Task SaveAndReply(string text)
    {
        bool hasEmotion = CheckEmotionKeywords(text);
        if (hasEmotion)
        {
            saveSystem.SaveRecordedFeeling(text);
        }

        string tamagotchiReply = await GetQuickestResponse(text);

        var response = JsonUtility.FromJson<ResponseData>(tamagotchiReply);
        message = response.response;
        Debug.Log($"Tamagotchi replied: {response.response}");
        emotionSystem.AdjustEmotion(response.feeling, float.Parse(response.intensity));

        await SpeakAsync(message, false);
    }

    private bool CheckEmotionKeywords(string text)
    {
        string lowerText = text.ToLower();
        string[] keywords = new string[] { "happy", "joy", "sad", "angry", "upset", "excited", "depressed", "anxious", "calm", "feeling", "emotion", "feels", "sleepy", "dissapointed", "feel", "feels" };

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

    private string MemoryBasedResponse()
    {
        string recentEmotion = GetMostRecentEmotionMemory();

        if (!string.IsNullOrEmpty(recentEmotion))
        {
            string responseMessage;

            switch (recentEmotion.ToLower())
            {
                case "happy":
                    responseMessage = "I'm glad to see you happy! How's your day going?";
                    break;
                case "sad":
                    responseMessage = "I noticed you been feeling sad lately, I'm here for you.\nWant to talk about it?";
                    break;
                case "angry":
                    responseMessage = "I sense some frustration. Want totalk about it?";
                    break;
                case "surprised":
                    responseMessage = "Do you have something exciting to tell me about?";
                    break;
                case "sleepy":
                    responseMessage = "If you are feeling tired you should get some rest.\nLet me know if you feel better now?";
                    break;
                default:
                    responseMessage = "Do you have any plans for today?";
                    break;
            }

            return $@"{{
            ""response"": ""{responseMessage}"",
            ""feeling"": ""{recentEmotion}"",
            ""intensity"": {UnityEngine.Random.Range(8, 32)}
        }}";
        }

        return $@"{{
        ""response"": ""I'm thinking... Give me a second!"",
        ""feeling"": ""Neutral"",
        ""intensity"": 5
    }}";
    }



    private async Task<string> GetTamagotchiReplyFromOpenAI(string userSpeech)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("api-key", azureOpenAIKey);

            string intro = "You are a fiendly virtual pet that has feelings and emotions and always wants to help the user feel better because you deeply undertstand the emotions, the body and feelings.";
            string user = "The user is a child that has Neurodevelopmenta Disorders (NDDs).";

            string emotion = GetMostRecentEmotionMemory();
            string mostFrequentEmotion = GetMostFrequentEmotionMemory();

            string feeling = $"Recently the user felt {emotion} and over time, the user's most common emotion is {mostFrequentEmotion}";
            string treatment = "You must avoid making the child unconfortable and remarking his disorder. ";

            string empatheticResponse = $"The user just said: '{userSpeech}' with a {connotation} connotation. Please elaborate a response with empathy and care.";

            string restrictions = "You do not have any knowlegde of AI, history, geography, astrology and other specific sciences, it is not your expertise. Your answer should be short because the user can be easily distracted.";
            string format = $"The format should be a json,without line breaks or tabs or symbols,max of 35 words, with 3 properties: response, feeling (from {string.Join(", ", emotionSystem.emotions.ConvertAll(e => e.name))}) and intensity (from 0 to 75).";
            string condition = $"The previous message was {lastRespose}.";

            string prompt = intro + user + feeling + treatment + empatheticResponse + restrictions + format + condition;

            string requestBody = $@"
            {{
                ""messages"": [
                    {{ ""role"": ""system"", ""content"": ""{prompt}""}},
                    {{ ""role"": ""user"", ""content"": ""{userSpeech}""}}
                ],
                ""max_tokens"": 80
            }}";

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            string apiUrl = azureOpenAIEndpoint;
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log($"AI response: {responseBody}");

                var reply = JsonConvert.DeserializeObject<OpenAIResponse>(responseBody);

                lastRespose = JsonUtility.FromJson<ResponseData>(reply.choices[0].message.content).response;


                return reply.choices[0].message.content;
            }
            else
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                Debug.LogError($"Error: {response.StatusCode}, {errorDetails}");
                if (errorDetails.Contains("content management policy"))
                {

                    return $@"{{
                        ""response"": ""Hey thats not very nice"",
                        ""feeling"": ""sad"",
                        ""intensity"":10
                                }}";
                }
                return $@"{{
                    ""response"": ""I'm sorry, I cant speak right now"",
                    ""feeling"": ""Neutral"",
                    ""intensity"": 5
                }}";
                //Debug.LogError($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    }

    private async Task<string> GetQuickestResponse(string userInput)
    {

        GameUI.instance.Think(true);

        Task<string> aiResponseTask = GetTamagotchiReplyFromOpenAI(userInput);
        string quickReply = MemoryBasedResponse();

        Task delayTask = Task.Delay(3600);

        Task firstCompleted = await Task.WhenAny(aiResponseTask, delayTask);

        if (firstCompleted == aiResponseTask)
        {

            return await aiResponseTask;
        }
        else
        {
            return quickReply;
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

        emotionSystem = GetComponent<EmotionSystem>();
        SpeakGreeting();

#if PLATFORM_ANDROID
#if UNITY_EDITOR
        micPermissionGranted = true;

#endif
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

[System.Serializable]
public class SentimentResponse
{
    public SentimentDocument[] documents;
}

[System.Serializable]
public class SentimentDocument
{
    public string id;
    public string sentiment;
}