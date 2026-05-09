using UnityEngine;
using System.Collections;

public class BossRock : MonoBehaviour
{
    enum State
    {
        Underground,
        Rising,
        Shooting,
        Vulnerable,
        Diving
    }

    private State currentState;

    [Header("References")]
    public Enemy enemy;
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Player")]
    private Transform player;

    [Header("Movement")]
    public Transform[] emergePoints;
    public float undergroundSpeed = 6f;

    [Header("Armor")]
    public int armorHitsRequired = 5;

    private int currentArmorHits;
    private bool armorBroken;

    [Header("Shoot Attack")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    public float shootInterval = 1f;
    public float vulnerableDuration = 5f;

    [Header("Counter Projectile")]
    public GameObject counterProjectilePrefab;
    public float counterProjectileSpeed = 8f;

    [Header("Timers")]
    public float riseDuration = 1f;
    public float diveDuration = 1f;

    [Header("Colliders")]
    public Collider2D damageCollider;

    private bool isDead = false;

    void Start()
    {
        if (enemy == null)
            enemy = GetComponent<Enemy>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        FindPlayer();

        StartCoroutine(BossLoop());
    }

    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");

        if (obj != null)
        {
            player = obj.transform;
        }
    }

    IEnumerator BossLoop()
    {
        while (!isDead)
        {
            yield return UndergroundMove();

            yield return Rise();

            yield return ShootPhase();

            yield return VulnerablePhase();

            yield return Dive();
        }
    }

    IEnumerator UndergroundMove()
    {
        currentState = State.Underground;

        RegenerateArmor();

        if (damageCollider != null)
            damageCollider.enabled = false;

        Transform target =
            emergePoints[Random.Range(0, emergePoints.Length)];

        while (Vector2.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                undergroundSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    IEnumerator Rise()
    {
        currentState = State.Rising;

        animator.SetTrigger("Rise");

        yield return new WaitForSeconds(riseDuration);

        if (damageCollider != null)
            damageCollider.enabled = true;
    }

    IEnumerator ShootPhase()
    {
        currentState = State.Shooting;

        float timer = 0f;

        while (timer < vulnerableDuration && !armorBroken)
        {
            timer += shootInterval;

            FireProjectileAtPlayer();

            yield return new WaitForSeconds(shootInterval);
        }
    }

    IEnumerator VulnerablePhase()
    {
        currentState = State.Vulnerable;

        float timer = 0f;

        while (timer < vulnerableDuration)
        {
            timer += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator Dive()
    {
        currentState = State.Diving;

        animator.SetTrigger("Dive");

        if (damageCollider != null)
            damageCollider.enabled = false;

        yield return new WaitForSeconds(diveDuration);
    }

    // =========================
    // DAMAGE
    // =========================

    public void TakeBossDamage(int damage)
    {
        if (isDead) return;

        // ARMOR ACTIVE
        if (!armorBroken)
        {
            currentArmorHits++;

            animator.SetTrigger("ArmorHit");

            FireCounterProjectile();

            Debug.Log("Armor hits: " + currentArmorHits);

            if (currentArmorHits >= armorHitsRequired)
            {
                BreakArmor();
            }

            return;
        }

        // REAL DAMAGE
        enemy.TakeDamage(damage);

        if (enemy.currentHealth <= 0)
        {
            isDead = true;
        }
    }

    void BreakArmor()
    {
        armorBroken = true;

        animator.SetBool("ArmorBroken", true);

        Debug.Log("Armor Broken");
    }

    void RegenerateArmor()
    {
        armorBroken = false;

        currentArmorHits = 0;

        animator.SetBool("ArmorBroken", false);

        Debug.Log("Armor Regenerated");
    }

    // =========================
    // PROJECTILES
    // =========================

    void FireProjectileAtPlayer()
    {
        if (player == null) return;

        Vector2 dir =
            (player.position - projectileSpawnPoint.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.identity
        );

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();

        if (projRb != null)
        {
            projRb.linearVelocity = dir * projectileSpeed;
        }
    }

    void FireCounterProjectile()
    {
        if (player == null) return;

        Vector2 dir =
            (player.position - projectileSpawnPoint.position).normalized;

        GameObject proj = Instantiate(
            counterProjectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.identity
        );

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();

        if (projRb != null)
        {
            projRb.linearVelocity = dir * counterProjectileSpeed;
        }
    }

    // =========================
    // DEBUG
    // =========================

    void OnDrawGizmosSelected()
    {
        if (projectileSpawnPoint != null && player != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(
                projectileSpawnPoint.position,
                player.position
            );
        }
    }
}