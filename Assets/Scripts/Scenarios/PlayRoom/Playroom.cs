using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Playroom : MonoBehaviour
{
    [SerializeField] private RectTransform minigamesScreen;

    private async void OnEnable()
    {
        Action speechEvent = EnterGameSelectionScreen;

        await Task.Delay(10);
        await Speech.instance.SpeakAsync("Do you want to play a game with me?", true, speechEvent);
    }

    public void EnterGameSelectionScreen()
    {
        Debug.Log("EnterGameSelectionScreen");
        TransitionsManager.Transition transition = new TransitionsManager.Transition(
            TransitionsManager.TransitioType.SlideUpDown,
            null,
            screen: minigamesScreen);


        TransitionsManager.Instance.DoTransition(transition);

    }

    public void ExitGameSelectionScreen()
    {
        Debug.Log("EnterGameSelectionScreen");
        TransitionsManager.Transition transition = new TransitionsManager.Transition(
            TransitionsManager.TransitioType.SlideUpDown,
            null,
            inverse: true,
            screen: minigamesScreen);


        TransitionsManager.Instance.DoTransition(transition);


    }

    private void OnDisable()
    {
        GameUI.instance.Talk(false);
    }
}
