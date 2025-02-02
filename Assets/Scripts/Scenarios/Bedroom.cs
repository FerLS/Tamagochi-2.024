using UnityEngine;
using UnityEngine.UI;

public class Bedroom : MonoBehaviour
{
    [Header("Background")]
    public GameObject LightOutBackground;


    [Header("Sleep")]
    public GameObject SleepButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnLightOff()
    {
        LightOutBackground.SetActive(true);
        SleepButton.SetActive(false);
    }

    public void TurnLightOn()
    {
        LightOutBackground.SetActive(false);
        SleepButton.SetActive(true);
    }
}
