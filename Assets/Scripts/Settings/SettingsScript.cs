using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialSettingsScreen;
    public GameObject AppareanceScreen;

    void Start()
    {
        InitialSettingsScreen.SetActive(true);
        AppareanceScreen.SetActive(false);
    }


    public void ShowAppearanceScreen()
    {
        InitialSettingsScreen.SetActive(false);
        AppareanceScreen.SetActive(true);
    }

    public void ShowInitialSettingsScreen()
    {
        InitialSettingsScreen.SetActive(true);
        AppareanceScreen.SetActive(false);
    }
}
