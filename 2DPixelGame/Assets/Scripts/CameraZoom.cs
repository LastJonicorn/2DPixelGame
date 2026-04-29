using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    public void ActivateCameraB()
    {
        cameraB.gameObject.SetActive(true);
        cameraA.gameObject.SetActive(false);
    }

    public void ActivateCameraA()
    {
        cameraA.gameObject.SetActive(true);
        cameraB.gameObject.SetActive(false);
    }
}