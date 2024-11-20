using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TamagochiUI : MonoBehaviour
{
    [Header("Body Parts")]

    [SerializeField] private Image[] Pupils;
    [SerializeField] private Image[] Body;

    [Header("Customize")]

    [SerializeField] private Color bodyColor;

    [SerializeField] private Color eyesColor;



    void SetEyesColor()
    {

        foreach (var pupil in Pupils)
        {
            pupil.color = eyesColor;
        }
    }

    void SetBodyColor()
    {

        foreach (var bodyPart in Body)
        {
            bodyPart.color = bodyColor;
        }

    }

    public void ChangeBodyColor(Color newColor)
    {
        bodyColor = newColor;
        foreach (var bodyPart in Body)
        {
            bodyPart.color = bodyColor;
        }
    }

    private void OnValidate()
    {

        SetEyesColor();
        SetBodyColor();

    }
}
