using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public GameObject root; // koko UI (panel)
    public Image fillImage;

    private Enemy boss;

    public void SetBoss(Enemy bossRef)
    {
        boss = bossRef;
    }

    void Update()
    {
        if (boss == null) return;

        fillImage.fillAmount = (float)boss.currentHealth / boss.maxHealth;
    }

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}