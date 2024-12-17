using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TicTacToeLogic : MonoBehaviour
{
    int spriteIndex = -1;

    private int[,] board = new int[3, 3];
    private bool isGameOver = false;

    [Header("Tamagotchi")]
    public GameObject tamagotchi;

    [Header("TicTacToe")]
    public GameObject boardObject;
    public GameObject backButton;
    public Sprite[] images; 
    public GameObject[] tokens;
    public GameObject FinishSign;
    public GameObject FinishText;


    private void Start()
    {
        FinishSign.SetActive(false);
        FinishText.SetActive(false);
        ResetBoard();
        boardObject.SetActive(true);
        backButton.SetActive(true);
        foreach (GameObject token in tokens)
        {
            token.SetActive(true);
        }
    }


    public void PlayerMove(int tokenIndex)
    {
        if (isGameOver) return;

        int row = tokenIndex / 3;
        int col = tokenIndex % 3;

        if (board[row, col] == 0) 
        {
            board[row, col] = 1; 
            UpdateTokenSprite(tokenIndex, 0);
            if (CheckWinCondition(1))
            {
                EndGame("You won!");
            }

            Invoke(nameof(MachineMove), 0.5f); 
        }
    }

    private void MachineMove()
    {
        if (isGameOver) return;

        bool played = false;
        while (!played)
        {
            int randomIndex = Random.Range(0, 9);
            int row = randomIndex / 3;
            int col = randomIndex % 3;

            if (board[row, col] == 0)
            {
                board[row, col] = 2;
                UpdateTokenSprite(randomIndex, 1);
                if (CheckWinCondition(2))
                {
                    EndGame("You lose :(");
                }
                played = true;
            }
        }

    }

    private void UpdateTokenSprite(int tokenIndex, int spriteIndex)
    {
        SpriteRenderer spriteRenderer = tokens[tokenIndex].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = images[spriteIndex];
    }

    private bool CheckWinCondition(int player)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player ||
                board[0, i] == player && board[1, i] == player && board[2, i] == player)
            {
                return true;
            }
        }

        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player ||
            board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
        {
            return true;
        }

        bool isDraw = true;
        foreach (int cell in board)
        {
            if (cell == 0) isDraw = false;
        }

        if (isDraw)
        {
            EndGame("It's a draw!");
            return false;
        }

        return false;
    }

    private void EndGame(string message)
    {
        isGameOver = true;
        FinishSign.SetActive(true);
        foreach (GameObject token in tokens)
        {
            token.SetActive(false);
        }
        TextMeshProUGUI tmpComponent = FinishText.GetComponent<TextMeshProUGUI>();
        if (tmpComponent != null)
        {
            tmpComponent.text = message;
        }
        FinishText.SetActive(true);
    }

    public void GoBack()
    {
        FinishSign.SetActive(false);
        FinishText.SetActive(false);
    }

    private void ResetBoard()
    {
        board = new int[3, 3];
        foreach (GameObject token in tokens)
        {
            token.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

}
