using UnityEngine;
using UnityEngine.UI;

public class GlobalUi : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialScreen;
    public GameObject GameScreen;
    public GameObject SettingsScreen;
    public GameObject ReportsScreen;


    [Header("Panels")]
    public GameObject EmotionsPanel;
    private Image[] emotionFaces;


    [Header("Speech")]
    public GameObject speechBubble;

    private void Start()
    {
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

    public void SetEmotionFace(Image emotionFace)
    {
        foreach (var face in emotionFaces)
        {
            
            face.color = new Color(255, 255, 255, 0.5f);
        }
        emotionFace.color = new Color(255, 255, 255, 1);
    }


    public void SetSpeechBubble(bool isActive)
    {
        if (speechBubble != null)
        {
            speechBubble.SetActive(isActive);
        }
        else
        {
            Debug.LogError("SpeechBubble is not assigned in GlobalUI.");
        }
    }

}
