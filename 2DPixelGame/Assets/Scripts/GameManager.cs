using UnityEngine;
using UnityEngine.SceneManagement;

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
    private string lastSceneName;

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
    private void Start()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Jos scene on sama → EI resetoida (kuolema)
        if (scene.name == lastSceneName)
        {
            return;
        }

        // Uusi scene → resetoi checkpoint
        hasCheckpoint = false;
        currentCheckpoint = null;

        lastSceneName = scene.name;
    }
}