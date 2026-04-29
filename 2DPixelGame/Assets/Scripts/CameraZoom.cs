using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineCamera vcam;

    public float normalSize = 5f;
    public float bossSize = 8f;
    public float zoomSpeed = 3f;

    private float targetSize;

    void Start()
    {
        targetSize = normalSize;
    }

    void Update()
    {
        var lens = vcam.Lens;

        lens.OrthographicSize = Mathf.Lerp(
            lens.OrthographicSize,
            targetSize,
            Time.deltaTime * zoomSpeed
        );

        vcam.Lens = lens;
    }

    public void SetBossZoom()
    {
        targetSize = bossSize;
    }

    public void ResetZoom()
    {
        targetSize = normalSize;
    }
}