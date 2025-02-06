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
        InitialScreen.SetActive(true) ;
        ReportsScreen.SetActive(false);
    }

    public void EnterSettingsScreen()
    {
        GameScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }

    public void ExitSettingsScreen()
    {
        SettingsScreen.SetActive(false);
        GameScreen.SetActive(true);
    }

    public void ShowPreviousScreen()
    {
        Debug.Log(previousScreen);
        previousScreen.SetActive(true);
    }

    public void SetPreviousScreen(GameObject screen)
    {
        Debug.Log(screen);
        previousScreen = screen;
        previousScreen.SetActive(false);
    }

    public void HideScreen(GameObject screen)
    {
        Debug.Log(screen);
        screen.SetActive(false);
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
