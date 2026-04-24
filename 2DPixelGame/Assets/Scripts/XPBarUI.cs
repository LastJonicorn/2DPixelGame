using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    public Image xpBar;

    void Start()
    {
        if (xpBar == null)
        {
            xpBar = GameObject.FindWithTag("XPBar").GetComponent<Image>();
        }
    }

    void Update()
    {
        GameManager gm = GameManager.instance;

        if (gm == null || xpBar == null) return;

        float ratio = (float)gm.exp / gm.expToNextLevel;

        xpBar.fillAmount = Mathf.Clamp01(ratio);
    }
}