using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItem[] items;

    public void BuyItem(int index)
    {
        ShopItem item = items[index];

        if (GameManager.instance.orbs < item.cost)
            return;

        if (AlreadyOwned(item))
            return;

        GameManager.instance.orbs -= item.cost;

        switch (item.itemType)
        {
            case ShopItem.ItemType.AttackPower:
                GameManager.instance.attackPower += item.value;
                break;

            case ShopItem.ItemType.HeavyAttackPower:
                GameManager.instance.heavyAttackPower += item.value;
                break;

            case ShopItem.ItemType.MaxHealth:
                GameManager.instance.maxHealth += item.value;
                break;

            case ShopItem.ItemType.Lantern:
                GameManager.instance.hasLantern = true;
                break;

            case ShopItem.ItemType.DoubleJump:
                GameManager.instance.hasDoubleJump = true;
                break;
        }

        SaveSystem.SaveGame();
    }

    bool AlreadyOwned(ShopItem item)
    {
        switch (item.itemType)
        {
            case ShopItem.ItemType.Lantern:
                return GameManager.instance.hasLantern;

            case ShopItem.ItemType.DoubleJump:
                return GameManager.instance.hasDoubleJump;
        }

        return false;
    }
}