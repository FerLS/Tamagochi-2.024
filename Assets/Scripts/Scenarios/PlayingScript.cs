using UnityEngine;

public class PlayingScript : MonoBehaviour
{
    [Header("Emotion System")]
    [SerializeField] private EmotionSystem emotionSystem;

    [Header("Values")]
    [SerializeField] private float increaseRate = 8f;
    [SerializeField] private bool canPlay = true;

    [Header("Game Objects to Enable/Disable")]
    [SerializeField] private GameObject playroomGames;
    [SerializeField] private GameObject park;

    private float timeElapsed = 0f;
    private int index = 0;

    void Start()
    {
        validatePlayingRule();
        UpdateGameObjects();
    }

    void Update()
    {
        if (emotionSystem && canPlay)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= 5f)
            {
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

            validatePlayingRule();
            UpdateGameObjects();
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

    private void validatePlayingRule()
    {
        canPlay = (emotionSystem.GetEnergy() >= 0.1);
    }

    private void UpdateGameObjects()
    {
        if (playroomGames.activeSelf)
        {

        }

        if (park)
        {
            GameObject ball = park.transform.Find("Ball").gameObject;
            if (ball) ball.SetActive(canPlay);
        }
    }
}
