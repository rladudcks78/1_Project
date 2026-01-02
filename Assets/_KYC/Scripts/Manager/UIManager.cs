using UnityEngine;

/// <summary>
/// 모든 게임 내 UI(인벤토리, 대화창 등)의 상태를 관리
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject dialoguePanel;

    public void Init()
    {
        // 시작 시 모든 UI를 끄고 초기화
        CloseAllPanels();
        Debug.Log("UIManager: UI 시스템 초기화 완료.");
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);
        }
    }

    public void CloseAllPanels()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }
}