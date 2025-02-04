using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    private string folderPath;
    private string filePath;

    public string selectedEmotion = "";

    private void Awake()
    {
        //folderPath = Application.persistentDataPath;
        folderPath = Application.dataPath + $"/SavedData";
    }
    public void SaveGameData(string gameName, string result)
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        filePath = folderPath + $"/{currentDate}.json";

        GameData gameData = new GameData
        {
            game = gameName,
            result = result,
            date = currentDate,
            time = currentTime
        };

        CombinedDataList combinedData = LoadCombinedData();
        combinedData.gameDataList.Add(gameData);

        SaveCombinedData(combinedData);
        Debug.Log($"Game data saved: {filePath}");
    }


    public void SelectEmotion(string emotion)
    {
        selectedEmotion = emotion;
    }

    public void SaveEmotion()
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        filePath = folderPath + $"/{currentDate}.json";

        EmotionData emotionData = new EmotionData
        {
            emotion = selectedEmotion,
            date = currentDate,
            time = currentTime
        };

        CombinedDataList combinedData = LoadCombinedData();
        combinedData.emotionDataList.Add(emotionData);

        SaveCombinedData(combinedData);
    }

    public string GetRecentEmotion()
    {
        CombinedDataList combinedData = LoadCombinedData();
        if (combinedData.emotionDataList.Count > 0)
        {
            // Recuperar la emoción más reciente
            return combinedData.emotionDataList[combinedData.emotionDataList.Count - 1].emotion;
        }
        return "neutral";
    }

    public string GetMostFrequentEmotion()
    {
        CombinedDataList combinedData = LoadCombinedData();
        Dictionary<string, int> emotionCount = new Dictionary<string, int>();

        foreach(var emotion in combinedData.emotionDataList)
        {
            if (emotionCount.ContainsKey(emotion.emotion))
            {
                emotionCount[emotion.emotion]++;
            }
            else
            {
                emotionCount[emotion.emotion] = 1;
            }
        }

        string mostFrequentEmotion = "";
        int max = 0;
        foreach(var emotion in emotionCount)
        {
            if (emotion.Value > max)
            {
                mostFrequentEmotion = emotion.Key;
                max = emotion.Value;
            }
        }

        return mostFrequentEmotion;
    }

    private CombinedDataList LoadCombinedData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<CombinedDataList>(json);
        }
        return new CombinedDataList();
    }

    private void SaveCombinedData(CombinedDataList data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public bool AnySavedDatainDate(string date)
    {
        filePath = folderPath + $"/{date}.json";
        return File.Exists(filePath);
       
    }

    public Dictionary<string, int> GetEmotionsFromDate(string date)
    {

        Dictionary<string, int> emotionCount = new Dictionary<string, int>()
            { { "Sad", 0 },  { "Happy", 0 }, { "Angry", 0 }, { "Surprised", 0 }, { "Sleepy", 0 }, { "Normal", 0 } };


        filePath = folderPath + $"/{date}.json";
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);
            CombinedDataList data = JsonUtility.FromJson<CombinedDataList>(fileContent);

            if (data != null && data.emotionDataList.Count > 0)
            {
                foreach (var emotion in data.emotionDataList)
                {
                    emotionCount[emotion.emotion]++;
                }

            }
        }
        return emotionCount;
    }

    public Dictionary<string, object> GetGamesPlayedFromDate(string date)
    {
        Dictionary<string, object> games = new Dictionary<string, object>
        {
            { "Tic Tac Toe", new Dictionary<string, int> { { "Won", 0 }, { "Lost", 0 }, { "Draw", 0 } } },
            { "Memory", 0 },
            { "Cups Ball", 0 }
        };
           
        string filePath = folderPath + $"/{date}.json";
        if (File.Exists(filePath))
        {
           string fileContent = File.ReadAllText(filePath);
           CombinedDataList data = JsonUtility.FromJson<CombinedDataList>(fileContent);

            if (data != null && data.gameDataList.Count > 0)
            {
                foreach (var gameData in data.gameDataList)
                {
                    string name = gameData.game;
                    string result = gameData.result;

                    if (name == "Tic Tac Toe")
                    {
                        var ticTacToeStats = games[name] as Dictionary<string, int>;
                        if (ticTacToeStats != null)
                        {
                            if (result.Contains("won")) ticTacToeStats["Won"]++;
                            else if (result.Contains("lost")) ticTacToeStats["Lost"]++;
                            else if (result.Contains("draw")) ticTacToeStats["Draw"]++;
                        }
                    }
                    else
                    {
                        if (games.ContainsKey(name))
                        {
                            games[name] = (int)games[name] + 1;
                        }
                    }
                }
            }
        }
        return games;
    }

}

[Serializable]
public class GameData
{
    public string game;
    public string result;
    public string date;
    public string time;
}

[Serializable]
public class EmotionData
{
    public string emotion;
    public string date;
    public string time;
}

[Serializable]
public class CombinedDataList
{
    public List<GameData> gameDataList = new List<GameData>();
    public List<EmotionData> emotionDataList = new List<EmotionData>();
}
