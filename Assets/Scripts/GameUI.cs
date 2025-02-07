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

    [Header("Color Theme")]
    [SerializeField] private TamagochiUI elmo;





    [Header("Park")]
    [SerializeField] private GameObject parkScenario;
    [Header("Playroom")]
    [SerializeField] private GameObject playroomScenario;


    [Header("Bedroom")]
    [SerializeField] private GameObject bedroomScenario;


    [Header("Bathroom")]
    [SerializeField] private GameObject bathroomScenario;


    [Header("Kitchen")]
    [SerializeField] private GameObject kitchenScenario;

    private Color colorTheme;

    void Start()
    {
        ChangeScenary(bedroomScenario);
        HighlightScenarioButton(bedroomButton);
    }

    void Update()
    {
        if (elmo)
        {
            colorTheme = elmo.GetBodyColor();
        }
    }

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

    public void ChangeScenary(GameObject scenario)
    {
        parkScenario.SetActive(false);
        playroomScenario.SetActive(false);
        bedroomScenario.SetActive(false);
        bathroomScenario.SetActive(false);
        kitchenScenario.SetActive(false);

        scenario.gameObject.SetActive(true);

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

    public void HighlightScenarioButton(Button button)
    {
        UnhighlightButton(parkButton);
        UnhighlightButton(playroomButton);
        UnhighlightButton(bedroomButton);
        UnhighlightButton(bathroomButton);
        UnhighlightButton(kitchenButton);

        Image icon = button.GetComponentInChildren<Image>();
        if (icon)
        {
            //icon.color = colorTheme;
            icon.color = new Color(208f / 255f, 136f / 255f, 64f / 255f);
        }

    }

    private void UnhighlightButton(Button button)
    {
        Image icon = button.GetComponentInChildren<Image>();
        if (icon)
        {
            icon.color = new Color(88f / 255f, 88f / 255f, 88f / 255f);
        }

    }

    public void HighlightScenarioButton(Button button)
    {
        UnhighlightButton(parkButton);
        UnhighlightButton(playroomButton);
        UnhighlightButton(bedroomButton);
        UnhighlightButton(bathroomButton);
        UnhighlightButton(kitchenButton);

        Image icon = button.GetComponentInChildren<Image>();
        if (icon)
        {
            //icon.color = colorTheme;
            icon.color = new Color(208f / 255f, 136f / 255f, 64f / 255f);
        }

    }

    private void UnhighlightButton(Button button)
    {
        Image icon = button.GetComponentInChildren<Image>();
        if (icon)
        {
            icon.color = new Color(88f / 255f, 88f / 255f, 88f / 255f);
        }

    }


}
