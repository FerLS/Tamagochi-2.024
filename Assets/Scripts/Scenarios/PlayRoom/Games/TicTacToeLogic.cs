using UnityEngine;

public class TicTacToeLogic : MonoBehaviour
{
    int spriteIndex = -1;

    // 3x3 board (0 = empty, 1 = player, 2 = Machine)
    private int[,] board = new int[3, 3];

    // 1 = player, 2 = AI
    private int currentPlayer = 1; 

    public Sprite[] images; 
    public GameObject[] tokens;

    private void Start()
    {
        ResetBoard();
    }

    public void PlayerMove(int tokenIndex)
    {
        int row = tokenIndex / 3;
        int col = tokenIndex % 3;

        if (board[row, col] == 0) // Only allow moves on empty spots
        {
            board[row, col] = 1; // Player move
            UpdateTokenSprite(tokenIndex, 0); // Set player sprite (images[0])
            CheckWinCondition(1);

            // Machine Turn
            Invoke(nameof(MachineMove), 0.5f); // Add delay for AI move
        }
    }

    private void MachineMove()
    {
        //Picks first empty spot
        for (int i = 0; i < tokens.Length; i++)
        {
            int row = i / 3;
            int col = i % 3;

            if (board[row, col] == 0)
            {
                board[row, col] = 2; 
                UpdateTokenSprite(i, 1); 
                CheckWinCondition(2);
                return;
            }
        }
    }

    private void UpdateTokenSprite(int tokenIndex, int spriteIndex)
    {
        SpriteRenderer spriteRenderer = tokens[tokenIndex].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = images[spriteIndex];
    }

    private void CheckWinCondition(int player)
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player ||
                board[0, i] == player && board[1, i] == player && board[2, i] == player)
            {
                Debug.Log((player == 1 ? "Player" : "AI") + " wins!");
                ResetBoard();
                return;
            }
        }

        // Check diagonals
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player ||
            board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
        {
            Debug.Log((player == 1 ? "Player" : "AI") + " wins!");
            ResetBoard();
            return;
        }

        // Check for draw
        bool isDraw = true;
        foreach (int cell in board)
        {
            if (cell == 0) isDraw = false;
        }

        if (isDraw)
        {
            Debug.Log("It's a draw!");
            ResetBoard();
        } 
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
