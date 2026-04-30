using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LockedDoor : MonoBehaviour
{
    public int sceneIndex;

    [Header("Keys")]
    public int requiredKeys = 3;

    [Header("UI")]
    public GameObject prompt;
    public TMP_Text promptText;

    [Header("Door (optional)")]
    public Animator animator;

    private bool playerInZone = false;
    private bool isUnlocked = false;

    void Update()
    {
        if (!playerInZone) return;

        UpdateUI();

        if (Input.GetButtonDown("Submit"))
        {
            TryEnter();
        }
    }

    void UpdateUI()
    {
        int currentKeys = GameManager.instance.keys;

        if (currentKeys >= requiredKeys)
        {
            isUnlocked = true;

            promptText.text = $"Keys needed {currentKeys}/{requiredKeys}";
            promptText.color = Color.green;
        }
        else
        {
            isUnlocked = false;

            promptText.text = $"Keys needed {currentKeys}/{requiredKeys}";
            promptText.color = Color.red;
        }
    }

    void TryEnter()
    {
        if (!isUnlocked)
        {
            Debug.Log("Not enough keys!");
            return;
        }

        // 🔥 avaa ovi (jos on animaatio)
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        // 🔥 siirry sceneen
        SaveSystem.SaveGame();
        FadeManager.instance.FadeToScene(sceneIndex);
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (!player.CompareTag("Player")) return;

        playerInZone = true;

        if (prompt != null)
            prompt.SetActive(true);

        UpdateUI();
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (!player.CompareTag("Player")) return;

        playerInZone = false;

        if (prompt != null)
            prompt.SetActive(false);
    }
}