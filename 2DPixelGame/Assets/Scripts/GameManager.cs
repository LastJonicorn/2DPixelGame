using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Data")]
    public float playerHealth = 100;
    public int maxHealth = 100;
    public int orbs = 0;
    public int exp = 0;
    public int playerMana = 0;
    public int maxMana = 0;

    public Vector3 respawnPosition;
    public bool hasCheckpoint = false;
    public Checkpoint currentCheckpoint;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}