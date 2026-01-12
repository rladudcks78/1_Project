using UnityEngine;
using UnityEngine.InputSystem; // Input System 네임스페이스 추가

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private InventoryView inventoryView;

    [Header("Hotbar")]
    [SerializeField] private HotbarView hotbarView;
    private int _selectedSlotIndex = 0;

    [Header("Test Item Assets")]
    [SerializeField] private ItemData hoeItem;   // 괭이 아이템 SO
    [SerializeField] private ItemData seedItem;  // 씨앗 아이템 SO

    public ItemData SelectedItem
    {
        get
        {
            var slots = MasterManager.Data.Player.inventorySlots;
            if (_selectedSlotIndex < slots.Count) return slots[_selectedSlotIndex].item;
            return null;
        }
    }

    public void Init()
    {
        var player = MasterManager.Data.Player;

        // 인벤토리가 항상 28칸을 유지하도록 빈 슬롯 데이터로 미리 채움
        while (player.inventorySlots.Count < 28)
        {
            player.inventorySlots.Add(new InventorySlot(null, 0));
        }

        inventoryView.InitView();

        // [제거] inventoryView.OnSlotClicked += HandleSlotClick; 
        // 이제 버튼이 없으므로 View에서 이벤트를 쏴주지 않습니다.

        RefreshInventory();

        // 기본 0번 슬롯 선택
        SelectSlot(0);

        Debug.Log("InventoryManager: 버튼 없는 인벤토리 시스템 초기화 완료.");
    }

    private void Update()
    {
        // 1. 숫자키 1~7 입력 감지 (InputSystem 방식)
        CheckHotbarInput();

        // 2. [테스트 요청 사항] 2, 3, 4번 키를 통한 아이템 강제 주입
        CheckTestInput();
    }

    private void CheckHotbarInput()
    {
        // Keyboard.current를 사용하여 1~7번 키 확인
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectSlot(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectSlot(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectSlot(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectSlot(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectSlot(4);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) SelectSlot(5);
        if (Keyboard.current.digit7Key.wasPressedThisFrame) SelectSlot(6);
    }

    private void CheckTestInput()
    {
        // 2번 키를 누르면 첫 번째 슬롯(0)에 괭이 주입
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            SetForceItem(0, hoeItem, 1);
            Debug.Log("테스트: 0번 슬롯에 괭이 주입");
        }

        // 3번 키를 누르면 두 번째 슬롯(1)에 씨앗 주입
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            SetForceItem(1, seedItem, 10);
            Debug.Log("테스트: 1번 슬롯에 씨앗 10개 주입");
        }

        // 4번 키는 '맨손'을 원하셨으므로, 아이템이 없는 7번 슬롯(핫바 외부)을 선택하거나 
        // 현재 선택된 슬롯이 빈 칸이 되도록 유도합니다. (여기서는 빈 6번 슬롯 선택으로 구현)
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            SelectSlot(6); // 핫바의 마지막 칸(보통 비어있음) 선택
            Debug.Log("테스트: 맨손(빈 슬롯) 선택");
        }
    }

    // 테스트용: 데이터를 강제로 쑤셔넣는 함수
    private void SetForceItem(int index, ItemData item, int count)
    {
        var player = MasterManager.Data.Player;

        // 리스트 크기가 부족하면 확장
        while (player.inventorySlots.Count <= index)
        {
            player.inventorySlots.Add(new InventorySlot(null, 0));
        }

        player.inventorySlots[index] = new InventorySlot(item, count);
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        var slots = MasterManager.Data.Player.inventorySlots;

        for (int i = 0; i < 28; i++)
        {
            // 1. 데이터가 있는지 로그로 먼저 확인
            if (i < slots.Count && slots[i].item != null)
            {
                Debug.Log($"슬롯 {i}: {slots[i].item.itemName} 있음, 아이콘: {slots[i].item.icon.name}");
            }

            Sprite icon = (i < slots.Count && slots[i].item != null) ? slots[i].item.icon : null;
            int count = (i < slots.Count) ? slots[i].count : 0;

            inventoryView.RenderSlot(i, icon, count); //
            if (i < 7) hotbarView.RenderSlot(i, icon); //
        }
    }

    public void AddItem(ItemData item, int amount)
    {
        // 1. 실제 데이터 계산은 PlayerData에게 시킵니다.
        bool success = MasterManager.Data.Player.AddItem(item, amount);

        // 2. 데이터 추가에 성공했다면 UI를 새로고침합니다.
        if (success)
        {
            RefreshInventory();
            Debug.Log($"{item.itemName} 추가 성공! UI를 갱신합니다.");
        }
    }

    //private void HandleSlotClick(int index)
    //{
    //    var playerModel = MasterManager.Data.Player;
    //    if (index >= playerModel.inventorySlots.Count) return;

    //    var slot = playerModel.inventorySlots[index];
    //    if (slot.item != null)
    //    {
    //        slot.item.Use();
    //        RefreshInventory();
    //    }
    //}

    public void SelectSlot(int index)
    {
        _selectedSlotIndex = index;
        hotbarView.SetSelection(index);

        var interactScript = Object.FindFirstObjectByType<PlayerInteract>();
        if (interactScript != null)
        {
            // SelectedItem이 null일 경우 "None"을 명시적으로 전달
            string nameToSend = (SelectedItem != null) ? SelectedItem.itemName : "None";
            interactScript.ChangeVisual(nameToSend);

            Debug.Log($"[Select] {index}번 슬롯 선택됨. 도구명: {nameToSend}");
        }
    }

    public ItemData GetSelectedItem()
    {
        // 이미 만들어두신 SelectedItem 프로퍼티를 활용하여 중복 로직을 방지합니다. (유지보수성 향상)
        return SelectedItem;
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        var playerModel = MasterManager.Data.Player;
        var slots = playerModel.inventorySlots;

        // [수정] 리스트의 Count가 아닌, 고정된 인벤토리 크기(28)를 기준으로 체크
        if (fromIndex < 0 || fromIndex >= 28 || toIndex < 0 || toIndex >= 28) return;
        if (fromIndex == toIndex) return;

        // 데이터 교체 (비어있는 칸이어도 null 데이터가 들어있으므로 정상 작동함)
        var temp = slots[fromIndex];
        slots[fromIndex] = slots[toIndex];
        slots[toIndex] = temp;

        Debug.Log($"[Inventory] 슬롯 교체 완료: {fromIndex} <-> {toIndex}");

        RefreshInventory();
        SelectSlot(_selectedSlotIndex);
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        // 1. 실제 데이터가 저장된 Player의 inventorySlots에 접근합니다.
        var slots = MasterManager.Data.Player.inventorySlots;

        // 2. 해당 아이템이 들어있는 '첫 번째' 슬롯을 찾습니다. (중첩 가능 아이템 고려)
        // 람다식을 사용하여 아이템 데이터(ScriptableObject)가 일치하는 슬롯을 검색합니다.
        var targetSlot = slots.Find(slot => slot.item == item);

        // 3. 슬롯을 찾았고, 보유 수량이 제거하려는 수량보다 많거나 같은지 확인합니다.
        if (targetSlot != null && targetSlot.count >= amount)
        {
            // 4. 수량 차감
            targetSlot.count -= amount;

            // 5. 수량이 0이 되면 슬롯 데이터 초기화
            if (targetSlot.count <= 0)
            {
                // [주의] InventorySlot 클래스에 item = null; count = 0; 로직이 포함되어 있어야 합니다.
                targetSlot.item = null;
                targetSlot.count = 0;
            }

            // 6. 변화가 생겼으니 UI를 새로고침합니다. (기존에 구현된 함수 활용)
            RefreshInventory();

            Debug.Log($"[Inventory] {item.itemName} {amount}개 제거 완료.");
            return true;
        }

        // 아이템이 없거나 수량이 부족한 경우
        Debug.LogWarning($"[Inventory] {item.itemName} 제거 실패: 수량 부족 또는 아이템 없음.");
        return false;
    }

    // 특정 슬롯의 아이템 데이터를 가져오는 도우미 함수 (드래그 시작 시 체크용)
    public ItemData GetItemInSlot(int index)
    {
        var slots = MasterManager.Data.Player.inventorySlots;
        if (index >= 0 && index < slots.Count) return slots[index].item;
        return null;
    }
}