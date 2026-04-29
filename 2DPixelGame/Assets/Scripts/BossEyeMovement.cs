using UnityEngine;

public class BossEyeMovement : MonoBehaviour
{
    public float speed = 3f;

    [Header("Movement Area")]
    public Collider2D movementBounds; // box collider alueelle

    [Header("Stun")]
    public float stunDuration = 0.5f;

    private Transform player;
    private bool isStunned;
    private float stunTimer;

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

        // stun logic
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }
            return;
        }

        MoveTowardsPlayer();
    }

    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPos = (Vector2)transform.position + direction * speed * Time.deltaTime;

        // clamp liike alueeseen
        if (movementBounds != null)
        {
            Bounds bounds = movementBounds.bounds;

            newPos.x = Mathf.Clamp(newPos.x, bounds.min.x, bounds.max.x);
            newPos.y = Mathf.Clamp(newPos.y, bounds.min.y, bounds.max.y);
        }

        transform.position = newPos;
    }

    // kutsutaan kun boss ottaa damagea
    public void Stun()
    {
        isStunned = true;
        stunTimer = stunDuration;
    }
}
