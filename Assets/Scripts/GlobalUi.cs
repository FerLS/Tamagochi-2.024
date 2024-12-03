using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlobalUi : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialScreen;
    public GameObject GameScreen;
    public GameObject SettingsScreen;
    public GameObject ReportsScreen;

    [Header("Inputs")]
    public TMP_InputField commentInputField;
    private string enteredNote;

    [Header("Emojis")]
    public Button[] emojiButtons; 
    private Button selectedEmojiButton; 
    private string selectedEmotion;

    



    private void Start()
    {
        if (commentInputField == null)
        {
            commentInputField = GameObject.Find("commentInputField").GetComponent<TMP_InputField>();
        }
        if (emojiButtons.Length > 0)
        {
            int lastIndex = emojiButtons.Length - 1;
            SelectEmoji(emojiButtons[lastIndex]); 
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

    public void SelectEmoji(Button selectedButton)
    {
        if (selectedEmojiButton != null)
        { 
            selectedEmojiButton.GetComponent<Image>().color = Color.white; 
        }

        selectedButton.GetComponent<Image>().color = Color.yellow; 
        selectedEmojiButton = selectedButton;

        selectedEmotion = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text; // Assuming the text is the emoji
    }


    private void SaveEmotionAndNote()
    {
        enteredNote = commentInputField.text;

        PlayerPrefs.SetString("SelectedEmotion", selectedEmotion);
        PlayerPrefs.SetString("EnteredNote", enteredNote);
        PlayerPrefs.Save();

        Debug.Log($"Emoción seleccionada: {selectedEmotion}, Nota ingresada: {enteredNote}");
    }

}
