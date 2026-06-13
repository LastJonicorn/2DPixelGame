using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public int cost;

    public enum ItemType
    {
        AttackPower,
        HeavyAttackPower,
        MaxHealth,

        Lantern,
        DoubleJump
    }

    public ItemType itemType;

    public int value;
}