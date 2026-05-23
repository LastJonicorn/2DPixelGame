using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f;
    public int damage = 20;

    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage, transform.position);
        }

        Destroy(gameObject);
    }
}