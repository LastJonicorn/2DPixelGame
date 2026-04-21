using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    // Update is called once per frame
    void Update()
    {
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.GameIsPaused) return;

        if (Input.GetButtonDown("Attack"))
        {
            Attack();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
