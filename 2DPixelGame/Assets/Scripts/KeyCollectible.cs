using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public int value = 1;

    private bool collected = false;

    //[Header("Audio")]
    //public AudioSource audioSource;
    //public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;
        if (!collision.CompareTag("Player")) return;

        collected = true;
        //AudioSource.PlayClipAtPoint(collectSound, transform.position);
        Collect();
    }

    void Collect()
    {
        GameManager.instance.keys += value;
        SaveSystem.SaveGame();
        //Debug.Log("Orb collected! Total: " + GameManager.instance.orbs);

        Destroy(gameObject);
    }
}
