using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerController _controller;
    private string _currentTool = "None";

    [Header("Reference")]
    [SerializeField] private ToolVisualizer _visualizer;

    [Header("Test Data")]
    [SerializeField] private CropData _testCrop; // 인스펙터에서 당근/밀 CropData를 할당하세요.

    private void Awake() => _controller = GetComponent<PlayerController>();

    private void OnEnable() => InputManager.OnInteract += PerformInteract;
    private void OnDisable() => InputManager.OnInteract -= PerformInteract;

    private void Start()
    {
        ChangeTool("None");
    }

    private void Update()
    {
        // 숫자 키로 도구 교체 (임시 시스템)
        if (Keyboard.current.digit1Key.wasPressedThisFrame) ChangeTool("Hoe");
        if (Keyboard.current.digit2Key.wasPressedThisFrame) ChangeTool("Seed");
        if (Keyboard.current.digit3Key.wasPressedThisFrame) ChangeTool("None");
    }

    private void PerformInteract()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        float interactRange = 2.0f;
        float distance = Vector2.Distance(transform.position, mouseWorldPos);

        // 상호작용 가능 거리 체크
        if (distance > interactRange)
        {
            Debug.Log("<color=red>거리가 너무 멉니다.</color>");
            return;
        }

        // --- 상황별 로직 분기 ---

        // 1. 도구가 선택된 상태 (개간, 씨앗 심기 등)
        if (_currentTool != "None" && !string.IsNullOrEmpty(_currentTool))
        {
            if (_currentTool == "Seed")
            {
                // TileManager의 수정된 메서드 호출: 위치, 도구명, 작물데이터 전달
                MasterManager.Tile.HandleInteraction(mouseWorldPos, _currentTool, _testCrop);
            }
            else
            {
                // 일반 도구(괭이 등) 호출
                MasterManager.Tile.HandleInteraction(mouseWorldPos, _currentTool);
            }
        }
        // 2. 맨손 상태 (수확 로직)
        else
        {
            TryHarvest(mouseWorldPos);
        }
    }

    /// <summary>
    /// 마우스 위치의 작물을 확인하고 수확을 시도합니다.
    /// </summary>
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
                    // 1. 인벤토리에 아이템 추가
                    MasterManager.Inventory.AddItem(harvestedItem, 1);

                    // 2. [핵심] 타일맵을 다시 개간된 상태로 변경
                    // 작물의 위치(hit.transform.position)를 넘겨줍니다.
                    MasterManager.Tile.ResetToTilled(hit.transform.position);

                    // 3. 작물 오브젝트 제거
                    Destroy(hit.collider.gameObject);

                    Debug.Log("수확 완료 및 땅 복구 성공!");
                }
            }
        }
    }

    private void ChangeTool(string toolName)
    {
        _currentTool = toolName;

        if (_visualizer != null)
        {
            _visualizer.UpdateVisual(toolName);
        }

        Debug.Log($"<color=orange>도구 교체: {toolName}</color>");
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