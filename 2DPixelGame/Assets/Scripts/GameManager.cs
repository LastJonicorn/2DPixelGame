using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Data")]
    public float playerHealth = 100;
    public int maxHealth = 100;

    public int orbs = 0;
    public int keys = 0;

    public int attackPower = 20;
    public int heavyAttackPower = 25;

    public int level = 1;
    public int exp = 0;
    public int expToNextLevel = 100;

    public float playerMana = 100;
    public float maxMana = 100;

    public Vector3 respawnPosition;
    public bool hasCheckpoint = false;
    public Checkpoint currentCheckpoint;
    private string lastSceneName;
    public string lastCheckpointScene;

    private bool loadingFromSave = false;

    public bool inputLocked = false;

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
        // 🔥 ÄLÄ RESETOI jos tullaan savesta
        if (loadingFromSave)
        {
            loadingFromSave = false;
            lastSceneName = scene.name;
            return;
        }

        // Jos sama scene → kuolema → ei reset
        if (scene.name == lastSceneName)
        {
            return;
        }

        // 🔥 ÄLÄ resetoi jos ollaan checkpointin scenessä
        if (scene.name == lastCheckpointScene)
        {
            lastSceneName = scene.name;
            return;
        }

        hasCheckpoint = false;
        currentCheckpoint = null;

        lastSceneName = scene.name;
    }

    public void AddExp(int amount)
    {
        exp += amount;

        if (exp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        exp -= expToNextLevel;
        level++;

        expToNextLevel += 75; // optional scaling

        // resettaa resurssit
        playerHealth = maxHealth;
        playerMana = maxMana;

        SyncPlayerStats();
        SyncAll();

        // avataan valinta UI:lle
        FindAnyObjectByType<LevelUpUI>().Open();
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        Debug.Log("Loaded orbs: " + orbs);
        Debug.Log("Loaded health: " + playerHealth);
        Debug.Log("Has checkpoint: " + hasCheckpoint);

        if (data.level <= 0)
        {
            level = 1;
            exp = 0;
            expToNextLevel = 100;
        }
        else
        {
            level = data.level;
            exp = data.exp;
            expToNextLevel = data.expToNextLevel;
        }

        playerMana = data.playerMana;
        maxMana = data.maxMana;

        playerHealth = data.playerHealth;
        maxHealth = data.maxHealth;

        level = data.level;
        exp = data.exp;
        expToNextLevel = data.expToNextLevel;

        attackPower = data.attackPower;
        heavyAttackPower = data.heavyAttackPower;

        orbs = data.orbs;
        keys = data.keys;

        respawnPosition = new Vector2(data.posX, data.posY + 2.0f);
        hasCheckpoint = true;

        loadingFromSave = true; // 🔥 TÄRKEÄ

        SceneManager.LoadScene(data.sceneIndex);
    }
    public void SyncPlayerStats()
    {
        PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
        if (ph != null)
        {
            ph.currentHealth = playerHealth;
        }

        PlayerMana pm = FindAnyObjectByType<PlayerMana>();
        if (pm != null)
        {
            pm.currentMana = playerMana;
        }
    }

    public void SyncAll()
    {
        PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
        if (ph != null)
        {
            ph.currentHealth = playerHealth;
            ph.UpdateUI();
        }

        PlayerMana pm = FindAnyObjectByType<PlayerMana>();
        if (pm != null)
        {
            pm.currentMana = playerMana;
            pm.UpdateUI();
        }
    }

    public void ResetData()
    {
        maxHealth = 100;
        playerHealth = maxHealth;

        maxMana = 100;
        playerMana = maxMana;

        orbs = 0;
        keys = 0;

        attackPower = 20;
        heavyAttackPower = 25;

        level = 1;
        exp = 0;
        expToNextLevel = 100;

    hasCheckpoint = false;
    }
}