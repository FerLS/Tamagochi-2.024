using UnityEngine;
using System;
using System.IO;

public class EmotionSelector : MonoBehaviour
{
    public string selectedEmotion = "";

    public void SelectEmotion(string emotion)
    {
        selectedEmotion = emotion;
    }

    public void SaveEmotion()
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        EmotionData emotionData = new EmotionData
        {
            emotion = selectedEmotion,
            date = currentDate
        };

        Debug.Log($"Emoción and date: {emotionData.emotion} & {emotionData.date}");

        string json = JsonUtility.ToJson(emotionData, true);

        string filePath = Application.dataPath + $"/SavedData/{currentDate}.json";
        //string filePath = Application.persistentDataPath + $"/{currentDate}.json";

        File.WriteAllText(filePath, json);

        Debug.Log($"Emoción guardada en el archivo: {filePath}");
    }
}

[Serializable]
public class EmotionData
{
    public string emotion;
    public string date;
}
