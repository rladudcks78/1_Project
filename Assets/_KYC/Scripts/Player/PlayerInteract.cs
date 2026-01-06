using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerController _controller;

    [Header("Reference")]
    [SerializeField] private ToolVisualizer _visualizer;

    private void Awake() => _controller = GetComponent<PlayerController>();

    private void OnEnable() => InputManager.OnInteract += PerformInteract;
    private void OnDisable() => InputManager.OnInteract -= PerformInteract;

    // [중요] Update에서 하던 숫자키 입력 로직은 InventoryManager로 옮겨졌으므로 삭제합니다.

    private void PerformInteract()
    {
        if (MasterManager.Dialogue.isDialogueActive) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        if (hit != null && hit.TryGetComponent<NPCController>(out NPCController npc))
        {
            if (Vector2.Distance(transform.position, hit.transform.position) <= 2.0f)
            {
                npc.Interact(); // 여기서 StartDialogue 호출
                return;
            }
        }


        float interactRange = 2.0f;
        float distance = Vector2.Distance(transform.position, mousePos);

        // 상호작용 가능 거리 체크
        if (distance > interactRange)
        {
            Debug.Log("<color=red>거리가 너무 멉니다.</color>");
            return;
        }

        // [핵심] 인벤토리 매니저로부터 현재 핫바에서 선택된 아이템 데이터를 가져옵니다.
        ItemData selectedItem = MasterManager.Inventory.SelectedItem;

        // 아이템이 없으면 "None", 있으면 아이템의 Type을 문자열로 가져옵니다 (Seed, Tool 등)
        string toolType = (selectedItem != null) ? selectedItem.type.ToString() : "None";

        // --- 상황별 로직 분기 ---

        if (toolType == "None")
        {
            // 1. 맨손 상태 -> 수확 시도
            TryHarvest(mousePos);
        }
        else
        {
            // 2. 도구나 씨앗을 들고 있는 상태
            HandleToolInteraction(mousePos, selectedItem, toolType);
        }
    }

    private void HandleToolInteraction(Vector3 mousePos, ItemData item, string toolType)
    {
        // 씨앗인 경우
        if (toolType == "Seed")
        {
            // [참고] Seed 아이템 데이터에는 CropData가 연결되어 있어야 합니다.
            // 만약 ItemData를 확장한 SeedItem 클래스가 있다면 형변환을 통해 가져옵니다.
            // 일단은 기존 방식대로 작동하도록 하되, 핫바 선택에 따라 비주얼을 업데이트합니다.

            // 테스트용 코드를 실제 아이템 데이터 기반으로 넘길 수 있게 확장 가능
            MasterManager.Tile.HandleInteraction(mousePos, "Seed");
        }
        else if (toolType == "Tool")
        {
            // 아이템 이름이나 ID로 괭이인지 확인
            if (item.itemName.Contains("괭이") || item.itemID.Contains("Hoe"))
            {
                MasterManager.Tile.HandleInteraction(mousePos, "Hoe");
            }
        }
    }

    private void TryHarvest(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent<Crop>(out Crop crop))
        {
            if (crop.CanHarvest())
            {
                ItemData harvestedItem = crop.GetHarvestItem();

                if (harvestedItem != null)
                {
                    MasterManager.Inventory.AddItem(harvestedItem, 1);
                    MasterManager.Tile.ResetToTilled(hit.transform.position);
                    Destroy(hit.collider.gameObject);

                    Debug.Log($"<color=cyan>{harvestedItem.itemName}</color> 수확 완료 및 땅 복구!");
                }
            }
        }
    }

    // [수정] 이제 이 메서드는 InventoryManager에서 슬롯이 바뀔 때 호출해주면 좋습니다.
    public void ChangeVisual(string toolName)
    {
        if (_visualizer != null)
        {
            _visualizer.UpdateVisual(toolName);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Gizmos.color = Vector2.Distance(transform.position, mousePos) <= 2.0f ? Color.green : Color.red;
        Gizmos.DrawWireSphere(mousePos, 0.2f);
    }
}