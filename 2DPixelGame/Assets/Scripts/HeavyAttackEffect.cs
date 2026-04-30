using UnityEngine;

public class HeavyAttackEffect : MonoBehaviour
{
    public int heavyAttackDamage;
    public LayerMask enemyLayers;

    public float lifetime = 2f;

    private void Start()
    {
        heavyAttackDamage = GameManager.instance.heavyAttackPower;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((enemyLayers.value & (1 << collision.gameObject.layer)) == 0)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(heavyAttackDamage);
        }
    }
}