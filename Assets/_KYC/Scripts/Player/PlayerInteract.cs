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
    [Header("Settings")]
    [SerializeField] private float interactRange = 2.0f; // 상호작용 가능한 최대 거리

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
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (MasterManager.Dialogue.isDialogueActive || MasterManager.Shop.IsShopActive) return;

        // 3. 마우스 위치 계산 (Z축은 0으로 고정)
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPos);
        mousePos.z = 0;

        float distance = Vector2.Distance(transform.position, mousePos);

        if (distance > interactRange)
        {
            Debug.Log($"<color=yellow>너무 멉니다! (거리: {distance:F1})</color>");
            return; // 거리가 멀면 아래 로직을 실행하지 않고 종료
        }

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
        // 1. 범위 내의 모든 콜라이더를 다 가져옵니다.
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.25f);

        if (hits.Length == 0) return;

        Crop closestCrop = null;
        float closestDistance = float.MaxValue;

        // 2. 검색된 것들 중 마우스 클릭 지점과 가장 가까운 '작물'을 찾습니다.
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Crop>(out Crop crop))
            {
                float distance = Vector2.Distance(pos, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCrop = crop;
                }
            }
        }

        // 3. 가장 가까운 작물이 있고 수확 가능하다면 진행
        if (closestCrop != null)
        {
            if (closestCrop.CanHarvest())
            {
                ItemData harvestedItem = closestCrop.GetHarvestItem();
                if (harvestedItem != null)
                {
                    MasterManager.Inventory.AddItem(harvestedItem, 1);
                    MasterManager.Tile.ResetToTilled(closestCrop.transform.position);

                    Debug.Log($"<color=cyan>{harvestedItem.itemName}</color> 수확 완료!");
                    Destroy(closestCrop.gameObject);
                }
            }
            else
            {
                Debug.Log("아직 다 자라지 않았습니다.");
            }
        }
    }

    public void ChangeVisual(string toolName)
    {
        if (_visualizer != null) _visualizer.UpdateVisual(toolName);
    }

    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        mousePos.z = 0;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mousePos, 0.25f); // 수확 판정 범위 시각화
    }
}