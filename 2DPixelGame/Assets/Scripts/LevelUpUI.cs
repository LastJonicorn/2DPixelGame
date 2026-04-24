using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject firstButton;

    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void AddHealth()
    {
        GameManager.instance.maxHealth += 10;

        // täytä HP heti
        GameManager.instance.playerHealth = GameManager.instance.maxHealth;

        Sync();
        Close();
    }

    public void AddMana()
    {
        GameManager.instance.maxMana += 10;

        GameManager.instance.playerMana = GameManager.instance.maxMana;

        Sync();
        Close();
    }

    public void AddAttack()
    {
        GameManager.instance.attackPower += 10;

        Sync();
        Close();
    }

    void Sync()
    {
        // pakota pelaaja + UI päivitys
        PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
        if (ph != null)
        {
            ph.currentHealth = GameManager.instance.playerHealth;
        }

        PlayerMana pm = FindAnyObjectByType<PlayerMana>();
        if (pm != null)
        {
            pm.currentMana = GameManager.instance.playerMana;
        }
    }

    void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}