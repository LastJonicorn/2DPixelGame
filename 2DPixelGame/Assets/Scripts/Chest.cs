using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public string chestID;
    public GameObject itemPrefab;
    public Transform spawnPoint;
    //public float launchForce = 5f;
    public Animator animator;

    private bool isOpened = false;

    void Start()
    {
        if (GameManager.instance.openedChests.Contains(chestID))
        {
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        if (isOpened) return;

        OpenChest();
    }

    void OpenChest()
    {
        if (isOpened) return;

        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        SpawnItem();

        // 🔥 TÄMÄ ON AINOA MITÄ TARVITAAN
        GameManager.instance.openedChests.Add(chestID);

        Destroy(gameObject, 0.5f);
    }

    void SpawnItem()
    {
        GameObject item = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);

        /*
         * Rigidbody2D rb = item.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 force = new Vector2(Random.Range(-1f, 1f), 1f) * launchForce;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        */
    }
}