using UnityEngine;
using System;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Transform slotGrid;
    private InventorySlotUI[] uiSlots;

    public event Action<int> OnSlotClicked;

    public void InitView()
    {
        // Grid 자식들에 있는 모든 SlotUI를 배열로 가져옵니다.
        uiSlots = slotGrid.GetComponentsInChildren<InventorySlotUI>();

        for (int i = 0; i < uiSlots.Length; i++)
        {
            int index = i;
            // 버튼 클릭 이벤트를 Presenter로 전달합니다.
            uiSlots[i].SlotButton.onClick.RemoveAllListeners();
            uiSlots[i].SlotButton.onClick.AddListener(() => OnSlotClicked?.Invoke(index));
        }
    }

    public void RenderSlot(int index, Sprite icon, int count)
    {
        if (index < uiSlots.Length)
        {
            uiSlots[index].UpdateSlot(icon, count);
        }
    }
}