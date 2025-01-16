using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EmotionSystem : MonoBehaviour
{

    public List<Emotion> emotions = new List<Emotion>();
    public string currentEmotion;

    public UnityEvent<string> OnEmotionChange;

    public Animator anim;

    // Cambiar la intensidad de una emoción
    public void AdjustEmotion(string name, float amount)
    {

        float amountToChange = 0;
        foreach (var emotion in emotions)
        {
            if (emotion.name == name)
            {
                float prevIntensity = emotion.intensity;
                emotion.intensity = Mathf.Clamp(emotion.intensity + amount, 0, 100);
                amountToChange = emotion.intensity - prevIntensity;
                break;
            }
        }

        int indexEmotion = 0;

        emotions.Sort((a, b) => a.intensity.CompareTo(b.intensity));

        while (amountToChange > 0)
        {

            if (emotions[indexEmotion].intensity > 0 && emotions[indexEmotion].name != name)

            {
                emotions[indexEmotion].intensity -= 1;
                amountToChange -= 1;
            }

            indexEmotion++;
            indexEmotion = indexEmotion % emotions.Count;
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
        ChangeAnimation(dominantEmotion.name);
        Debug.Log($"La emoción predominante es: {currentEmotion}");




    }


    public void ChangeAnimation(string emotion)
    {
        anim.CrossFade(emotion, 0.1f);
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

