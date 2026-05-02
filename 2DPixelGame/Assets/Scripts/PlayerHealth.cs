using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public Image healthBar;
    public Animator animator;

    private Rigidbody2D rb;

    public float knockbackForce = 10f;
    public float knockbackUpForce = 3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 🔥 HAETAAN AINA GAMEMANAGERISTA
        currentHealth = GameManager.instance.playerHealth;

        if (healthBar == null)
        {
            healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
        }

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthBar.fillAmount = currentHealth / GameManager.instance.maxHealth;
    }

    public void AddHealth(int health)
    {
        currentHealth += health;

        float maxHealth = GameManager.instance.maxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        GameManager.instance.playerHealth = currentHealth;

        UpdateUI();
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        currentHealth -= damage;
        GameManager.instance.playerHealth = currentHealth;

        animator.SetTrigger("TakeDamage");

        ApplyKnockback(attackerPosition);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void ApplyKnockback(Vector2 attackerPosition)
    {
        float directionX = transform.position.x > attackerPosition.x ? 1f : -1f;

        // Kulma 45 astetta (voit säätää)
        Vector2 direction = new Vector2(directionX, 1f).normalized;

        rb.linearVelocity = direction * knockbackForce;
    }

    public void Die()
    {
        GameManager.instance.playerHealth = GameManager.instance.maxHealth;

        currentHealth = GameManager.instance.maxHealth;

        UpdateUI();

        FindAnyObjectByType<DeathScreen>().PlayerDied();
    }
}