using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

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
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        EmotionData emotionData = new EmotionData
        {
            emotion = selectedEmotion,
            date = currentDate,
            time = currentTime
        };

        string filePath = Application.dataPath + $"/SavedData/{currentDate}.json";

        List<EmotionData> emotionList = new List<EmotionData>();

        // Si el archivo ya existe, leer los datos existentes
        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);
            emotionList = JsonUtility.FromJson<EmotionDataList>(existingJson).emotionDataList;
        }

        // Agregar el nuevo EmotionData a la lista
        emotionList.Add(emotionData);

        // Serializar la lista completa de EmotionData
        EmotionDataList emotionDataList = new EmotionDataList { emotionDataList = emotionList };
        string json = JsonUtility.ToJson(emotionDataList, true);

        // Guardar el archivo con los datos actualizados
        File.WriteAllText(filePath, json);

        Debug.Log($"Saved on File: {filePath}");
    }
}

[Serializable]
public class EmotionData
{
    public string emotion;
    public string date;
    public string time;
}

[Serializable]
public class EmotionDataList
{
    public List<EmotionData> emotionDataList;
}
