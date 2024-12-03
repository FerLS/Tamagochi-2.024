using UnityEngine;
using TMPro;

public class GlobalUi : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialScreen;
    public GameObject GameScreen;
    public GameObject SettingsScreen;
    public GameObject ReportsScreen;

    [Header("Inputs")]
    public TMP_InputField commentInputField; 

    private string selectedEmotion;
    private string enteredNote;



    private void Start()
    {
        if (commentInputField == null)
        {
            commentInputField = GameObject.Find("commentInputField").GetComponent<TMP_InputField>();

            if (commentInputField == null)
            {
                Debug.LogError("commentInputField could not be found in the scene!");
            }
            else
            {
                Debug.Log("commentInputField successfully assigned at runtime.");
            }
        }
    }

    public void EnterGameScreen()
    {
        SaveEmotionAndNote();
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

    private void SaveEmotionAndNote()
    {
        enteredNote = commentInputField.text;

        PlayerPrefs.SetString("EnteredNote", enteredNote);
        PlayerPrefs.Save();

        Debug.Log($"Comment Added: {enteredNote}");
    }

}
