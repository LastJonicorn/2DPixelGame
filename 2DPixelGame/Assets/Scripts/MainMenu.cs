using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject settingsPanel;

    public GameObject firstButton;      // New Game
    public GameObject continueButton;   // 🔥 uusi

    private void Start()
    {
        bool hasSave = SaveSystem.SaveExists();

        // Näytä/piilota Continue
        continueButton.SetActive(hasSave);

        // Valitse oikea nappi ohjaimelle
        if (hasSave)
        {
            EventSystem.current.SetSelectedGameObject(continueButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    public void PlayGame()
    {
        // 🔥 New Game → resetoi save
        SaveSystem.DeleteSave();
        GameManager.instance.ResetData();

        FadeManager.instance.FadeToScene(1);
    }

    public void ContinueGame()
    {
        GameManager.instance.LoadGame();
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}