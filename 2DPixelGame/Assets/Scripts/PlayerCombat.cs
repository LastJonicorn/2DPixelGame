using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public LayerMask enemyLayers;
    public int attackDamage; 
    private PlayerMana mana;

    [Header("Heavy Attack")]
    public GameObject heavyAttackPrefab;
    public Transform[] heavyAttackPoints;
    public float heavyAttackRange = 1f;
    public int heavyAttackDamage = 80;

    public Transform airAttackPoint;

    private PlayerMovement movement;

    void Start()
    {
        attackDamage = GameManager.instance.attackPower;
        mana = GetComponent<PlayerMana>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenu pauseMenu = FindAnyObjectByType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.GameIsPaused) return;

        if (movement.controller.IsGrounded && Input.GetButtonDown("Attack"))
        {
            Attack();
        }

        if (Input.GetButtonDown("HeavyAttack"))
        {
            HeavyAttack();
        }

        if (!movement.controller.IsGrounded && Input.GetButtonDown("Attack"))
        {
            //Debug.Log("Air Attack");
            AirAttack(); 
        }
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

        Vector2 boxSize = new Vector2(1.0f, 1.5f); // leveys, korkeus
        Vector2 boxCenter = (Vector2)airAttackPoint.position + Vector2.down * 0.75f;

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            boxCenter,
            boxSize,
            0f,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(GameManager.instance.attackPower);
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

        Vector2 boxSize = new Vector2(1.0f, 1.5f);
        Vector2 boxCenter = (Vector2)airAttackPoint.position + Vector2.down * 0.75f;

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
