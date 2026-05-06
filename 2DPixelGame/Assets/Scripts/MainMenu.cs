using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject settingsPanel;

    public GameObject firstButton;
    public GameObject continueButton;

    [Header("Settings Navigation")]
    public GameObject firstSelectedInSettings; // esim. Volume Slider

    private GameObject lastSelected;

    private void Start()
    {
        bool hasSave = SaveSystem.SaveExists();

        continueButton.SetActive(hasSave);

        if (hasSave)
        {
            SelectUI(continueButton);
        }
        else
        {
            SelectUI(firstButton);
        }
    }

    public void PlayGame()
    {
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

    // 🔥 SETTINGS

    public void OpenSettingsPanel()
    {
        // tallenna nykyinen valinta
        lastSelected = EventSystem.current.currentSelectedGameObject;

        settingsPanel.SetActive(true);

        StartCoroutine(SelectAfterFrame(firstSelectedInSettings));
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);

        GameObject target = lastSelected != null ? lastSelected : firstButton;

        StartCoroutine(SelectAfterFrame(target));
    }

    IEnumerator SelectAfterFrame(GameObject obj)
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    void SelectUI(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}