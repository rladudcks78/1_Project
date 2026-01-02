using UnityEngine;

public enum ItemType
{
    Seed,   // 씨앗
    Tool,   // 도구
    Food,   // 음식
    Resource // 자원 (나무, 돌 등)
}

/// <summary>
/// 모든 아이템의 최상위 부모 클래스
/// </summary>
public abstract class ItemData : ScriptableObject
{
    [Header("Common Info")]
    public string itemID;       // 고유 식별자
    public string itemName;     // 아이템 이름
    [TextArea]
    public string description;  // 아이템 설명
    public Sprite icon;         // 인벤토리 표시용 아이콘
    public ItemType type;       // 아이템 타입 
    public int maxStack = 99;   // 최대 겹치기 개수

    /// <summary>
    /// 아이템 사용 시 호출되는 추상 메서드
    /// 자식 클래스에서 각자 다르게 구현합니다.
    /// </summary>
    public abstract void Use();
}