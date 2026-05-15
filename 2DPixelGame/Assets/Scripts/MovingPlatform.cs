using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 moveDirection = Vector2.right;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool movingForward = true;

    void Start()
    {
        startPosition = transform.position;

        targetPosition = startPosition +
            (Vector3)(moveDirection.normalized * moveDistance);
    }

    void Update()
    {
        Vector3 destination = movingForward
            ? targetPosition
            : startPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            moveSpeed * Time.deltaTime
        );

        // Kun saavutaan pisteeseen → vaihda suunta
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            movingForward = !movingForward;
        }
    }
}