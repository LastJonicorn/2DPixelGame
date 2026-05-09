using UnityEngine;

public class FireParticleDamage : MonoBehaviour
{
    public int damage = 10;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();

            if (hp != null)
            {
                hp.TakeDamage(damage, transform.position);
            }
        }
    }
}