using UnityEngine;
using TMPro;

public class LockedDoor : MonoBehaviour
{
    public int requiredKeys = 3;

    [Header("UI")]
    public GameObject keyUIPanel;
    public TMP_Text keyText;

    [Header("Door")]
    public Animator animator;

    private bool playerInside = false;
    private bool isOpen = false;

    void Update()
    {
        if (!playerInside || isOpen) return;

        // päivitä UI jatkuvasti
        UpdateUI();

        // Submit = esim E / A nappi
        if (Input.GetButtonDown("Submit"))
        {
            TryOpen();
        }
    }

    void UpdateUI()
    {
        int currentKeys = GameManager.instance.keys;
        keyText.text = $"Keys {currentKeys}/{requiredKeys}";
    }

    void TryOpen()
    {
        int currentKeys = GameManager.instance.keys;

        if (currentKeys >= requiredKeys)
        {
            Open();
        }
        else
        {
            Debug.Log("Not enough keys!");
            // myöhemmin: shake UI / sound
        }
    }

    void Open()
    {
        isOpen = true;

        animator.SetTrigger("Open");

        GetComponent<Collider2D>().enabled = false;
        keyUIPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInside = true;
        keyUIPanel.SetActive(true);

        UpdateUI();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInside = false;
        keyUIPanel.SetActive(false);
    }
}