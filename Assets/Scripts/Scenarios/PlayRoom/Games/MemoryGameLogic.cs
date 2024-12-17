using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MemoryGameLogic : MonoBehaviour
{
    public Sprite[] animals;
    [SerializeField] private Sprite backImage;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessAnimal, secondGuessAnimal;

    private int countCorrectGuesses = 0;
    private int gameGuesses ;


    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        gameGuesses = gamePuzzles.Count/2;
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("puzzle");

        for (int i = 0; i < objects.Length; i++)
        {
            Button buttonComponent = objects[i].GetComponent<Button>();
            if (buttonComponent != null)
            {
                btns.Add(buttonComponent);
                btns[i].image.sprite = backImage;
            }
            else
            {
                Debug.LogWarning($"El objeto {objects[i].name} no tiene un componente Button.");
            }
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

    void AddListeners()
    {
        foreach(Button btn in btns)
        {
            btn.onClick.AddListener( () =>  PickPuzzle() );
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

            if (firstGuessAnimal == secondGuessAnimal)
            {
                Debug.Log("MATCH");
            }
            else
            {

                Debug.Log("Boo!");
            }

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

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

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
            Debug.Log("Finished!!");
        }
    }

    
}
