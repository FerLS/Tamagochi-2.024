using System.Linq;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Screens")]
    public GameObject InitialSettingsScreen;
    public GameObject AppareanceScreen;

    [Header("Settings")]

    public FontTypes[] fonts;

    public TMP_Dropdown fontDropdown;


    void Start()
    {
        InitialSettingsScreen.SetActive(true);
        AppareanceScreen.SetActive(false);
        fontDropdown.onValueChanged.AddListener(SetCustomFont);

    }


    public void SetCustomFont(int index)
    {

        Debug.Log(index);
        AdaptativeFont.SetFont(fonts[index]);
        FindObjectsByType<AdaptativeFont>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList().ForEach(font => font.UpdateFont());
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
