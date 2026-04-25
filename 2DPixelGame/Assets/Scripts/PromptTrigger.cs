using UnityEngine;

public class PromptTrigger : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (panel != null)
            panel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (panel != null)
            panel.SetActive(false);
    }
}