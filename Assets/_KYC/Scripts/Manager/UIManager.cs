using UnityEngine;
using UnityEngine.InputSystem; //

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel; //
    [SerializeField] private GameObject dialoguePanel; //
    [SerializeField] private GameObject shopPanel;

    public void Init()
    {
        CloseAllPanels(); //
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
            inventoryPanel.SetActive(isActive); //

            // 창이 열릴 때만 인벤토리 내용을 최신화
            if (isActive)
            {
                // MasterManager를 통해 Presenter의 리프레시 호출
                // MasterManager.Inventory.RefreshInventory(); 
            }
        }
    }

    public void OpenShopUI()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            // 상점이 열릴 때 다른 UI(인벤토리 등)를 닫고 싶다면 여기서 처리 가능합니다.
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
        if (shopPanel != null) shopPanel.SetActive(false);

        // [중요] 대화창을 닫을 때는 DialogueManager의 상태도 리셋해줘야 합니다.
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);
            if (MasterManager.Dialogue != null)
            {
                MasterManager.Dialogue.EndDialogue(); // 대화 종료 로직 강제 호출
            }
        }
    }
}