using UnityEngine;

public class LadderPassThrough : MonoBehaviour
{
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void IgnorePlayer(Collider2D playerCol, bool ignore)
    {
        Physics2D.IgnoreCollision(playerCol, col, ignore);
    }
}