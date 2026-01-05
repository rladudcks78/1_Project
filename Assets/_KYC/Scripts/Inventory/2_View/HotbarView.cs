using UnityEngine;
using System.Collections.Generic;

public class HotbarView : MonoBehaviour
{
    [Header("Slot References")]
    [SerializeField] private List<HotbarSlotUI> hotbarSlots = new List<HotbarSlotUI>();

    public void RenderSlot(int index, Sprite icon)
    {
        if (index < hotbarSlots.Count)
            hotbarSlots[index].UpdateSlot(icon);
    }

    public void SetSelection(int index)
    {
        // 1. 일단 모든 슬롯의 하이라이트를 끈다
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            hotbarSlots[i].SetHighlight(false);
        }

        // 2. 선택된 번호의 슬롯만 하이라이트를 켠다
        if (index < hotbarSlots.Count)
        {
            hotbarSlots[index].SetHighlight(true);
        }
    }
}