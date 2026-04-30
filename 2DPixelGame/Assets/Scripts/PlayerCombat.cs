using System.Drawing;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public LayerMask enemyLayers;
    public int attackDamage; 
    private PlayerMana mana;

    [Header("Attack Timing")]
    public float attackCooldown = 0.3f;
    private float lastAttackTime;

    [Header("Heavy Attack")]
    public GameObject heavyAttackPrefab;
    public Transform[] heavyAttackPoints;
    public float heavyAttackRange = 1f;

    [Header("Air Attack")]
    public Transform airAttackPoint;
    public float airAttackWidth = 1f;
    public float airAttackHeight = 1.5f;
    public Transform airAttackInstantiatePoint;
    public GameObject airAttackPrefab;
    public float bounceForce = 12f;

    private PlayerMovement movement;

    void Start()
    {
        attackDamage = GameManager.instance.attackPower;

        mana = GetComponent<PlayerMana>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenu pauseMenu = FindAnyObjectByType<PauseMenu>();

        if (pauseMenu != null && pauseMenu.GameIsPaused) 
            return;

        if (GameManager.instance != null && GameManager.instance.inputLocked)
            return;

        if (movement.controller.IsGrounded && Input.GetButtonDown("Attack") && CanAttack())
        {
            Attack();
            lastAttackTime = Time.time;
        }

        if (Input.GetButtonDown("HeavyAttack") && CanAttack())
        {
            HeavyAttack();
            lastAttackTime = Time.time;
        }

        if (!movement.controller.IsGrounded && Input.GetButtonDown("Attack") && CanAttack())
        {
            AirAttack();
            lastAttackTime = Time.time;
        }
    }
    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    void Attack()
    {
        //Näytetään hyökkäysanimaatio
        animator.SetTrigger("Attack");

        //Osutaanko viholliseen?
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        //Vahingoita vihollista
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(GameManager.instance.attackPower);
        }
    }
    void HeavyAttack()
    {
        int manaCost = 30;

        if (mana == null || !mana.UseMana(manaCost))
        {
            Debug.Log("Not enough mana!");
            return;
        }

        animator.SetTrigger("HeavyAttack");
    }

    void AirAttack()
    {
        animator.SetTrigger("AirAttack");

        if (airAttackPrefab != null)
        {
            Instantiate(airAttackPrefab, airAttackInstantiatePoint.position, Quaternion.identity);
        }

        Vector2 boxSize = new Vector2(airAttackWidth, airAttackHeight); // leveys, korkeus
        Vector2 boxCenter = (Vector2)airAttackPoint.position + Vector2.down * 0.75f;

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            boxCenter,
            boxSize,
            0f,
            enemyLayers
        );

        bool hitEnemy = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(GameManager.instance.attackPower);
            hitEnemy = true;
        }

        // bounce jos osuttiin
        if (hitEnemy)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        }
    }

    public void HeavyHit1() => DoHeavyHit(0);
    public void HeavyHit2() => DoHeavyHit(1);
    public void HeavyHit3() => DoHeavyHit(2);
    public void HeavyHit4() => DoHeavyHit(3);

    void DoHeavyHit(int index)
    {
        if (index >= heavyAttackPoints.Length) return;

        Transform point = heavyAttackPoints[index];

        if (heavyAttackPrefab != null)
        {
            Instantiate(heavyAttackPrefab, point.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Vector2 boxSize = new Vector2(airAttackWidth, airAttackHeight);
        Vector2 boxCenter = (Vector2)airAttackPoint.position + Vector2.down * 0.75f;

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
