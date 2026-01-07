using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Button itemIconButton;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private Button buyButton;

    private ItemData _item;

    public void SetItem(ItemData item)
    {
        _item = item;
        itemImage.sprite = item.icon;
        buyPriceText.text = $"{item.buyPrice}G";

        // 1. 아이콘 클릭 시 -> 설명창만 업데이트
        itemIconButton.onClick.RemoveAllListeners();
        itemIconButton.onClick.AddListener(() => {
            MasterManager.Shop.ShowItemDescription(_item);
        });

        // 2. 구매 버튼 클릭 시 -> 샵매니저의 기존 구매 함수 호출
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => {
            // 이미 ShopManager에 만들어두신 함수를 그대로 사용합니다.
            MasterManager.Shop.PurchaseItem(_item);
        });
    }
}