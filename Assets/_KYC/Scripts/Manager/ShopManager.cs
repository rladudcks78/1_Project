using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private Transform content; // ShopSlot(Prefab)들이 생성될 곳
    [SerializeField] private Button closeButton;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI goldText;         // 이미지의 Gold(txt)
    [SerializeField] private TextMeshProUGUI descriptionText;  // 이미지의 Description(txt)

    [Header("Prefabs")]
    [SerializeField] private GameObject shopSlotPrefab; // ShopSlot 프리팹

    private NPCData _currentShopData;
    public bool IsShopActive { get; private set; }

    public void Init()
    {
        IsShopActive = false;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            // [수정] 직접 끄는 대신 UIManager를 통해 안전하게 닫도록 변경
            closeButton.onClick.AddListener(() => MasterManager.UI.CloseAllPanels());
        }
    }

    // 상점 열기: 대화 종료 후 호출될 함수
    public void OpenShop(NPCData npcData)
    {
        if (npcData == null) return;
        _currentShopData = npcData;

        if (shopSlotPrefab == null) return;

        // 1. UI 노출 명령 (UIManager 역할)
        MasterManager.UI.OpenShopUI();
        IsShopActive = true;

        // 2. 내용물 초기화 (ShopManager 역할)
        if (descriptionText != null) descriptionText.text = "";
        UpdateGoldUI();

        // 3. 슬롯 생성 (아이템 관리 로직)
        foreach (Transform child in content) Destroy(child.gameObject);

        foreach (ItemData item in _currentShopData.shopItems)
        {
            if (item == null) continue;
            GameObject slotObj = Instantiate(shopSlotPrefab, content);
            slotObj.GetComponent<ShopSlot>().SetItem(item);
        }
    }

    public void PurchaseItem(ItemData item)
    {
        if (item == null || MasterManager.Data == null) return;

        if (MasterManager.Data.Player.gold >= item.buyPrice)
        {
            MasterManager.Data.AddGold(-item.buyPrice);
            MasterManager.Inventory.AddItem(item, 1);

            UpdateGoldUI();

            if (descriptionText != null)
                descriptionText.text = $"<color=blue>{item.itemName}</color> BUY!";
        }
        else
        {
            if (descriptionText != null)
                descriptionText.text = "<color=red>소지금이 부족합니다!</color>";
        }
    }

    public void UpdateGoldUI()
    {
        if (goldText != null && MasterManager.Data != null)
            goldText.text = $"보유 금액: {MasterManager.Data.Player.gold}G";
    }

    public void ShowItemDescription(ItemData item)
    {
        if (item == null || descriptionText == null) return;

        // 기획에 맞춰 이름과 설명을 설명란에 표시
        descriptionText.text = $"<b>[{item.itemName}]</b>\n{item.description}";

        Debug.Log($"{item.itemName} 설명 표시 중");
    }

    public void SellItem(ItemData item, int amount)
    {
        // 1. 방어 코드: 데이터가 유효한지 체크
        if (item == null || MasterManager.Data == null) return;

        // 2. 판매 총액 계산
        int totalGold = item.sellPrice * amount;

        // 3. 인벤토리에서 아이템 제거 (InventoryManager에 RemoveItem이 구현되어 있어야 함)
        // 만약 RemoveItem이 없다면 인벤토리 로직에 맞춰 수정이 필요합니다.
        bool success = MasterManager.Inventory.RemoveItem(item, amount);

        if (success)
        {
            // 4. 데이터 매니저를 통해 플레이어 골드 증가
            MasterManager.Data.AddGold(totalGold);

            // 5. UI 갱신 (보유 금액 텍스트 등)
            UpdateGoldUI();

            Debug.Log($"[Shop] {item.itemName} {amount}개 판매 완료. +{totalGold}G");
        }
        else
        {
            Debug.LogError("[Shop] 인벤토리에서 아이템 제거에 실패했습니다.");
        }
    }

    public void SetShopInactive()
    {
        IsShopActive = false;
        _currentShopData = null;
        Debug.Log("Shop Logic: 상점 데이터 참조 해제");
    }
}