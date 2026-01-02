using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private PlayerController _controller;

    // 업로드하신 파일의 파라미터명과 동일하게 세팅
    private static readonly int HashUp = Animator.StringToHash("IsWalking_Up");
    private static readonly int HashDown = Animator.StringToHash("IsWalking_Down");
    private static readonly int HashSide = Animator.StringToHash("IsWalking_Side");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 물리 속도가 0.1보다 클 때만 걷는 중이라고 판단 (벽 뚫기 방지)
        bool isMoving = _rb.linearVelocity.sqrMagnitude > 0.01f;
        Vector2 dir = _controller.CurrentTargetDirection;

        // 파라미터 초기화
        _anim.SetBool(HashUp, false);
        _anim.SetBool(HashDown, false);
        _anim.SetBool(HashSide, false);

        if (isMoving)
        {
            if (dir.y > 0) _anim.SetBool(HashUp, true);
            else if (dir.y < 0) _anim.SetBool(HashDown, true);
            else if (dir.x != 0)
            {
                _anim.SetBool(HashSide, true);
                _sr.flipX = (dir.x > 0); 
            }
        }
    }
}