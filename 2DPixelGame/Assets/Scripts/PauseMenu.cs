using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject firstButton;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;

        //Fix for the shop bug
        FindAnyObjectByType<Shop>()?.EnableShopNavigation();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;

        //Fix for the shop bug
        FindAnyObjectByType<Shop>()?.DisableShopNavigation();

        EventSystem.current.SetSelectedGameObject(firstButton);

    }

    public void LoadHub()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
            return;

        Time.timeScale = 1;
        FadeManager.instance.FadeToScene(2);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        FadeManager.instance.FadeToScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
