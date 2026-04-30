using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class BossEyeMovement : MonoBehaviour
{
    enum State
    {
        Chase,
        Shooting,
        Stunned
    }

    public bool facingRight = true;
    public bool facingLeft = false;
    private bool isDead = false;

    public float speed = 3f;
    public Animator animator;

    [Header("Movement Area")]
    public Collider2D movementBounds;

    private Transform player;

    [Header("Stun")]
    public float stunDuration = 0.5f;
    private float stunTimer;

    [Header("Knockback")]
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.2f;

    private float knockbackTimer;
    private Vector2 knockbackDirection;

    [Header("Radial Attack")]
    public GameObject projectilePrefab;
    public int projectileCount = 12;
    public float shootCooldown = 3f;
    public float shootDelay = 0.5f;

    private float lastShootTime;
    private float shootTimer;
    private bool isShooting;

    private State currentState = State.Chase;

    private BossHealthBar healthBar;
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    void Start()
    {
        FindPlayer();
        healthBar = FindAnyObjectByType<BossHealthBar>();

        if (healthBar != null)
        {
            healthBar.SetBoss(GetComponent<Enemy>());
            healthBar.Hide(); // aluksi piilossa
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        bool playerInside = movementBounds.bounds.Contains(player.position);

        if (healthBar != null)
        {
            if (playerInside)
                healthBar.Show();
            else
                healthBar.Hide();
        }

        if (playerInside && cameraA != null && cameraB != null)
        {
            ActivateCameraB();
        }
        else
        {
            ActivateCameraA();
        }

        if (movementBounds != null && !movementBounds.bounds.Contains(player.position))
            return;

        switch (currentState)
        {
            case State.Chase:
                HandleChase();
                break;

            case State.Shooting:
                HandleShooting();
                break;

            case State.Stunned:
                HandleStun();
                break;
        }

        ClampToBounds();
    }
    public void ActivateCameraB()
    {
        cameraB.gameObject.SetActive(true);
        cameraA.gameObject.SetActive(false);
    }

    public void ActivateCameraA()
    {
        cameraA.gameObject.SetActive(true);
        cameraB.gameObject.SetActive(false);
    }

    void HandleChase()
    {
        FacePlayer();
        MoveTowardsPlayer();

        if (Time.time >= lastShootTime + shootCooldown)
        {
            currentState = State.Shooting;
            shootTimer = shootDelay;

            //animator.SetTrigger("Attack"); // optional animaatio
        }
    }
    void HandleShooting()
    {
        shootTimer -= Time.deltaTime;

        // boss pysyy paikallaan ja katsoo pelaajaa
        FacePlayer();

        if (shootTimer <= 0f && !isShooting)
        {
            FireRadialProjectiles();
            isShooting = true;
        }

        // pieni viive ennen kuin palaa chaseen
        if (shootTimer <= -0.2f)
        {
            lastShootTime = Time.time;
            isShooting = false;
            currentState = State.Chase;
        }
    }

    void FireRadialProjectiles()
    {
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            proj.GetComponent<EnemyProjectile>().InitDirection(dir);
        }
    }

    void HandleStun()
    {
        stunTimer -= Time.deltaTime;

        // 🔥 knockback liike
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;

            Vector2 newPos = (Vector2)transform.position + knockbackDirection * knockbackForce * Time.deltaTime;

            ClampPosition(ref newPos);
            transform.position = newPos;
        }

        if (stunTimer <= 0f)
        {
            currentState = State.Chase;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPos = (Vector2)transform.position + direction * speed * Time.deltaTime;

        ClampPosition(ref newPos);

        transform.position = newPos;
    }

    void ClampToBounds()
    {
        if (movementBounds == null) return;

        Vector2 pos = transform.position;
        ClampPosition(ref pos);
        transform.position = pos;
    }

    void ClampPosition(ref Vector2 pos)
    {
        if (movementBounds == null) return;

        Bounds bounds = movementBounds.bounds;

        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);
    }

    public void Stun()
    {
        stunTimer = stunDuration;
        currentState = State.Stunned;

        ApplyKnockback();
    }
    void ApplyKnockback()
    {
        if (player == null) return;

        Vector2 dir = (Vector2)transform.position - (Vector2)player.position;

        float verticalDiff = player.position.y - transform.position.y;

        // 🔥 JOS PELAAJA YLÄPUOLELLA → pakota sivulle
        if (verticalDiff > 0.5f)
        {
            float horizontalDir = Mathf.Sign(dir.x);

            // jos ollaan suoraan päällä → valitse random suunta
            if (Mathf.Abs(horizontalDir) < 0.1f)
            {
                horizontalDir = Random.value > 0.5f ? 1f : -1f;
            }

            knockbackDirection = new Vector2(horizontalDir, 0.2f).normalized;
        }
        else
        {
            // normaali knockback
            knockbackDirection = dir.normalized;
        }

        knockbackTimer = knockbackDuration;
    }

    void FacePlayer()
    {
        float dirX = player.position.x - transform.position.x;

        if (dirX > 0f)
        {
            facingRight = true;
            facingLeft = false;

            animator.SetBool("FacingRight", true);
            animator.SetBool("FacingLeft", false);
        }
        else
        {
            facingRight = false;
            facingLeft = true;

            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", true);
        }
    }

    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
        }
    }

    public void Die()
    {
        ActivateCameraA();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 2f;
        }
        isDead = true;

        healthBar.Hide();
        Events.OnBossDeath?.Invoke();

        this.enabled = false; // lopettaa Update kokonaan
    }
}