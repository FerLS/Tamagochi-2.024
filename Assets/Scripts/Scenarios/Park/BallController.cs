using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D ball;

    [SerializeField] private GameObject wallPrefab;
    public float wallThickness = 1f;

    [Header("Walls")]
    [SerializeField] private BoxCollider2D Top;
    [SerializeField] private BoxCollider2D Bottom;
    [SerializeField] private BoxCollider2D Left;
    [SerializeField] private BoxCollider2D Right;

    private BoxCollider2D[] wallPositions;

    void Start()
    {

        ball = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        ball.linearVelocity = randomDirection * speed;

        CreateWalls();
    }

    private void CreateWalls()
    {
        wallPositions = new BoxCollider2D[]
        {
            Top, Bottom, Left, Right
        };

        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Entro el click");
            BounceOnClick();
        }
    }

    private void BounceOnClick()
    {
        Vector2 newDirection = -ball.linearVelocity.normalized; // Invierte la direcci�n
        newDirection += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)); // Peque�a variaci�n
        newDirection.Normalize(); // Normalizar para que la velocidad se mantenga constante

        ball.linearVelocity = newDirection * speed;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            ball.linearVelocity = randomDirection * speed;
        }
    }

}
