using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// 플레이어의 모든 상호작용(NPC 대화, 농사, 수확)을 담당하는 클래스.
/// 포트폴리오 포인트: Input System과 UI EventSystem 간의 실행 순서 문제를 해결한 로직 포함.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    private PlayerController _controller;

    [Header("Reference")]
    [SerializeField] private ToolVisualizer _visualizer;

    // 상호작용 예약 플래그 (UI 체크 타이밍 문제를 피하기 위함)
    private bool _interactReserved = false;

    private void Awake() => _controller = GetComponent<PlayerController>();

    // InputManager의 이벤트를 구독
    private void OnEnable() => InputManager.OnInteract += ReserveInteract;
    private void OnDisable() => InputManager.OnInteract -= ReserveInteract;

    /// <summary>
    /// 입력이 들어왔을 때 즉시 실행하지 않고 '예약'만 합니다.
    /// 이렇게 하면 유니티의 Update 루프 안에서 안전하게 UI 점유 여부를 판단할 수 있습니다.
    /// </summary>
    private void ReserveInteract()
    {
        _interactReserved = true;
    }

    private void Update()
    {
        // 입력 예약이 되었을 때만 실행
        if (_interactReserved)
        {
            PerformInteract();
            _interactReserved = false; // 실행 후 예약 해제
        }
    }

    private void PerformInteract()
    {
        // 1. 현재 프레임에서 마우스가 UI 위에 있는지 체크
        // Update 내에서 실행하므로 IsPointerOverGameObject()가 정확한 상태를 반환합니다.
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 2. 대화 중이거나 상점이 열려 있다면 월드 클릭 무시
        if (MasterManager.Dialogue.isDialogueActive || MasterManager.Shop.IsShopActive)
        {
            return;
        }

        // 3. 마우스 위치 계산 (Z축은 0으로 고정)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // 4. NPC 상호작용 체크
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        if (hit != null && hit.TryGetComponent<NPCController>(out NPCController npc))
        {
            // 거리 체크 (2.5 유닛 이내일 때만 대화 가능)
            if (Vector2.Distance(transform.position, hit.transform.position) <= 2.5f)
            {
                npc.Interact();
                return; // NPC와 상호작용했다면 아래 농사 로직은 건너뜀
            }
        }

        // 5. 농사 및 아이템 사용 로직
        HandleWorldInteraction(mousePos);
    }

    private void HandleWorldInteraction(Vector3 mousePos)
    {
        ItemData item = MasterManager.Inventory.GetSelectedItem();

        if (item == null)
        {
            TryHarvest(mousePos); // 오직 맨손일 때만 수확 시도
            Debug.Log("맨손 상호작용: 수확 시도");
            return; // 맨손 로직 끝났으므로 종료
        }

        if (item != null)
        {
            // 1. 씨앗류 처리 (ID 범위를 200~299로 정했다고 가정)
            if (item is CropData seedData)
            {
                MasterManager.Tile.HandleInteraction(mousePos, "Seed", seedData);
            }
            // 2. 도구류 처리 (ID 101: 괭이)
            else if (item.itemID == 101)
            {
                MasterManager.Tile.HandleInteraction(mousePos, "Hoe");
            }
            // 3. 도구류 처리 (ID 104: 물뿌리개 - 향후 확장 대비)
            else if (item.itemID == 104)
            {
                MasterManager.Tile.HandleInteraction(mousePos, "WateringCan");
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

                    Debug.Log($"<color=cyan>{harvestedItem.itemName}</color> 수확 완료!");
                }
            }
        }
    }

    public void ChangeVisual(string toolName)
    {
        if (_visualizer != null) _visualizer.UpdateVisual(toolName);
    }
}