using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float currentMana;

    public Image manaBar;

    void Start()
    {
        currentMana = GameManager.instance.playerMana;

        if (manaBar == null)
            manaBar = GameObject.FindWithTag("ManaBar").GetComponent<Image>();

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        float maxMana = GameManager.instance.maxMana;

        if (maxMana <= 0f) return;

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

        float maxMana = GameManager.instance.maxMana;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        GameManager.instance.playerMana = currentMana;
    }
}