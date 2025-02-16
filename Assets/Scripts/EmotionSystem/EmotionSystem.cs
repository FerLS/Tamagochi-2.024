using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EmotionSystem : MonoBehaviour
{
    [Header("Emotions")]
    public List<Emotion> emotions = new List<Emotion>();
    public string currentEmotion;

    [Header("Energy Bar")]
    public EnergyBar energyBar; 

    
    [Header("Save System")]
    public SaveSystem saveSystem; 
    private string recentEmotion;


    [Header("Events and Animations")]
    public UnityEvent<string> OnEmotionChange;
    public Animator anim;
    public bool isSleeping = false;


    private const float TotalPercentage = 100f;

    void Start()
    {
        recentEmotion = saveSystem.GetRecentEmotion();
        InitializeEmotions();
    }

    void Update()
    {
        ChangeAnimation(currentEmotion); ;
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
                break;

            }
            
        }
        if (targetEmotion != null)
        {
            DistributeAdjustment(targetEmotion, totalAdjustment);
        }
        UpdateCurrentEmotion();

        AdjustEnergyBar();
    }

    public float GetEmotionIntensity(string name)
    {
        float intensity = 0;
        foreach (var emotion in emotions)
        {
            if (emotion.name == name)
            {
                intensity = emotion.intensity;
            }
        }
        return intensity;
    }

    public void ChangeAnimation(string emotion)
    {
        if (emotion=="Sleepy" && GetEnergy()>0.35)
        {
            return;
        }
        else
        {
            anim.CrossFade(emotion, 0.1f);
        }
        
    }

    private void InitializeEmotions()
    {
        int random = Random.Range(0,100);
        if (random%3==0){
            int amount = emotions.Count;
            float predominantPercentage = TotalPercentage / amount + 10;
            float percentage = (TotalPercentage-predominantPercentage) / (amount-1);
            foreach (var emotion in emotions)
            {
                if (emotion.name == recentEmotion || (emotion.name=="Neutral" && recentEmotion=="Normal")){
                    emotion.intensity = predominantPercentage;
                }else{
                    emotion.intensity = percentage;
                }   
                
            }
        }else{
            int amount = emotions.Count;
            float percentage = TotalPercentage / amount;
            foreach (var emotion in emotions)
            {
                emotion.intensity = percentage;
            }
        }
        
        
        UpdateCurrentEmotion();

        AdjustEnergyBar();
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

        if (correction > 0)
        {
            int randomEmotion = Random.Range(0, emotions.Count);
            emotions[randomEmotion].intensity = Mathf.Clamp(emotions[randomEmotion].intensity + correction, 0, TotalPercentage);
        }
    }


    private void UpdateCurrentEmotion()
    {
        if (isSleeping)
        {
            return;
        }

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
    }

    private void AdjustEnergyBar()
    {
        float positiveEmotions = 0;
        float sleepy = 0;
        float relation = 0;

        foreach (var emotion in emotions)
        {
            if (emotion.name == "Happy" 
                || emotion.name == "Surprised" 
                || emotion.name == "Neutral")
            {
                positiveEmotions += emotion.intensity;
            }
            if (emotion.name == "Sleepy")
            {
                sleepy += emotion.intensity;
            }
        }

        relation = 1 - (sleepy / positiveEmotions);

        energyBar.SetEnergy(relation);

    }
    
    public void Sleep()
    {
        isSleeping = true;
        anim.SetBool("IsSleeping", true);
    }

    public void WakeUp()
    {
        isSleeping = false;
        anim.SetBool("IsSleeping", false);
    }

    public float GetEnergy()
    {
        return energyBar.GetEnergy();
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

