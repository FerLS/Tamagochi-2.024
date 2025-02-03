using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GlobalUi : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialScreen;
    public GameObject GameScreen;
    public GameObject SettingsScreen;
    public GameObject ReportsScreen;

    private GameObject previousScreen;

    [Header("Panels")]
    public GameObject EmotionsPanel;
    private Image[] emotionFaces;

    private void Start()
    {
        previousScreen = GameScreen;
        emotionFaces = EmotionsPanel.GetComponentsInChildren<Image>(true);
    }

    public void EnterGameScreen()
    {
        InitialScreen.SetActive(false);
        GameScreen.SetActive(true);
    }

    public void EnterReportsScreen()
    {
        ReportsScreen.SetActive(true);
    }

    public void ExitReportsScreen()
    {
        ReportsScreen.SetActive(false);
    }

    public void EnterSettingsScreen()
    {
        SettingsScreen.SetActive(true);
    }

    public void ExitSettingsScreen()
    {

        SettingsScreen.SetActive(false);
    }

    public void SetPreviousScreen()
    {
        previousScreen.SetActive(true);
    }

    public void HideScreen(GameObject actualScreen)
    {
        previousScreen = actualScreen;
        actualScreen.SetActive(false);
    }

    public void SetEmotionFace(Image emotionFace)
    {
        foreach (var face in emotionFaces)
        {

            face.color = new Color(255, 255, 255, 0.5f);
        }
        emotionFace.color = new Color(255, 255, 255, 1);
    }

}
