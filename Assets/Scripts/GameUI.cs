using UnityEngine;

public class GameUI : MonoBehaviour
{
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


    public void ChangeScenary(Transform scenary)
    {

        parkScenary.SetActive(false);
        playroomScenary.SetActive(false);
        bedroomScenary.SetActive(false);
        bathroomScenary.SetActive(false);
        kitchenScenary.SetActive(false);

        scenary.gameObject.SetActive(true);
    }
}
