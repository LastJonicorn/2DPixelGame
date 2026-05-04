using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyDashAI : MonoBehaviour
{
    [Header("Detection")]
    public float detectionDistance = 6f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 2f;

    private bool isDashing = false;
    private float lastDashTime;
    public LayerMask playerLayer;

    private Transform player;
    public Animator animator;
    private bool isDead = false;

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        if (isDashing) return;

        CheckForPlayer();
    }

    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
        }
    }

    void CheckForPlayer()
    {
        // määritä suunta (vain vasen/oikea)
        Vector2 direction = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, detectionDistance, playerLayer);

        Debug.DrawRay(transform.position, direction * detectionDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash(direction));
            }
        }
    }

    IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        animator.SetTrigger("Attack");

        float timer = 0f;

        while (timer < dashDuration)
        {
            timer += Time.deltaTime;

            // vain X-akseli
            transform.position += new Vector3(direction.x, 0f, 0f) * dashSpeed * Time.deltaTime;

            yield return null;
        }

        isDashing = false;
    }

    public void Die()
    {
        isDead = true;
        this.enabled = false; // lopettaa Update kokonaan
    }
}