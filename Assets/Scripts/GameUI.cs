using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public static GameUI instance;

    [Header("Speech")]

    [SerializeField] private GameObject typeScreen;
    [SerializeField] private TMP_InputField typeInputField;
    [SerializeField] private Button yesButton;

    private Action questionEvent;

    public GameObject speechBubble;

    public TextMeshProUGUI outputText;






    [Header("Park")]
    [SerializeField] private GameObject parkScenary;
    [Header("Playroom")]
    [SerializeField] private GameObject playroomScenary;


    [Header("Bedroom")]
    [SerializeField] private GameObject bedroomScenary;


    [Header("Bathroom")]
    [SerializeField] private GameObject bathroomScenary;


    [Header("Kitchen")]
    [SerializeField] private GameObject kitchenScenary;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScenary(GameObject scenary)
    {
        parkScenary.SetActive(false);
        playroomScenary.SetActive(false);
        bedroomScenary.SetActive(false);
        bathroomScenary.SetActive(false);
        kitchenScenary.SetActive(false);

        scenary.gameObject.SetActive(true);

    }

    public void OpenTypeScreen()
    {

        typeScreen.SetActive(true);
        typeInputField.Select();
        typeInputField.text = "";
        typeInputField.ActivateInputField();
    }

    public void CloseTypeScreen()
    {
        typeScreen.SetActive(false);
        Speech.instance.OnTextSumbit(typeInputField.text);


    }

    public void Talk(bool isTalking, string message = "", bool isQuestion = false, Action questionEvent = null)
    {
        speechBubble.SetActive(isTalking);
        StartCoroutine(TypeText(message));


        if (isQuestion)
        {
            speechBubble.transform.GetChild(1).gameObject.SetActive(true);
            this.questionEvent = questionEvent;

        }
    }
    public IEnumerator TypeText(string message, float delay = 0.05f)
    {
        outputText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            outputText.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    public void OnNoButton()
    {


        Talk(false);
    }
    public void OnYesButton()
    {
        questionEvent?.Invoke();
        Talk(false);
    }


}
