using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUi : MonoBehaviour
{
    [Header("Screens")]
    private GameObject previousScreen;

    [Header("Panels")]
    public GameObject EmotionsPanel;
    private Image[] emotionFaces;

    private void Start()
    {
        emotionFaces = EmotionsPanel.GetComponentsInChildren<Image>(true);
    }

    public void EnterScreen(RectTransform screen)
    {
        Action actionInTheEnd = () =>
        {
            previousScreen?.SetActive(false);
            previousScreen = screen.gameObject;
        };

        TransitionsManager.Instance.DoTransition(
            new TransitionsManager.Transition(
                TransitionsManager.TransitioType.SlideUpDown,
                actionInTheEnd,
                screen: screen.GetComponent<RectTransform>(),
                ease: Ease.OutBounce,
                duration: 1f
            )
        );
    }

    public void HideScreen(GameObject screen)
    {
        Debug.Log(screen);
        screen.SetActive(false);
    }

    public void SetEmotionFace(Image emotionFace)
    {
        foreach (var face in emotionFaces)
        {
            face.color = new Color(255, 255, 255, 0.5f);
        }
        emotionFace.color = new Color(255, 255, 255, 1);
    }
}
