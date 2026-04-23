using UnityEngine;
using TMPro;

public class OrbUI : MonoBehaviour
{
    public TextMeshProUGUI orbText;

    void Update()
    {
        orbText.text = " : " + GameManager.instance.orbs;
    }
}