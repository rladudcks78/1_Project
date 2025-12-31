using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 플레이어의 현재 상태를 저장하는 데이터 컨테이너입니다.
/// ScriptableObject를 사용하여 게임 실행 중에도 값이 유지되며, 인스펙터에서 실시간 확인이 가능합니다.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Farm/Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Info")]
    public string playerName = "New Farmer";
    public int gold;
    public int moveSpeed;

    [Header("Inventory")]
    // 실제 게임에서는 ID를 저장하는 방식이 좋지만, 
    // 개발 초기 단계에서는 직관성을 위해 Slot 클래스를 사용합니다.
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    /// <summary>
    /// 데이터를 초기화하는 메서드 (새 게임 시작 시 호출)
    /// </summary>
    public void ResetData(int gold, int moveSpeed)
    {
        gold = 500;
        moveSpeed = 5;
        inventorySlots.Clear();
    }
}

/// <summary>
/// 인벤토리 한 칸의 정보를 담는 클래스
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemData item; // 위에서 만든 ItemData SO 참조
    public int count;

    public InventorySlot(ItemData newItem, int amount)
    {
        item = newItem;
        count = amount;
    }
}