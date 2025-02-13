using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class PlayingScript : MonoBehaviour
{
    [Header("Emotion System")]
    [SerializeField] private EmotionSystem emotionSystem;

    [Header("Values")]
    [SerializeField] private float increaseRate = 8f;
    [SerializeField] private bool canPlay = true;
    [SerializeField] private bool isTryingToPlay = false;

    [Header("Game Objects from Games")]
    [SerializeField] private GameObject park;
    [SerializeField] private GameObject tictactoe;
    [SerializeField] private GameObject memory;
    [SerializeField] private GameObject cupsBall;

    private float timeElapsed = 0f;
    private int index = 0;


    void Update()
    {
        if (emotionSystem)
        {
            validatePlayingRule();
            UpdateGameObjects();

            if (isTryingToPlay && canPlay)
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed >= 5f)
                {
                    if (index % 3 == 0)
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
        isTryingToPlay = (park.activeInHierarchy || tictactoe.activeInHierarchy || memory.activeInHierarchy || cupsBall.activeInHierarchy);
        canPlay = (emotionSystem.GetEnergy() >= 0.13);
    }

 
    private void UpdateGameObjects()
    {
        if(park.activeInHierarchy)
        {
            Transform ball = park.transform.Find("Ball"); 
            if (ball != null)
            {
                ball.gameObject.SetActive(canPlay); 
            }
        }
        DisableandEnableObject(tictactoe);
        DisableandEnableObject(memory);
        DisableandEnableObject(cupsBall);
    }

    private void DisableandEnableObject(GameObject game)
    {
        if (game == null || !game.activeInHierarchy) return;
        Button[] buttons = game.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            if (button.name == "Exit")
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = canPlay;
            }
        }

        Transform warningTransform = game.transform.Find("WarningSign");
        if (warningTransform != null)
        {
            GameObject warningSign = warningTransform.gameObject;
            warningSign.SetActive(!canPlay); 
        }
    }

}
