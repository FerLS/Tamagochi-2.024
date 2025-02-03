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
    private Color selectedEyesColor = Color.black;

    void Start()
    {
        selectedBodyColor = Tamagotchi.GetBodyColor();
        UpdateColors();

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

    private void UpdateColors()
    {
        Tamagotchi.ChangeBodyColor(selectedBodyColor);
        Tamagotchi.ChangeEyesColor(selectedEyesColor);

        TamagotchiSettings.ChangeBodyColor(selectedBodyColor);
        TamagotchiSettings.ChangeEyesColor(selectedEyesColor);

        bodyColorBox.color = selectedBodyColor;
        eyesColorBox.color = selectedEyesColor;
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
    }

    public void SaveChanges()
    {
        gameObject.SetActive(false);
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
