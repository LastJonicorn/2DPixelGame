using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyPatrol patrol;

    [Header("Enemy's Life")]
    public Animator animator;
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Enemy Attack")]
    public Transform attackPoint;
    public float attackRange = 0.1f;
    public LayerMask playerLayer;
    public int attackDamage = 20;
    public float attackCooldown = 2f;
    private float lastAttackTime = -Mathf.Infinity;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;


    void Start()
    {
        currentHealth = maxHealth;
        patrol = GetComponent<EnemyPatrol>();
    }

    private void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (hitPlayer != null)
        {
            Attack(hitPlayer);
        }
    }

    void Attack(Collider2D player)
    {
        lastAttackTime = Time.time;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        audioSource.PlayOneShot(hurtSound);
        //Näytä vahingoittumisanimaatio
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        audioSource.PlayOneShot(deathSound);

        // Pysäytä liike
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // Poista patrol
        EnemyPatrol patrol = GetComponent<EnemyPatrol>();
        if (patrol != null)
        {
            patrol.enabled = false;
        }

        GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
        this.enabled = false;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
