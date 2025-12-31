using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;
    private PlayerInputHandler _input;
    private Rigidbody2D _rb; // 물리 속도 체크를 위해 추가

    private static readonly int HashMoveUp = Animator.StringToHash("IsWalking_Up");
    private static readonly int HashMoveSide = Animator.StringToHash("IsWalking_Side");
    private static readonly int HashMoveDown = Animator.StringToHash("IsWalking_Down");
    private static readonly int HashIsMoving = Animator.StringToHash("IsMoving");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _input = GetComponent<PlayerInputHandler>();
        _rb = GetComponent<Rigidbody2D>(); // 리지드바디 캐싱
    }

    private void Update()
    {
        HandleMovementAnim();
    }

    private void HandleMovementAnim()
    {
        if (_input == null || _rb == null) return;

        // [팩트 체크] 입력이 있거나, 실제 속도가 유의미하게 존재할 때만 움직임으로 간주
        // Threshold를 0.05f 정도로 넉넉히 잡아 Idle 전환을 확실하게 유도합니다.
        bool isMoving = _input.IsMoving || _rb.linearVelocity.sqrMagnitude > 0.05f;

        _anim.SetBool(HashIsMoving, isMoving);

        // 방향 전환은 오직 '입력'이 있을 때만 갱신 (서 있을 때 고개 돌리기 방지)
        if (_input.IsMoving)
        {
            Vector2 moveDir = _input.MovementInput;

            _anim.SetBool(HashMoveUp, false);
            _anim.SetBool(HashMoveDown, false);
            _anim.SetBool(HashMoveSide, false);

            if (moveDir.y > 0) _anim.SetBool(HashMoveUp, true);
            else if (moveDir.y < 0) _anim.SetBool(HashMoveDown, true);
            else if (moveDir.x != 0)
            {
                _anim.SetBool(HashMoveSide, true);
                _sr.flipX = (moveDir.x > 0);
            }
        }
    }
}