using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public int tokenIndex;
    private TicTacToeLogic logic;

    void Start()
    {
        logic = GetComponentInParent<TicTacToeLogic>();
    }

    void OnMouseDown()
    {
        logic.PlayerMove(tokenIndex);
    }
}