using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float maxMana = 100f;
    public float currentMana;

    public Image manaBar;

    void Start()
    {
        maxMana = GameManager.instance.maxMana;
        currentMana = GameManager.instance.playerMana;

        if (manaBar == null)
            manaBar = GameObject.FindWithTag("ManaBar").GetComponent<Image>();

        UpdateManaUI(); // 🔥 TÄRKEÄ
    }

    void Update()
    {
        if (maxMana <= 0f) return;

        UpdateManaUI();
    }

    void UpdateManaUI()
    {
        manaBar.fillAmount = currentMana / maxMana;
    }

    public bool UseMana(float amount)
    {
        if (currentMana < amount) return false;

        currentMana -= amount;
        GameManager.instance.playerMana = currentMana;

        return true;
    }

    public void AddMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        GameManager.instance.playerMana = currentMana;
    }
}