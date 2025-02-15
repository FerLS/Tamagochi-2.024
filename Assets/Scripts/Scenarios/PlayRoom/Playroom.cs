using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Playroom : MonoBehaviour
{
    [SerializeField] private RectTransform minigamesScreen;

    private async void OnEnable()
    {

        Action speechEvent = () =>
        {
            Debug.Log("SpeechEvent");
            EnterScreen(minigamesScreen);
        };

        await Task.Delay(10);
        await Speech.instance.SpeakAsync("Do you want to play a game with me?", true, speechEvent);
    }

    public void EnterScreen(RectTransform screen)
    {
        Debug.Log("EnterGameSelectionScreen");
        TransitionsManager.Transition transition = new TransitionsManager.Transition(
            TransitionsManager.TransitioType.SlideUpDown,
            null,
            screen: screen);


        TransitionsManager.Instance.DoTransition(transition);

    }

    public void ExitScreen(RectTransform screen)
    {
        Debug.Log("EnterGameSelectionScreen");
        TransitionsManager.Transition transition = new TransitionsManager.Transition(
            TransitionsManager.TransitioType.SlideUpDown,
            null,
            inverse: true,
            screen: screen);


        TransitionsManager.Instance.DoTransition(transition);


    }


    private void OnDisable()
    {
        GameUI.instance.Talk(false);
    }
}
