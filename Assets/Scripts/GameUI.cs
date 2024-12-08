using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
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
        HighlightButton(playroomButton);
    }

    public void ChangeScenary(GameObject scenary)
    {
        parkScenary.SetActive(false);
        playroomScenary.SetActive(false);
        bedroomScenary.SetActive(false);
        bathroomScenary.SetActive(false);
        kitchenScenary.SetActive(false);

        scenary.SetActive(true);
    }

    public void HighlightButton(Button chosenScenario)
    {
        if (lastClickedButton != null)
        {
            lastClickedButton.image.color = new Color(89f / 255f, 89f / 255f, 89f / 255f);
        }

        chosenScenario.image.color = new Color(202f / 255f, 121f / 255f, 34f / 255f); ;

        lastClickedButton = chosenScenario;
    }
}
