using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel;

    private bool isDead = false;

    public GameObject firstButton;

    public void PlayerDied()
    {
        if (isDead) return;

        isDead = true;
        deathPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstButton);

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        GameManager.instance.playerHealth = GameManager.instance.maxHealth;
        GameManager.instance.playerMana = GameManager.instance.maxMana;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        FadeManager.instance.FadeToScene(0);
    }
}
