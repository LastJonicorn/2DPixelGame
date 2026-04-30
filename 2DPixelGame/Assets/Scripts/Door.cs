using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorID;
    public Animator animator;

    private bool isOpen;

    void OnEnable()
    {
        Events.OnDoorEvent += HandleDoorEvent;
    }

    void OnDisable()
    {
        Events.OnDoorEvent -= HandleDoorEvent;
    }

    void HandleDoorEvent(string id)
    {
        if (isOpen) return;

        if (id != doorID) return;

        Open();
    }

    public void Open()
    {
        isOpen = true;

        animator.SetTrigger("Open");
    }
}