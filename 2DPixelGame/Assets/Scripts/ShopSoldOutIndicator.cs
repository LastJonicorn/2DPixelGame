using UnityEngine;

public class ShopSoldOutIndicator : MonoBehaviour
{
    public GameObject soldOutSprite;

    void Start()
    {
        UpdateSoldOut();
    }

    public void UpdateSoldOut()
    {
        soldOutSprite.SetActive(GameManager.instance.hasLantern);
    }
}