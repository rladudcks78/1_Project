using UnityEngine;

// ItemType은 ItemData 파일 안에 있어도 되고 별도로 있어도 됩니다.
public enum ItemType { Seed, Tool, Food, Resource }

public abstract class ItemData : ScriptableObject
{
    [Header("Common Info")]
    public string itemID;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemType type;
    public int buyPrice;
    public int sellPrice;
    public int maxStack = 99;

    public abstract void Use();
}