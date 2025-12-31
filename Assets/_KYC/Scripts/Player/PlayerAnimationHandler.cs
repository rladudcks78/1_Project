using UnityEngine;

/// <summary>
/// New Input System의 데이터를 받아 애니메이션 Bool 파라미터를 제어합니다.
/// </summary>
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;
    private PlayerInputHandler _input; // 입력 전담 스크립트 참조

    // 파라미터 해시 캐싱 (성능 최적화)
    private static readonly int HashMoveUp = Animator.StringToHash("IsWalking_Up");
    private static readonly int HashMoveSide = Animator.StringToHash("IsWalking_Side");
    private static readonly int HashMoveDown = Animator.StringToHash("IsWalking_Down");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        HandleMovementAnim();
    }

    private void HandleMovementAnim()
    {
        if (_input == null) return;

        // 1. 모든 상태 초기화
        bool isUp = false;
        bool isDown = false;
        bool isSide = false;

        // 2. Input System의 벡터값을 가져옴
        Vector2 moveDir = _input.MovementInput;

        // 3. 우선순위 결정 (상 -> 하 -> 좌 -> 우)
        if (moveDir.y > 0) // 위
        {
            isUp = true;
            // 위를 볼 때 flipX는 이전 상태를 유지하거나 기본값(false) 처리
        }
        else if (moveDir.y < 0) // 아래
        {
            isDown = true;
        }
        else if (moveDir.x < 0) // 왼쪽
        {
            isSide = true;
            _sr.flipX = false; // 리소스가 오른쪽 기준이라면 true, 왼쪽 기준이라면 false (조절 필요)
        }
        else if (moveDir.x > 0) // 오른쪽
        {
            isSide = true;
            _sr.flipX = true;
        }

        // 4. 애니메이터에 전달
        _anim.SetBool(HashMoveUp , isUp);
        _anim.SetBool(HashMoveDown, isDown);
        _anim.SetBool(HashMoveSide, isSide);
    }
}