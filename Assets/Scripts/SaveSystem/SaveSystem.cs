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

   

    /* // Guardar datos
    public static void SaveGame(GameData data)
    {

        string filePath = Application.persistentDataPath + "/" + data.GetType().Name + ".json";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Datos guardados en {filePath}");
    }

    // Cargar datos
    public static GameData LoadGame(string dataType)
    {
        string filePath = Application.persistentDataPath + "/" + dataType + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Datos cargados correctamente");
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado.");
            return null; // O puedes retornar datos predeterminados
        }
    }*/
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
