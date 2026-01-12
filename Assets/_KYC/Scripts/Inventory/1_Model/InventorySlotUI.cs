using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;    // 자식 Item(button)의 Image 컴포넌트
    [SerializeField] private TextMeshProUGUI countText; // 자식 CountText(TMP)

    [HideInInspector] public int slotIndex;


    public void UpdateSlot(Sprite icon, int count)
    {
        if (icon != null)
        {
            itemIcon.sprite = icon;
            itemIcon.gameObject.SetActive(true);
            itemIcon.color = Color.white; // 드래그 종료 후 투명도 복구용
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }

        countText.text = count > 1 ? count.ToString() : "";
        countText.gameObject.SetActive(count > 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // 1. MasterManager의 UIManager를 통해 고스트 아이콘 참조
        var ui = MasterManager.UI;
        if (ui.GhostIconImage == null) return;

        // 2. 아이템 데이터 확인
        ItemData item = MasterManager.Inventory.GetItemInSlot(slotIndex);
        if (item == null) return;

        // 3. 고스트 아이콘 설정 및 활성화
        ui.GhostIconImage.sprite = item.icon;
        ui.GhostIconImage.gameObject.SetActive(true);

        // 원래 슬롯 투명도 조절
        itemIcon.color = new Color(1, 1, 1, 0.5f);
        ui.GhostRect.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var ui = MasterManager.UI;
        if (ui.GhostIconImage.gameObject.activeSelf)
        {
            ui.GhostRect.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var ui = MasterManager.UI;
        if (ui.GhostIconImage == null) return;

        ui.GhostIconImage.gameObject.SetActive(false);
        itemIcon.color = Color.white;

        GameObject target = eventData.pointerEnter;
        if (target != null)
        {
            InventorySlotUI targetSlot = target.GetComponentInParent<InventorySlotUI>();
            if (targetSlot != null && targetSlot != this)
            {
                MasterManager.Inventory.SwapItems(this.slotIndex, targetSlot.slotIndex);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 1. 데이터 매니저를 통해 실제 슬롯 데이터 확인
        var player = MasterManager.Data.Player;
        if (slotIndex < 0 || slotIndex >= player.inventorySlots.Count) return;

        var slotData = player.inventorySlots[slotIndex];
        if (slotData.item == null) return;

        // 2. 마우스 우클릭인지 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 3. 상점이 열려있는지 UIManager에게 물어봄
            if (MasterManager.UI.IsShopOpen)
            {
                // 판매 팝업 호출 (UIManager 경유)
                MasterManager.UI.ShowSellPopup(slotData.item, slotData.count);
                Debug.Log($"[Shop] {slotData.item.itemName} 판매 팝업 오픈");
            }
            else
            {
                // 상점이 닫혀있으면 일반 사용
                slotData.item.Use();
                MasterManager.Inventory.RefreshInventory();
            }
        }
    }
}