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

    [Header("EXP")]
    public int expValue = 20;

    BossEyeMovement bossMovement;

    void Start()
    {
        currentHealth = maxHealth;
        patrol = GetComponent<EnemyPatrol>();
        bossMovement = GetComponent<BossEyeMovement>();
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
            playerHealth.TakeDamage(attackDamage, transform.position);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        audioSource.PlayOneShot(hurtSound);
        //Näytä vahingoittumisanimaatio
        animator.SetTrigger("Hurt");

        if (bossMovement != null)
        {
            bossMovement.Stun();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        EnemyShooter shooter = GetComponent<EnemyShooter>();
        if (shooter != null)
        {
            shooter.Die();
        }
        BossEyeMovement bossMovement = GetComponent<BossEyeMovement>();
        if (bossMovement != null)
        {
            bossMovement.Die();
        }

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

        GameManager.instance.AddExp(expValue);

        GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
        this.enabled = false;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
