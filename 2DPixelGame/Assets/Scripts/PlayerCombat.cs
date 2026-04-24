using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    private PlayerMana mana;
    void Start()
    {
        mana = GetComponent<PlayerMana>();
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenu pauseMenu = FindAnyObjectByType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.GameIsPaused) return;

        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }

        if (Input.GetButtonDown("HeavyAttack"))
        {
            HeavyAttack();
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
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    void HeavyAttack()
    {
        int manaCost = 30;

        // ei tarpeeksi manaa
        if (mana == null || !mana.UseMana(manaCost))
        {
            Debug.Log("Not enough mana!");
            return;
        }

        // animaatio
        animator.SetTrigger("HeavyAttack");

        // enemmän damagea
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange * 1.5f, // isompi range
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage * 2);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
