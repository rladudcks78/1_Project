using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemImage;      // 자식 ItemImage
    [SerializeField] private GameObject selectionFrame; // 자식 SelectFrame (GameObject로 설정)

    public void UpdateSlot(Sprite icon)
    {
        if (itemImage == null) return;

        if (icon != null)
        {
            itemImage.sprite = icon;
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            itemImage.gameObject.SetActive(false);
        }
    }

    // 이 슬롯의 프레임을 켜거나 끄는 함수
    public void SetHighlight(bool isActive)
    {
        if (selectionFrame != null)
        {
            selectionFrame.SetActive(isActive);
        }
    }
}