using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class Playroom : MonoBehaviour
{

    [Header("Global")]
    [SerializeField] private GlobalUi globalUI;

    [Header("Games")]

    [SerializeField] private GameObject gamesScreen;



    private async void OnEnable()
    {
        Action speechEvent = EnterGameSelectionScreen;

        await Task.Delay(10);
        await Speech.instance.SpeakAsync("Do you want to play a game with me?", true, speechEvent);
    }

    public void EnterGameSelectionScreen()
    {

        Debug.Log("EnterGameSelectionScreen");
        gamesScreen.SetActive(true);


    }
    public void ExitGameSelectionScreen()
    {
        gamesScreen.SetActive(false);
    }



    private void OnDisable()
    {
        GameUI.instance.Talk(false);
    }


}
