using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private IInteractable currentInteractable;

    void Update()
    {
        if (currentInteractable != null && Input.GetButtonDown("Submit"))
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}