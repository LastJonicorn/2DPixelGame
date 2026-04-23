using UnityEngine;

public class OrbCollectible : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Collect();
    }

    void Collect()
    {
        GameManager.instance.orbs += value;

        Debug.Log("Orb collected! Total: " + GameManager.instance.orbs);

        Destroy(gameObject);
    }
}
