using UnityEngine;

public class PogoPoint : MonoBehaviour
{
    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;
    public Transform effectPoint;

    public void OnHit()
    {
        if (hitEffectPrefab != null)
        {
            GameObject fx = Instantiate(
                hitEffectPrefab,
                effectPoint.position,
                Quaternion.identity
            );

            Destroy(fx, 2f);
        }
    }
}