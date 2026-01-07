using UnityEngine;

// 아이템 타입을 구분하여 로직 분기를 쉽게 만듭니다.
public enum ItemType { Seed, Tool, Food, Resource }

/// <summary>
/// 모든 아이템의 최상위 부모 클래스 (추상 클래스)
/// </summary>
public abstract class ItemData : ScriptableObject
{
    [Header("Common Info")]
    public int itemID;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemType type;
    public int buyPrice;
    public int sellPrice;
    public int maxStack = 99;

    // 아이템 클릭/사용 시 실행될 추상 메서드
    public abstract void Use();
}