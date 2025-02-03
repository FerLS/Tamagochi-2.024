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



    public void SetEyesColor()
    {

        foreach (var pupil in Pupils)
        {
            pupil.color = eyesColor;
        }
    }

    public Color GetEyesColor()
    {
        return eyesColor; 
    }

    public void SetBodyColor()
    {

        foreach (var bodyPart in Body)
        {
            bodyPart.color = bodyColor;
        }

    }

    public Color GetBodyColor()
    {
        return bodyColor;
    }

    public void ChangeBodyColor(Color newColor)
    {
        bodyColor = newColor;
        foreach (var bodyPart in Body)
        {
            bodyPart.color = bodyColor;
        }
    }

    public void ChangeEyesColor(Color newColor)
    {
        eyesColor = newColor;
        foreach (var pupil in Pupils)
        {
            pupil.color = eyesColor;
        }
    }

    private void OnValidate()
    {

        SetEyesColor();
        SetBodyColor();

    }
}
