using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData statData;

    private Rigidbody2D _rb;
    private Vector2 _targetDirection;
    private Vector2 _lastFacingDirection = Vector2.down; // 기본값: 아래쪽 바라보기

    public Vector2 CurrentTargetDirection => _lastFacingDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // 초기 설정 유지...
    }

    private void OnEnable() => InputManager.OnMove += HandleMoveInput;
    private void OnDisable() => InputManager.OnMove -= HandleMoveInput;

    private void HandleMoveInput(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f)
        {
            _targetDirection = Vector2.zero;
            return;
        }

        // 4방향 이동 로직
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            _targetDirection = new Vector2(input.x > 0 ? 1 : -1, 0);
        else
            _targetDirection = new Vector2(0, input.y > 0 ? 1 : -1);

        // [중요] 이동 중일 때만 마지막 바라본 방향을 갱신합니다.
        _lastFacingDirection = _targetDirection;
    }

    // Interactor가 가져갈 방향 (이동 중엔 현재 방향, 멈추면 마지막 방향)
    public Vector2 GetFacingDirection()
    {
        return _lastFacingDirection;
    }

    private void FixedUpdate()
    {
        if (statData == null) return;
        Vector2 targetVelocity = _targetDirection * statData.moveSpeed;
        _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, targetVelocity, 20f * Time.fixedDeltaTime);
    }
}