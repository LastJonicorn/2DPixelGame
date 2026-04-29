using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;
    public float lifetime = 3f;
    public float rotationSpeed = 720f;

    private Vector2 moveDir;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 🔥 VANHA: seuraa targettia
    public void Init(Transform target)
    {
        moveDir = (target.position - transform.position).normalized;
        Destroy(gameObject, lifetime);
    }

    // 🔥 UUSI: radial / suora suunta
    public void InitDirection(Vector2 dir)
    {
        moveDir = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // liike
        rb.linearVelocity = moveDir * speed;

        // pyörintä
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth hp = collision.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}