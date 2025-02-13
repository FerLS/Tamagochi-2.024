using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
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



    [Header("Dirties")]
    private int dirtyLevel = 0;
    [SerializeField] private Image[] dirties;

    public UnityEvent onCleaned;

    [Header("Customize")]
    [SerializeField]
    private Color bodyColor;

    [SerializeField]
    private Color eyesColor;




    [Header("Movement")]
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 startPos = new Vector3(-2.06f, -7.57f, 0);






    public void OnChangeScenario()
    {

        targetPosition = startPos;
    }

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
        if (position.x < targetPosition.x)
        {
            transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        }

        float speed = Vector2.Distance(targetPosition, position) / 2;

        await DOTween.To(() => targetPosition, x => targetPosition = x, position, speed).SetEase(Ease.InOutQuad).OnUpdate(() =>
        {
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
            LookTama.xTama = targetPosition.x;
        }).AsyncWaitForCompletion();
        anim.CrossFade("Idle", 0.3f);

    }




    public void SetDirtyLevel(bool clean)
    {
        dirtyLevel = Mathf.Clamp(dirtyLevel + (clean ? -1 : 1), 0, dirties.Length);

        for (int i = 0; i < dirties.Length; i++)
        {
            dirties[i].DOFade(i < dirtyLevel ? 1 : 0, 0.3f);

        }

        if (dirtyLevel == 0)
        {
            onCleaned?.Invoke();
        }

    }
}
