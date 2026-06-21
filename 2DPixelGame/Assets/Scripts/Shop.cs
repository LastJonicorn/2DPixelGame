using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [Header("UI")]
    public GameObject promptUI;
    public GameObject shopUI;

    [Header("Navigation")]
    public GameObject firstSelectedButton;

    public CanvasGroup shopCanvasGroup;
    public GameObject selectionIndicator;

    private bool playerInside;
    private bool isOpen;

    void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetButtonDown("Submit"))
        {
            if (isOpen)
                CloseShop();
            else
                OpenShop();
        }
    }

    void OpenShop()
    {
        isOpen = true;

        shopUI.SetActive(true);
        promptUI.SetActive(false);

        GameManager.instance.inputLocked = true;

        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;

        StartCoroutine(SelectAfterFrame(firstSelectedButton));
    }

    void CloseShop()
    {
        isOpen = false;

        if (shopUI != null)
            shopUI.SetActive(false);

        GameManager.instance.inputLocked = false;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void DisableShopNavigation()
    {
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;

        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }


    public void EnableShopNavigation()
    {
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;

        if (selectionIndicator != null)
            selectionIndicator.SetActive(true);

        StartCoroutine(SelectAfterFrame(firstSelectedButton));
    }

    IEnumerator SelectAfterFrame(GameObject obj)
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;
        promptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        promptUI.SetActive(false);
        playerInside = false;

        if (shopUI != null)
            CloseShop();
    }
}