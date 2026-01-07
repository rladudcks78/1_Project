using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;    // 자식 Item(button)의 Image 컴포넌트
    [SerializeField] private TextMeshProUGUI countText; // 자식 CountText(TMP)
    [SerializeField] private Button itemButton; // 자식 Item(button) 그 자체

    public Button SlotButton => itemButton;

    public void UpdateSlot(Sprite icon, int count)
    {
        if (icon != null)
        {
            itemIcon.sprite = icon;
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }

        // 수량 표시 (1개보다 많을 때만 숫자 노출)
        countText.text = count > 1 ? count.ToString() : "";
        countText.gameObject.SetActive(count > 0);
    }
}