using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    private bool isOpen;

    void OnEnable()
    {
        Events.OnBossDeath += OpenDoor;
    }

    void OnDisable()
    {
        Events.OnBossDeath -= OpenDoor;
    }

    void OpenDoor()
    {
        isOpen = true;
        animator.SetTrigger("Open");
    }
}