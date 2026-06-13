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