using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어의 이동과 기초 입력을 관리하는 클래스
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5.0f;

    // 상태 정의 (클래스 외부에서도 접근 가능하도록 public)
    public enum PlayerState { Idle, Moving, Acting }
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    private Vector2 _inputVec;
    private Vector2 _lastDir = Vector2.down;

    // 캐싱 변수
    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sr;
    private PlayerInteraction _interaction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _interaction = GetComponent<PlayerInteraction>();
    }

    private void FixedUpdate()
    {
        // 행동(Acting) 중일 때는 물리 이동을 차단하여 조작감을 높임
        if (CurrentState == PlayerState.Acting)
        {
            _rb.linearVelocity = Vector2.zero; // 유니티 6에서는 velocity 대신 linearVelocity 권장
            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 nextVec = _inputVec * (speed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + nextVec);
    }

    // Input System Message: 이동 입력
    private void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();

        if (_inputVec != Vector2.zero)
        {
            _lastDir = _inputVec.normalized;
            CurrentState = PlayerState.Moving;
        }
        else
        {
            CurrentState = PlayerState.Idle;
        }

        UpdateAnimation();
    }

    // Input System Message: 액션 입력 (F키 혹은 클릭)
    private void OnAction(InputValue value)
    {
        // 이미 행동 중이거나 입력이 떼어질 때는 무시
        if (CurrentState == PlayerState.Acting || !value.isPressed) return;

        // 실제 액션 로직은 Interaction 클래스에 위임
        if (_interaction != null)
        {
            _interaction.ExecuteAction(_lastDir);
        }
    }

    private void UpdateAnimation()
    {
        _anim.SetFloat("Speed", _inputVec.magnitude);

        if (_inputVec != Vector2.zero)
        {
            _anim.SetFloat("xDir", _inputVec.x);
            _anim.SetFloat("yDir", _inputVec.y);
            _sr.flipX = _inputVec.x < 0;
        }
    }

    // 상태 변경을 위한 외부 접근 메서드 (Interaction에서 호출)
    public void SetState(PlayerState newState) => CurrentState = newState;

    // 플레이어가 바라보는 방향 반환
    public Vector2 GetFacingDirection() => _lastDir;
}