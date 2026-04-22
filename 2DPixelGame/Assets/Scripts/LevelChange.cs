using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int sceneIndex;

    private bool playerInZone = false;

    public GameObject prompt;

    void Update()
    {
        if (playerInZone && Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            playerInZone = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            playerInZone = false;
            prompt.SetActive(false);
        }
    }
}
