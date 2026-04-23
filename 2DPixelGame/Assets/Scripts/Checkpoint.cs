using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject fire; // tämä on se tuli child-object

    void Start()
    {
        if (fire != null)
            fire.SetActive(false);

        // Jos tämä checkpoint on current → sytytä tuli
        if (GameManager.instance.currentCheckpoint == this)
        {
            TurnOnFire();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ActivateCheckpoint();

    }

    void ActivateCheckpoint()
    {
        // Jos tämä on jo aktiivinen → ei tehdä mitään
        if (GameManager.instance.currentCheckpoint == this) return;

        // 🔥 Sammuta vanhan checkpointin tuli
        if (GameManager.instance.currentCheckpoint != null)
        {
            GameManager.instance.currentCheckpoint.TurnOffFire();
        }

        // 🔥 Aseta tämä aktiiviseksi
        GameManager.instance.currentCheckpoint = this;

        GameManager.instance.respawnPosition = transform.position;
        GameManager.instance.hasCheckpoint = true;

        TurnOnFire();
    }

    public void TurnOnFire()
    {
        if (fire != null)
            fire.SetActive(true);
    }

    public void TurnOffFire()
    {
        if (fire != null)
            fire.SetActive(false);
    }
}