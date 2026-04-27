using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifetime = 1f;

    private TextMeshPro text;
    private Color color;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        color = text.color;
    }

    public void Setup(int damage)
    {
        text.text = damage.ToString();
    }

    void Update()
    {
        // liikkuu ylöspäin
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // fade out
        color.a -= Time.deltaTime / lifetime;
        text.color = color;

        // tuho
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}