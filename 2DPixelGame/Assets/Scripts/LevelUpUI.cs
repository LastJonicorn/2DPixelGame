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
        Close();
    }

    public void AddMana()
    {
        GameManager.instance.maxMana += 10;
        Close();
    }

    public void AddAttack()
    {
        GameManager.instance.attackPower += 10;
        Close();
    }

    void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}