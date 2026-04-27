using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public Image healthBar;
    public Animator animator;

    private void Start()
    {
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GameManager.instance.playerHealth = currentHealth;

        animator.SetTrigger("TakeDamage");

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.instance.playerHealth = GameManager.instance.maxHealth;

        currentHealth = GameManager.instance.maxHealth;

        UpdateUI();

        FindAnyObjectByType<DeathScreen>().PlayerDied();
    }
}