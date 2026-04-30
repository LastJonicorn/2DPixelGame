using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectorIndicator : MonoBehaviour
{
    public RectTransform indicator; // nuoli / highlight
    public Vector2 offset; // hienosäätö

    public float speed = 10f;

    public float amplitude = 10f;
    public float frequency = 2f;

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null) return;

        RectTransform target = selected.GetComponent<RectTransform>();
        if (target == null) return;

        float wave = Mathf.Sin(Time.unscaledTime * frequency) * amplitude;

        Vector3 targetPos = target.position + (Vector3)offset + new Vector3(wave, 0f, 0f);

        indicator.position = Vector3.Lerp(
            indicator.position,
            targetPos,
            Time.unscaledDeltaTime * speed
        );
    }
}