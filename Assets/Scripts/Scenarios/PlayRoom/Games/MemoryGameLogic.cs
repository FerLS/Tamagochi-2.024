using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MemoryGameLogic : MonoBehaviour
{
    [Header("Game Board")]
    [SerializeField] private Transform puzzleField;
    [SerializeField] private GameObject puzzleBtnPrefab;

    [Header("Signs")]
    public GameObject FinishSign;

    [Header("Items")]
    [SerializeField] private List<Sprite> gamePuzzles = new List<Sprite>();
    [SerializeField] private Sprite[] animals;
    [SerializeField] private Sprite backImage;

    [Header("Save System")]
    public SaveSystem saveSystem;


    private List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessAnimal, secondGuessAnimal;

    private int countCorrectGuesses = 0;
    private int gameGuesses ;


    void Awake()
    {
        InitializeButtons();
    }

    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        ShuffleCards(gamePuzzles);
        gameGuesses = gamePuzzles.Count/2;
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject _button = Instantiate(puzzleBtnPrefab, puzzleField, false);
            _button.name = "" + i;
        }
    }

    void GetButtons()
    {
        btns.Clear();
        foreach(Transform child in puzzleField)
        {
            Button btn = child.GetComponent<Button> (); 
            if (btn)
            {
                btns.Add(btn);
                btn.image.sprite = backImage;
            }
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }
            gamePuzzles.Add(animals[index]);
            index++;
        }

    }

    public void PickPuzzle()
    {
        string choice = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(choice);
            firstGuessAnimal = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(choice);
            secondGuessAnimal = gamePuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            StartCoroutine(CheckPuzzleMatch());
        }
    }

    IEnumerator CheckPuzzleMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstGuessAnimal == secondGuessAnimal)
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            CheckIfGameFinished();
        }
        else
        {
            btns[firstGuessIndex].image.sprite = backImage;
            btns[secondGuessIndex].image.sprite = backImage;
        }

        yield return new WaitForSeconds(0.2f);
        firstGuess = secondGuess = false;
    }

    void CheckIfGameFinished()
    {
        countCorrectGuesses++;
        if (countCorrectGuesses == gameGuesses)
        {
            FinishSign.SetActive(true);
            if (saveSystem != null)
            {
                saveSystem.SaveGameData("Memory", "");
            }
        }

       
    }

    void ShuffleCards(List<Sprite> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Sprite aux = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            
            cards[i] = cards[randomIndex];
            cards[randomIndex] = aux;
        }

    }

    public void RestartGame()
    {
        FinishSign.SetActive(false);

        countCorrectGuesses = 0;
        ShuffleCards(gamePuzzles);
        ShowCards();
        gameGuesses = gamePuzzles.Count / 2;
    }
    
    void ShowCards()
    {
        for (int i =0; i < btns.Count; i++)
        {
            btns[i].interactable = true;
            btns[i].image.sprite = backImage;
        }
    }
}
