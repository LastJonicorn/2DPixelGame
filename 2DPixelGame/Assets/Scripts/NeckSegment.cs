using UnityEngine;

public class NeckSegment : MonoBehaviour
{
    public Transform target;

    [Header("Follow")]
    public bool lockSegment = false;

    public float distance = 0.5f;
    public float followSpeed = 15f;

    void Update()
    {
        // 🔥 segment pysyy täysin paikallaan
        if (lockSegment)
            return;

        if (!target) return;

        Vector3 dir = transform.position - target.position;
        float currentDistance = dir.magnitude;

        if (currentDistance == 0f) return;

        Vector3 desiredPosition =
            target.position + dir.normalized * distance;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}