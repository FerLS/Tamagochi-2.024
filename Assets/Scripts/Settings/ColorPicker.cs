using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ColorPicker : MonoBehaviour
{
    [Header("Original Tamgotchi")]
    public TamagochiUI Tamagotchi;

    [Header("Settings Tamgotchi")]
    public TamagochiUI TamagotchiSettings;

    [Header("Color Options")]
    public Button[] bodyColorButtons;
    public Button[] eyesColorButtons;

    [Header("UI Elements")]
    public Image bodyColorBox;
    public Image eyesColorBox;

    private Dictionary<Button, Color> originalBodyColors = new Dictionary<Button, Color>();
    private Dictionary<Button, Color> originalEyesColors = new Dictionary<Button, Color>();

    private Color selectedBodyColor;
    private Color selectedEyesColor;

    void Start()
    {
        selectedBodyColor = Tamagotchi.GetBodyColor();
        selectedEyesColor = Tamagotchi.GetEyesColor();

        LoadBoxColors();

        foreach (var bcolor in bodyColorButtons)
        {
            Image img = bcolor.GetComponent<Image>();
            originalBodyColors[bcolor] = img.color;
        }
        foreach (var ecolor in eyesColorButtons)
        {
            Image img = ecolor.GetComponent<Image>();
            originalEyesColors[ecolor] = img.color;
        }
    }

    private void LoadBoxColors()
    {
        bodyColorBox.color = selectedBodyColor;
        eyesColorBox.color = selectedEyesColor;
    }

    private void UpdateColors()
    {
       
        Tamagotchi.ChangeBodyColor(selectedBodyColor);
        Tamagotchi.ChangeEyesColor(selectedEyesColor);

        TamagotchiSettings.ChangeBodyColor(selectedBodyColor);
        TamagotchiSettings.ChangeEyesColor(selectedEyesColor);

    }

    public void SelectBodyColor(Button newBodyColor)
    {
        foreach (var bcolor in bodyColorButtons)
        {
            Image img = bcolor.GetComponent<Image>();

            if (bcolor != newBodyColor)
            {
                img.color = Color.Lerp(originalBodyColors[bcolor], Color.black, 0.5f);
            }
            else
            {
                img.color = originalBodyColors[bcolor];
                selectedBodyColor = originalBodyColors[bcolor];
            }
        }
        UpdateColors();
        LoadBoxColors();
    }

    public void SelectEyeColor(Button newEyeColor)
    {
        foreach (var ecolor in eyesColorButtons)
        {
            Image img = ecolor.GetComponent<Image>();

            if (ecolor != newEyeColor)
            {
                img.color = Color.Lerp(originalEyesColors[ecolor], Color.black, 0.5f);
            }
            else
            {
                img.color = originalEyesColors[ecolor];
                selectedEyesColor = originalEyesColors[ecolor];
            }
        }
        UpdateColors();
        LoadBoxColors();
    }

    public void SaveChanges()
    {
        gameObject.SetActive(false);

        PlayerPrefs.SetFloat("BodyColor_R", selectedBodyColor.r);
        PlayerPrefs.SetFloat("BodyColor_G", selectedBodyColor.g);
        PlayerPrefs.SetFloat("BodyColor_B", selectedBodyColor.b);

        PlayerPrefs.SetFloat("EyesColor_R", selectedEyesColor.r);
        PlayerPrefs.SetFloat("EyesColor_G", selectedEyesColor.g);
        PlayerPrefs.SetFloat("EyesColor_B", selectedEyesColor.b);

        PlayerPrefs.Save();
    }

    public void ShowBodyColorOptions()
    {
        foreach (var bcolor in bodyColorButtons)
        {
            bcolor.gameObject.SetActive(true);
        }
        foreach (var ecolor in eyesColorButtons)
        {
            ecolor.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public void ShowEyesColorOptions()
    {
        foreach (var bcolor in bodyColorButtons)
        {
            bcolor.gameObject.SetActive(false);
        }
        foreach (var ecolor in eyesColorButtons)
        {
            ecolor.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
    }
}
