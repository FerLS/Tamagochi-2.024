using UnityEngine;

public class TicTacToeLogic : MonoBehaviour
{
    int spriteIndex = -1;

    public int PlayerTurn()
    {
        spriteIndex++;
        return spriteIndex % 2;
    }
}
