using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyPatrol patrol;
    BossEyeMovement bossMovement;
    EnemyDashAI dashAI;
    BossVolcano bossVolcano;
    DragonHead bossDragon;

    [Header("Enemy's Life")]
    public Animator animator;
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Enemy Attack")]
    public Transform[] attackPoints;
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


    void Start()
    {
        currentHealth = maxHealth;
        patrol = GetComponent<EnemyPatrol>();
        bossMovement = GetComponent<BossEyeMovement>();
        dashAI = GetComponent<EnemyDashAI>();
        bossVolcano = GetComponent<BossVolcano>();
        bossDragon = GetComponent<DragonHead>();
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
        foreach (Transform point in attackPoints)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(
                point.position,
                attackRange,
                playerLayer
            );

            if (hitPlayer != null)
            {
                Attack(hitPlayer);
                return; // est‰‰ multi-hitit samalla framella
            }
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
        //N‰yt‰ vahingoittumisanimaatio
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
        EnemyDashAI dashAI = GetComponent<EnemyDashAI>();
        if (dashAI != null)
        {
            dashAI.Die();
        }
        BossVolcano bossVolcano = GetComponent<BossVolcano>();
        if (bossVolcano != null)
        {
            bossVolcano.Die();
        }
        DragonHead bossDragon = GetComponent<DragonHead>();
        if (bossDragon != null)
        {
            bossDragon.Die();
        }

        animator.SetBool("IsDead", true);
        audioSource.PlayOneShot(deathSound);

        // Pys‰yt‰ liike
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
        if (attackPoints == null) return;

        foreach (Transform point in attackPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, attackRange);
            }
        }
    }
}
