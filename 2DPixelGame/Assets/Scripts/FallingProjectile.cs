using UnityEngine;

public class FallingProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float fallSpeed = 8f;

    [Header("Damage")]
    public int damage = 20;

    [Header("Lifetime")]
    public float lifetime = 3f;

    private Vector2 direction = Vector2.down;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position +=
            (Vector3)(direction * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerHealth player =
            collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage, transform.position);
        }

        Destroy(gameObject);
    }
}