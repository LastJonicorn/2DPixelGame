using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternLight : MonoBehaviour
{
    public Light2D lightComp;
    public float onIntensity = 1f;
    public float offIntensity = 0f;

    void Start()
    {
        if (lightComp == null)
            lightComp = GetComponent<Light2D>();

        UpdateLight();
    }

    public void UpdateLight()
    {
        if (GameManager.instance.hasLantern)
            lightComp.intensity = onIntensity;
        else
            lightComp.intensity = offIntensity;
    }
}