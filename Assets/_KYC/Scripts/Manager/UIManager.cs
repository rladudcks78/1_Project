using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; //

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel; //
    [SerializeField] private GameObject dialoguePanel; //
    [SerializeField] private GameObject shopPanel;

    [Header("Sub Popups")]
    // [추가] 판매 확인 팝업 스크립트를 직접 참조합니다.
    [SerializeField] private SellConfirmationPopup sellConfirmationPopup;

    [Header("Drag & Drop References")]
    [SerializeField] private Image ghostIconImage;

    public Image GhostIconImage => ghostIconImage;
    public RectTransform GhostRect => ghostIconImage != null ? ghostIconImage.rectTransform : null;

    // 상점 활성화 여부를 외부에서 알 수 있게 프로퍼티로 노출
    public bool IsShopOpen => shopPanel != null && shopPanel.activeSelf;

    public void Init()
    {
        CloseAllPanels(); //
        if (ghostIconImage != null) ghostIconImage.gameObject.SetActive(false);
        if (sellConfirmationPopup != null) sellConfirmationPopup.gameObject.SetActive(false);
        Debug.Log("UIManager: UI 시스템 초기화 완료."); //
    }

    private void Update()
    {
        // 1. 인벤토리 토글 키 입력 체크 (Input System 방식)
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        // 2. ESC 키 입력 시 모든 창 닫기
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseAllPanels();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);
            if (isActive) MasterManager.Inventory.RefreshInventory();
        }
    }

    public void OpenShopUI()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);

            // 포트폴리오 포인트: 상점이 열리면 판매를 위해 인벤토리도 강제로 함께 엽니다.
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(true);
                MasterManager.Inventory.RefreshInventory();
            }
        }
    }

    public void ShowSellPopup(ItemData item, int count)
    {
        if (sellConfirmationPopup != null)
        {
            // 팝업 스크립트의 OpenPopup 호출
            sellConfirmationPopup.OpenPopup(item, count);
        }
    }

    public void OpenDialogueUI()
    {
        if (dialoguePanel != null)
        {
            // 대화창이 열릴 때는 다른 UI(인벤토리 등)를 닫아주는 것이 좋습니다.
            inventoryPanel.SetActive(false);
            shopPanel.SetActive(false);

            dialoguePanel.SetActive(true);
        }
    }

    public void CloseAllPanels()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        // 판매 팝업도 함께 닫아줍니다.
        if (sellConfirmationPopup != null) sellConfirmationPopup.gameObject.SetActive(false);

        if (shopPanel != null && shopPanel.activeSelf)
        {
            shopPanel.SetActive(false);
            MasterManager.Shop.SetShopInactive();
        }

        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);
            MasterManager.Dialogue.EndDialogue();
        }
    }
}