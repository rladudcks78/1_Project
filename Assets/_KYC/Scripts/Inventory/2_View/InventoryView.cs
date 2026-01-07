using UnityEngine;
using System;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Transform slotGrid;
    private InventorySlotUI[] uiSlots;

    public event Action<int> OnSlotClicked;

    public void InitView()
    {
        // Grid의 자식들로부터 SlotUI를 다 가져옵니다.
        uiSlots = slotGrid.GetComponentsInChildren<InventorySlotUI>();

        for (int i = 0; i < uiSlots.Length; i++)
        {
            int index = i;

            // [중요] 각 슬롯 UI에 인덱스를 직접 대입해줍니다.
            uiSlots[i].slotIndex = index;

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