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


    [Header("Park")]
    [SerializeField] private GameObject parkScenary;
    [SerializeField] private Button parkButton;

    [Header("Playroom")]
    [SerializeField] private GameObject playroomScenary;
    [SerializeField] private Button playroomButton;

    [Header("Bedroom")]
    [SerializeField] private GameObject bedroomScenary;
    [SerializeField] private Button bedroomButton;

    [Header("Bathroom")]
    [SerializeField] private GameObject bathroomScenary;
    [SerializeField] private Button bathroomButton;

    [Header("Kitchen")]
    [SerializeField] private GameObject kitchenScenary;
    [SerializeField] private Button kitchenButton;

    private Button lastClickedButton;

    void Start()
    {
        HighlightButton(bedroomButton);
    }

<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes
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

public void Talk(bool isTalking, string message = "")
{
    speechBubble.SetActive(isTalking);
    outputText.text = message;
}

}
