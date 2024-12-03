using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmotionSystem : MonoBehaviour
{

    public List<Emotion> emotions = new List<Emotion>();
    public string currentEmotion;

    public UnityEvent<string> OnEmotionChange;

    // Cambiar la intensidad de una emoción
    public void AdjustEmotion(string name, float amount)
    {
        foreach (var emotion in emotions)
        {
            if (emotion.name == name)
            {
                emotion.intensity = Mathf.Clamp(emotion.intensity + amount, 0, 100);
                break;
            }
        }

        UpdateCurrentEmotion();
    }

    // Determinar la emoción predominante
    private void UpdateCurrentEmotion()
    {
        Emotion dominantEmotion = emotions[0];

        foreach (var emotion in emotions)
        {
            if (emotion.intensity > dominantEmotion.intensity)
            {
                dominantEmotion = emotion;
            }
        }

        currentEmotion = dominantEmotion.name;
        OnEmotionChange.Invoke(currentEmotion);
        Debug.Log($"La emoción predominante es: {currentEmotion}");

    }


}


[System.Serializable]
public class Emotion
{
    public string name;
    public float intensity;

    public Emotion(string name, float intensity)
    {
        this.name = name;
        this.intensity = intensity;
    }
}

