using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D ball;

    [SerializeField] private GameObject wallPrefab;
    public float wallThickness = 1f;

    void Start()
    {

        ball = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        ball.linearVelocity = randomDirection * speed;

        CreateWalls();
    }

    private void CreateWalls()
    {
        float width = 4.8f;
        float height = 9.6f;
                
        Instantiate(wallPrefab, new Vector3(0, height + wallThickness / 2, 0), Quaternion.identity);  
        Instantiate(wallPrefab, new Vector3(0, -height - wallThickness / 2, 0), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(-width - wallThickness / 2, 0, 0), Quaternion.identity);    
        Instantiate(wallPrefab, new Vector3(width + wallThickness / 2, 0, 0), Quaternion.identity);     
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = GetComponent<Collider2D>();

            if (collider.OverlapPoint(mousePosition))
            {
                BounceOnClick();
            }
        }
    }

    private void BounceOnClick()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        ball.linearVelocity = randomDirection * speed;
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
