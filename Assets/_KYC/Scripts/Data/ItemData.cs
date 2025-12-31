using UnityEngine;

public enum ItemType { Seed, Crop, Tool, Material }

[CreateAssetMenu(fileName = "Item_", menuName = "Farm/Item/Basic")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string id;              // 고유 ID (저장 시 사용)
    public string itemName;        // 표시 이름
    public Sprite icon;            // UI 아이콘
    public ItemType type;          // 아이템 타입

    [Header("경제 데이터")]
    public int buyPrice;           // 구매가
    public int sellPrice;          // 판매가

    [Header("최대 중첩 개수")]
    public int maxStack = 99;      // 인벤토리 한 칸당 최대 개수
}