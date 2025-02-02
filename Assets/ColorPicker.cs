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


    [Header("UI Elements")]
    public Image bodyColorBox;
    public Image eyesColorBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateColorBoxes();
    }

    private void UpdateColorBoxes()
    {
        Color bodyColor = Tamagotchi.GetBodyColor();
        Color eyesColor = Tamagotchi.GetEyesColor();

        bodyColorBox.color = bodyColor;
        eyesColorBox.color = eyesColor;
    }
}
