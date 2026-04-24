using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path => Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        SaveData data = new SaveData();

        data.playerHealth = GameManager.instance.playerHealth;
        data.playerMana = GameManager.instance.playerMana;

        data.maxMana = GameManager.instance.maxMana;
        data.maxHealth = GameManager.instance.maxHealth;

        data.attackPower = GameManager.instance.attackPower;

        data.orbs = GameManager.instance.orbs;

        data.level = GameManager.instance.level;
        data.exp = GameManager.instance.exp;
        data.expToNextLevel = GameManager.instance.expToNextLevel;

        data.posX = GameManager.instance.respawnPosition.x;
        data.posY = GameManager.instance.respawnPosition.y;

        data.sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Game saved to: " + path);
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool SaveExists()
    {
        return File.Exists(path);
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}