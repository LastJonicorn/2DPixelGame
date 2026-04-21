using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }

    public void AddHealth(int health)
    {
        currentHealth += health;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("TakeDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }
}
