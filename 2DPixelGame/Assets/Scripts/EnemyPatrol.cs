using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;

    [Header("Checks")]
    public Transform wallCheck;
    public Transform ledgeCheck;
    public float checkDistance = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        // Liike
        float direction = movingRight ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // Tarkista seinä edessä
        bool wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, checkDistance, groundLayer);

        // Tarkista onko maata edessä
        Vector2 ledgePos = ledgeCheck.position + Vector3.right * (movingRight ? 0.2f : -0.2f);
        bool groundDetected = Physics2D.Raycast(ledgePos, Vector2.down, checkDistance, groundLayer);

        // Jos seinä tai ei maata → käänny
        if (wallDetected || !groundDetected)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Debug visualisointi
    private void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, checkDistance);

        }

        if (ledgeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ledgeCheck.position, checkDistance);
        }
    }
}