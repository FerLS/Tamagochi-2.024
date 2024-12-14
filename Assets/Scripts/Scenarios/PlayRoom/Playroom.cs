using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks; 

public class Playroom : MonoBehaviour
{

    [Header("Global")]
    [SerializeField] private GlobalUi globalUI;

    [Header("Tamagotchi")]
    [SerializeField] private Speech tamagotchiSpeech;

    [Header("Screens")]
    [SerializeField] private GameObject Background;
    [SerializeField] private GameObject SelectGame;
    [SerializeField] private GameObject Games;
    [SerializeField] private GameObject TicTacToe;
    [SerializeField] private GameObject Memory;
    [SerializeField] private GameObject Cups;

    [Header("Buttons")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private async void OnEnable() 
    {
        Background.SetActive(true);
        SelectGame.SetActive(false);
        Games.SetActive(false);
        TicTacToe.SetActive(false);
        Memory.SetActive(false);

        if (tamagotchiSpeech != null)
        {
            await tamagotchiSpeech.SpeakAsync("Do you want to play a game with me?", true);
            SetButtons(true);
        }
    }

    public void EnterGameSelectionScreen()
    {
        Background.SetActive(false);
        SelectGame.SetActive(true);
        SetButtons(false);
    }

    public void SetButtons(bool isActive)
    {
        yesButton.gameObject.SetActive(isActive);
        noButton.gameObject.SetActive(isActive);
    }

    public void SetSelectedGame(GameObject selectedGame)
    {
        Debug.Log(selectedGame);
        Background.SetActive(false);
        SelectGame.SetActive(false);
        TicTacToe.SetActive(false);
        Memory.SetActive(false);
        Cups.SetActive(false);


        Games.SetActive(true);
        selectedGame.SetActive(true);
    }

    private void OnDisable()
    {
        tamagotchiSpeech.HideSpeechBubble();
    }

    private void Update()
    {

    }


}
