using UnityEngine;

public class ParkScript : MonoBehaviour
{
    [Header("Emotion System")]
    [SerializeField] private EmotionSystem emotionSystem;

    private float increaseRate = 8f; 
    private float timeElapsed = 0f;
    private int index = 0;

    void Update()
    {
        if (emotionSystem)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= 5f)
            {
                Debug.Log(index);
                if (index%3 == 0)
                {
                    IncreaseHappy();
                }
                else
                {
                    IncreaseSleepy();
                }
                index++;
                timeElapsed = 0f; 
            }
        }
    }
    private void IncreaseSleepy()
    {
        float currentIntensity = emotionSystem.GetEmotionIntensity("Sleepy");
        if (currentIntensity < 100) 
        { 
            emotionSystem.AdjustEmotion("Sleepy", increaseRate);
        }
        
    }

    private void IncreaseHappy()
    {
        float currentIntensity = emotionSystem.GetEmotionIntensity("HappyS");
        if (currentIntensity < 100)
        {
            emotionSystem.AdjustEmotion("Happy", increaseRate);
        }

    }
}
