using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject panel;
    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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
