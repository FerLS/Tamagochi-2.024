using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class TicTacToeLogic : MonoBehaviour
{
    private int[,] board = new int[3, 3];
    private bool isGameOver = false;
    [Header("TicTacToe")]
    public Sprite[] images;
    public Image[] tokens;
    public TextMeshProUGUI FinishSign;

    [Header("Save System")]
    public SaveSystem saveSystem;

    private void OnEnable()
    {
        FinishSign.transform.parent.gameObject.SetActive(false);
        ResetBoard();

        isGameOver = false;
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
                    EndGame("You lost :(");
                }
                played = true;
            }
        }

    }

    private void UpdateTokenSprite(int tokenIndex, int spriteIndex)
    {
        tokens[tokenIndex].DOFade(1, 0.2f);
        tokens[tokenIndex].transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
        tokens[tokenIndex].sprite = images[spriteIndex];
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
        FinishSign.transform.parent.gameObject.SetActive(true);
        FinishSign.text = message;

        if (saveSystem != null)
        {
            saveSystem.SaveGameData("Tic Tac Toe", message);
        }
    }

    private void ResetBoard()
    {
        board = new int[3, 3];
        foreach (Image token in tokens)
        {
            token.sprite = null;
            token.DOFade(0, 0);

        }

    }


    public void RestartGame()
    {
        FinishSign.transform.parent.gameObject.SetActive(false);
        ResetBoard();

        foreach (Image token in tokens)
        {
            token.DOFade(0, 0);
        }

        isGameOver = false;
    }
}
