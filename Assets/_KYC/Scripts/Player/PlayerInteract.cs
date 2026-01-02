using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerController _controller;
    private string _currentTool = "None"; // 현재 선택된 도구 (나중에 숫자로 변경 가능)

    [Header("Reference")]
    [SerializeField] private ToolVisualizer _visualizer;

    private void Awake() => _controller = GetComponent<PlayerController>();

    private void OnEnable() => InputManager.OnInteract += PerformInteract;
    private void OnDisable() => InputManager.OnInteract -= PerformInteract;

    private void Start()
    {
        // 2. 시작 시점에 비주얼을 초기화 (맨손 상태로 시작)
        ChangeTool("None");
    }

    private void Update()
    {
        // 도구 선택 키 (임시)
        if (Keyboard.current.digit1Key.isPressed)
        {
            ChangeTool("Hoe");
            Debug.Log("괭이");
        }

        if (Keyboard.current.digit2Key.isPressed)
        {
            ChangeTool("Seed");
            Debug.Log("씨앗");
        }
        if (Keyboard.current.digit3Key.isPressed)
        {
            ChangeTool("None");
            Debug.Log("빈손");
        }
    }

    private void PerformInteract()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        float interactRange = 2.0f;
        float distance = Vector2.Distance(transform.position, mouseWorldPos);

        if (distance <= interactRange)
        {
            // 도구가 없을 때 (None)
            if (_currentTool == "None" || string.IsNullOrEmpty(_currentTool))
            {
                // 맨손일 때는 "터치"만 가능 (나중에 NPC 대화나 수확 등에 사용)
                Debug.Log($"<color=white>맨손 터치: {mouseWorldPos} 지점</color>");
                // MasterManager.Tile.HandleInteraction은 호출하지 않음
                return;
            }

            // 도구가 있을 때만 타일 매니저 호출
            MasterManager.Tile.HandleInteraction(mouseWorldPos, _currentTool);
        }
        else
        {
            Debug.Log("거리가 너무 멉니다");
        }
    }

    // 도구 변경 메서드 (나중에 인벤토리에서 호출)
    private void ChangeTool(string toolName)
    {
        _currentTool = toolName;

        // 시각화 스크립트에게 명령 전달
        if (_visualizer != null)
        {
            _visualizer.UpdateVisual(toolName);
        }

        Debug.Log($"<color=orange>도구 교체: {toolName}</color>");
    }

    private void OnDrawGizmos()
    {
        // 마우스 위치 실시간 확인용 (에디터 실행 중일 때만)
        if (!Application.isPlaying) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Gizmos.color = Vector2.Distance(transform.position, mousePos) <= 2.0f ? Color.green : Color.red;
        Gizmos.DrawWireSphere(mousePos, 0.2f);
    }
}