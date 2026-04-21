using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int healthAmount = 20;
    public Collider2D targetCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == targetCollider)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthAmount);
            }

            Destroy(gameObject);
        }
    }
}
