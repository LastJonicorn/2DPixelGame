using UnityEngine;

public class BossEyeMovement : MonoBehaviour
{
    enum State
    {
        Chase,
        Stunned
    }

    public bool facingRight = true;
    public bool facingLeft = false;

    public float speed = 3f;
    public Animator animator;

    [Header("Movement Area")]
    public Collider2D movementBounds;

    private Transform player;

    [Header("Stun")]
    public float stunDuration = 0.5f;
    private float stunTimer;

    private State currentState = State.Chase;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // jos pelaaja ei ole alueella → boss ei aktivoidu
        if (movementBounds != null && !movementBounds.bounds.Contains(player.position))
            return;

        switch (currentState)
        {
            case State.Chase:
                HandleChase();
                break;

            case State.Stunned:
                HandleStun();
                break;
        }

        ClampToBounds();
    }

    void HandleChase()
    {
        FacePlayer();
        MoveTowardsPlayer();
    }

    void HandleStun()
    {
        stunTimer -= Time.deltaTime;

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
}