using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Farm/Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Info")]
    public string playerName = "New Farmer";
    public int gold;
    public int moveSpeed;

    [Header("Inventory")]
    // [중요] 아래에 정의된 InventorySlot 클래스를 리스트로 사용합니다.
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int maxSlotCount = 20;

    public bool AddItem(ItemData item, int count)
    {
        // 1. 같은 아이템이 있는지 확인 (기존 로직 유지)
        InventorySlot existingSlot = inventorySlots.Find(slot => slot.item == item);
        if (existingSlot != null)
        {
            existingSlot.count += count;
            return true;
        }

        // 2. [수정] 빈 슬롯(item이 null인 곳)을 찾아서 데이터만 채워넣기
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == null)
            {
                inventorySlots[i].item = item;
                inventorySlots[i].count = count;
                return true; // 빈 자리에 채웠으므로 성공 반환
            }
        }

        Debug.LogWarning("모든 슬롯이 아이템으로 가득 찼습니다!");
        return false;
    }

    public void ResetData()
    {
        gold = 500;
        moveSpeed = 5;
        inventorySlots.Clear();
    }
}

// [핵심] 클래스 중괄호 외부(끝난 뒤)에 정의해야 다른 파일에서 찾을 수 있습니다.
[System.Serializable]
public class InventorySlot
{
    public ItemData item; // ItemData 추상 클래스를 참조
    public int count;

    public InventorySlot(ItemData newItem, int amount)
    {
        item = newItem;
        count = amount;
    }
}