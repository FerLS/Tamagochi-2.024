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

    private Vector2 targetPosition;
    private bool isMoving = false;

    [SerializeField] private TamagochiUI tama;

    void Start()
    {
        ball = GetComponent<Rigidbody2D>();
        targetPosition = ball.position;

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
        CheckForClick();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    private void CheckForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = mousePosition;
            isMoving = true;

            if (Random.value < 0.5f)
            {
                tama.SetDirtyLevel(false);
            }
        }
    }

    private void MoveToTarget()
    {
        Vector2 direction = (targetPosition - ball.position).normalized;
        float distance = Vector2.Distance(ball.position, targetPosition);

        if (distance > 0.1f)
        {
            ball.MovePosition(ball.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            ball.position = targetPosition;
            isMoving = false;
        }
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
