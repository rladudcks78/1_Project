using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("MVP Connections")]
    [SerializeField] private InventoryView inventoryView;

    public void Init()
    {
        // 1. View 초기화
        inventoryView.InitView();

        // 2. View의 버튼 클릭 이벤트를 구독
        inventoryView.OnSlotClicked += HandleSlotClick;

        // 3. 현재 데이터 반영
        RefreshInventory();

        Debug.Log("InventoryManager: MVP 시스템 초기화 성공.");
    }

    public void RefreshInventory()
    {
        // DataManager의 public 프로퍼티인 Player(대문자)를 참조합니다.
        var slots = MasterManager.Data.Player.inventorySlots;

        for (int i = 0; i < 28; i++)
        {
            if (i < slots.Count)
            {
                inventoryView.RenderSlot(i, slots[i].item.icon, slots[i].count);
            }
            else
            {
                inventoryView.RenderSlot(i, null, 0);
            }
        }
    }

    public void AddItem(ItemData item, int count)
    {
        // Model(PlayerData)에 아이템 추가 명령
        bool isAdded = MasterManager.Data.Player.AddItem(item, count);

        if (isAdded)
        {
            // 데이터가 바뀌었으니 즉시 View(UI)를 새로고침
            RefreshInventory();
        }
    }

    private void HandleSlotClick(int index)
    {
        var playerModel = MasterManager.Data.Player;
        if (index >= playerModel.inventorySlots.Count) return;

        var slot = playerModel.inventorySlots[index];
        if (slot.item != null)
        {
            Debug.Log($"[MVP] {slot.item.itemName} 아이템 사용 버튼 클릭");
            slot.item.Use(); // 추상 메서드 호출
            RefreshInventory(); // 데이터 변화(수량 등)를 UI에 즉시 반영
        }
    }
}