using UnityEngine;
using TMPro;

public class PlayerStatusUI : MonoBehaviour
{
    public GameObject panel;

    [Header("Texts")]
    public TMP_Text orbsText;
    public TMP_Text levelText;
    public TMP_Text expText;
    public TMP_Text manaText;
    public TMP_Text healthText;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            Toggle();
        }
    }

    void Toggle()
    {
        isOpen = !isOpen;

        panel.SetActive(isOpen);

        if (isOpen)
        {
            Time.timeScale = 0f;
            UpdateUI();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void UpdateUI()
    {
        var gm = GameManager.instance;

        orbsText.text = "Orbs: " + gm.orbs;
        levelText.text = "Level: " + gm.level;
        expText.text = "EXP: " + gm.exp + " / " + gm.expToNextLevel;
        manaText.text = "Mana: " + gm.playerMana + " / " + gm.maxMana;
        healthText.text = "HP: " + gm.playerHealth + " / " + gm.maxHealth;
    }
}