using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bedroom : MonoBehaviour
{
    [Header("Background")]
    public GameObject LightOutBackground;


    [Header("Buttons")]
    public GameObject SleepButton;
    public GameObject WakeUpButton;


    [Header("Emotion System")]
    public EmotionSystem EmotionSystem;

    private bool isSleeping = false;
    private Coroutine sleepCoroutine;

    void Start()
    {
        WakeUpButton.SetActive(false);
        SleepButton.SetActive(true);

        isSleeping = !SleepButton.activeSelf;
    }


    public void TurnLightOff()
    {
        isSleeping = true;
        LightOutBackground.SetActive(true);
        SleepButton.SetActive(false);
        WakeUpButton.SetActive(true);

        sleepCoroutine = StartCoroutine(IncreaseSleepyEmotion());
    }

    public void TurnLightOn()
    {
        isSleeping = false;
        LightOutBackground.SetActive(false);
        SleepButton.SetActive(true);
        WakeUpButton.SetActive(false);

        if (sleepCoroutine != null)
        {
            StopCoroutine(sleepCoroutine);
        }
    }

    private IEnumerator IncreaseSleepyEmotion()
    {
        while (isSleeping)
        {
            float currentIntensity = EmotionSystem.GetEmotionIntensity("Sleepy");
            EmotionSystem.AdjustEmotion("Sleepy", -(currentIntensity));

            yield return new WaitForSeconds(2f);  
        }
    }
}
