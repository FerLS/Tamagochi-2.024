using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    GameObject board;
    SpriteRenderer spriteRenderer;
    public Sprite[] images;
    bool unplayed = true;

    private void Start()
    {
        spriteRenderer.sprite = null;
    }

    private void OnMouseDown()
    {
        if (unplayed)
        {
            int index = board.GetComponent<TicTacToeLogic>().PlayerTurn();
            spriteRenderer.sprite = images[index];
            unplayed = false;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        board = GameObject.Find("Board");
    }
}