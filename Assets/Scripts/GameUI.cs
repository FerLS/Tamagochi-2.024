using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public static GameUI instance;

    [Header("Speech")]

    [SerializeField] private GameObject typeScreen;
    [SerializeField] private TMP_InputField typeInputField;

    public GameObject speechBubble;

    public TextMeshProUGUI outputText;

    [Header("Color Theme")]
    [SerializeField] private TamagochiUI elmo;

    [Header("Park")]
    [SerializeField] private GameObject parkScenario;
    [SerializeField] private Button parkButton;

    [Header("Playroom")]
    [SerializeField] private GameObject playroomScenario;
    [SerializeField] private Button playroomButton;

    [Header("Bedroom")]
    [SerializeField] private GameObject bedroomScenario;
    [SerializeField] private Button bedroomButton;

    [Header("Bathroom")]
    [SerializeField] private GameObject bathroomScenario;
    [SerializeField] private Button bathroomButton;

    [Header("Kitchen")]
    [SerializeField] private GameObject kitchenScenario;
    [SerializeField] private Button kitchenButton;

    private Button lastClickedButton;
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

    public void Talk(bool isTalking, string message = "")
    {
        speechBubble.SetActive(isTalking);
        outputText.text = message;
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
