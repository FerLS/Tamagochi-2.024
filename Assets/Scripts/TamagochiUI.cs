using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TamagochiUI : MonoBehaviour
{




    [Header("Components")]

    [SerializeField] private Animator anim;
    [Header("Body Parts")]
    [SerializeField]
    private Image[] Pupils;

    [SerializeField]
    private Image[] Body;

    [Header("Customize")]
    [SerializeField]
    private Color bodyColor;

    [SerializeField]
    private Color eyesColor;

    public void SetEyesColor()
    {
        LoadEyesColors();
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
        LoadBodyColors();
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

    private void LoadBodyColors()
    {
        if (PlayerPrefs.HasKey("BodyColor_R"))
        {
            float r = PlayerPrefs.GetFloat("BodyColor_R");
            float g = PlayerPrefs.GetFloat("BodyColor_G");
            float b = PlayerPrefs.GetFloat("BodyColor_B");
            bodyColor = new Color(r, g, b);
        }
    }

    private void LoadEyesColors()
    {
        if (PlayerPrefs.HasKey("EyesColor_R"))
        {
            float r = PlayerPrefs.GetFloat("EyesColor_R");
            float g = PlayerPrefs.GetFloat("EyesColor_G");
            float b = PlayerPrefs.GetFloat("EyesColor_B");
            eyesColor = new Color(r, g, b);
        }
    }

    public async Task MoveTo(Vector3 position)
    {
        anim.CrossFade("Walk", 0.3f);
        if (position.x < transform.position.x)
        {
            transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        }

        float speed = Vector2.Distance(transform.position, position) / 2;
        await transform.DOMove(position, speed).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        anim.CrossFade("Idle", 0.3f);

    }
}
