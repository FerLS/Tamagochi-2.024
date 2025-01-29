using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EmotionSystem : MonoBehaviour
{
    [Header("Emotions")]
    public List<Emotion> emotions = new List<Emotion>();
    public string currentEmotion;

    [Header("Events and Animations")]
    public UnityEvent<string> OnEmotionChange;
    public Animator anim;


    private const float TotalPercentage = 100f;

    void Start()
    {
        InitializeEmotions();
    }

    private void InitializeEmotions()
    {
        int amount = emotions.Count;
        float initialPercentage = TotalPercentage / amount; 
        foreach (var emotion in emotions)
        {
            emotion.intensity = initialPercentage;
        }
        UpdateCurrentEmotion();
    }

    public void AdjustEmotion(string name, float amount)
    {
        Emotion targetEmotion = null;
        float totalAdjustment = 0f;

        foreach (var emotion in emotions)
        {
            if (emotion.name == name)
            {
                targetEmotion = emotion;
                amount = amount / 100;
                float newIntensity = Mathf.Clamp(emotion.intensity + amount * emotion.intensity, 0, TotalPercentage);
                totalAdjustment = newIntensity - emotion.intensity;
                emotion.intensity = newIntensity;
                Debug.Log($"Emotion: {emotion.name}, New Intensity: {newIntensity}, Adjustment: {totalAdjustment}");
                break;
            }
            
        }
        if (targetEmotion != null)
        {
            DistributeAdjustment(targetEmotion, totalAdjustment);
            UpdateCurrentEmotion();
            ChangeAnimation(name);
        }
        UpdateCurrentEmotion();
    }

    private void DistributeAdjustment(Emotion adjustedEmotion, float adjustment)
    {
        //float totalIntensity = TotalPercentage - adjustedEmotion.intensity;
        //float adjustmentPerEmotion = totalIntensity == 0 ? 0 : adjustment / totalIntensity;

        float adjustmentPerEmotion = adjustment / (emotions.Count - 1);
        foreach (var emotion in emotions)
        {
            if (emotion != adjustedEmotion)
            {
                float newIntensity = emotion.intensity - adjustmentPerEmotion;
                //float newIntensity = Mathf.Clamp(emotion.intensity - adjustmentPerEmotion * emotion.intensity, 0, TotalPercentage);
                emotion.intensity = newIntensity;
            }
        }
        NormalizePercentages();
    }

    private void NormalizePercentages()
    {
        float total = 0f;
        foreach (var emotion in emotions)
        {
            total += emotion.intensity;
        }

        float correction = TotalPercentage - total;

        if (emotions.Count > 0)
        {
            int randomEmotion = Random.Range(0, emotions.Count);
            Debug.Log(randomEmotion);
            emotions[randomEmotion].intensity = Mathf.Clamp(emotions[randomEmotion].intensity + correction, 0, TotalPercentage);
        }
    }


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

