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
        inventoryView.InitView();
        inventoryView.OnSlotClicked += HandleSlotClick;
        RefreshInventory();

        // 게임 시작 시 기본적으로 0번 슬롯 선택 상태로 시작
        SelectSlot(0);

        Debug.Log("InventoryManager: InputSystem 기반 시스템 초기화 성공.");
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

        // 메인 인벤토리(28칸)와 핫바(7칸) 갱신
        for (int i = 0; i < 28; i++)
        {
            Sprite icon = (i < slots.Count && slots[i].item != null) ? slots[i].item.icon : null;
            int count = (i < slots.Count) ? slots[i].count : 0;

            inventoryView.RenderSlot(i, icon, count);
            if (i < 7) hotbarView.RenderSlot(i, icon);
        }
    }

    public void AddItem(ItemData item, int count)
    {
        if (MasterManager.Data.Player.AddItem(item, count))
        {
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
            slot.item.Use();
            RefreshInventory();
        }
    }

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
}