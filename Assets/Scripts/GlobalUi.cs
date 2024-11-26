using UnityEngine;

public class GlobalUi : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialScreen;
    public GameObject GameScreen;
    public GameObject SettingsScreen;

    public GameObject ReportsScreen;
    public void EnterGameScreen()
    {

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


}
