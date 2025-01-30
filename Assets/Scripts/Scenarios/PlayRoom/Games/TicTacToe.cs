using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public int tokenIndex;
    private TicTacToeLogic logic;

    private void Start()
    {
        logic = FindObjectOfType<TicTacToeLogic>();
    }

    private void OnMouseDown()
    {
        logic.PlayerMove(tokenIndex); 
    }
}