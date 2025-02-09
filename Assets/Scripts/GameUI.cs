using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [Header("Speech")]
    [SerializeField]
    private GameObject typeScreen;

    [SerializeField]
    private TMP_InputField typeInputField;

    [SerializeField]
    private Button yesButton;

    private Action questionEvent;

    public GameObject speechBubble;

    public TextMeshProUGUI outputText;

    [Header("Color Theme")]
    [SerializeField]
    private TamagochiUI elmo;

    [Header("Park")]
    [SerializeField]
    private GameObject parkScenario;
    [SerializeField] private Vector3[] parkPositions;

    [Header("Playroom")]
    [SerializeField]
    private GameObject playroomScenario;
    [SerializeField] private Vector3[] playRoomPositions;


    [Header("Bedroom")]
    [SerializeField]
    private GameObject bedroomScenario;
    [SerializeField] private Vector3[] bedroomPositions;

    [Header("Bathroom")]
    [SerializeField]
    private GameObject bathroomScenario;

    [Header("Kitchen")]
    [SerializeField]
    private GameObject kitchenScenario;


    [SerializeField]
    private Image[] scnearioButtons;

    public float delayBetWalk = 3f;

    void Start()
    {
        ChangeScenary(bedroomScenario, true);
        HighlightScenarioButton(scnearioButtons[0]);
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
        ChangeScenary(scenario, false);
    }

    public void ChangeScenary(GameObject scenario, bool noTrans)
    {
        Action actionInMiddle = () =>
        {
            parkScenario.SetActive(false);
            playroomScenario.SetActive(false);
            bedroomScenario.SetActive(false);
            bathroomScenario.SetActive(false);
            kitchenScenario.SetActive(false);
            scenario.gameObject.SetActive(true);
            elmo.OnChangeScenario();
        };
        if (noTrans)
        {
            actionInMiddle();
        }
        else
        {
            TransitionsManager.Instance.DoTransition(
                new TransitionsManager.Transition(
                    TransitionsManager.TransitioType.SideBy,
                    actionInMiddle
                )
            );
        }
        WalkAroundScenario(scenario);



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

    public void HighlightScenarioButton(Image button)
    {
        foreach (Image image in scnearioButtons)
        {
            image.color = new Color(88f / 255f, 88f / 255f, 88f / 255f);
        }
        button.color = new Color(208f / 255f, 136f / 255f, 64f / 255f);
    }
    public void Talk(
        bool isTalking,
        string message = "",
        bool isQuestion = false,
        Action questionEvent = null
    )
    {
        speechBubble.SetActive(isTalking);
        StartCoroutine(TypeText(message));

        if (isQuestion)
        {
            speechBubble.transform.GetChild(0).gameObject.SetActive(true);
            this.questionEvent = questionEvent;
        }
    }

    public IEnumerator TypeText(string message, float delay = 0.02f)
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


    public async void WalkAroundScenario(GameObject scenario)
    {

        await Task.Delay(TimeSpan.FromSeconds(4));

        if (scenario == parkScenario)
        {
            WalkAround(parkPositions, scenario);
        }
        else if (scenario == playroomScenario)
        {
            WalkAround(playRoomPositions, scenario);
        }
        else if (scenario == bedroomScenario)
        {
            WalkAround(bedroomPositions, scenario);

        }


    }

    private async void WalkAround(Vector3[] positions, GameObject scenario)
    {

        foreach (Vector3 position in positions)
        {
            if (!scenario.activeSelf)
            {
                return;
            }
            await elmo.MoveTo(position);
            await Task.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(-1, 1) + delayBetWalk));
        }

        WalkAround(positions, scenario);



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (playroomScenario.activeSelf)
        {
            for (int i = 0; i < playRoomPositions.Length; i++)
            {
                Gizmos.DrawWireSphere(playRoomPositions[i], 0.5f);
                if (i < playRoomPositions.Length - 1)
                {
                    Gizmos.DrawLine(playRoomPositions[i], playRoomPositions[i + 1]);
                }
            }
        }
        else if (parkScenario.activeSelf)
        {
            for (int i = 0; i < parkPositions.Length; i++)
            {
                Gizmos.DrawWireSphere(parkPositions[i], 0.5f);
                if (i < parkPositions.Length - 1)
                {
                    Gizmos.DrawLine(parkPositions[i], parkPositions[i + 1]);
                }
            }
        }
        else if (bedroomScenario.activeSelf)
        {
            for (int i = 0; i < bedroomPositions.Length; i++)
            {
                Gizmos.DrawWireSphere(bedroomPositions[i], 0.5f);
                if (i < bedroomPositions.Length - 1)
                {
                    Gizmos.DrawLine(bedroomPositions[i], bedroomPositions[i + 1]);
                }
            }
        }
    }
}
