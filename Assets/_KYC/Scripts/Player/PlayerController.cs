using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private PlayerData statData;

    [Header("Movement Settings")]
    [Range(0f, 100f)][SerializeField] private float _acceleration = 20f; // 가속도 (높을수록 빠릿함)
    [Range(0f, 100f)][SerializeField] private float _deceleration = 25f; // 감속도 (멈출 때 속도)

    private Rigidbody2D _rb;
    private PlayerInputHandler _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInputHandler>();

        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 팩트 체크: 물리 연산이 프레임에 독립적이도록 설정
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void FixedUpdate()
    {
        if (statData == null || _input == null) return;

        // 1. 목표 속도 결정
        Vector2 targetVelocity = _input.MovementInput * statData.moveSpeed;

        // 2. 현재 상태에 따라 가속/감속 비율 선택
        float lerpSpeed = _input.IsMoving ? _acceleration : _deceleration;

        // 3. Lerp를 이용한 선형 보간 이동
        _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, targetVelocity, lerpSpeed * Time.fixedDeltaTime);

        // [핵심] 4. 속도가 충분히 낮아지면 강제로 0으로 고정 (Snap to Zero)
        // Lerp는 점근선이기 때문에 이 처리가 없으면 애니메이션이 안 멈출 수 있습니다.
        if (!_input.IsMoving && _rb.linearVelocity.sqrMagnitude < 0.01f)
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }
}