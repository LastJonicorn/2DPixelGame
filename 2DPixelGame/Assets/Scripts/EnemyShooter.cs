using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Animator animator;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    public float shootRange = 6f;

    private float lastShootTime;

    private bool isDead = false;

    private Transform player;

    void Update()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
                player = obj.transform;
            }
            else
            {
                return; // ei vielä pelaajaa
            }
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= shootRange && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        lastShootTime = Time.time;

        animator.SetTrigger("Attack");

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        proj.GetComponent<EnemyProjectile>().Init(player);
    }

    public void Die()
    {
        isDead = true;
        this.enabled = false; // lopettaa Update kokonaan
    }
}