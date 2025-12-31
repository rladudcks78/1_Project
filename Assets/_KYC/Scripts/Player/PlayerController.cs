using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private PlayerData statData;

    private Rigidbody2D _rb;
    private PlayerInputHandler _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInputHandler>();

        // 팩트 체크: 톱다운 2D에서 이 설정은 필수입니다. 
        // 인스펙터에서 수동으로 해도 되지만, 코드로 강제하는 것이 포트폴리오상 안전합니다.
        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (statData == null || _input == null) return;

        // InputHandler에서 이미 대각선이 필터링된 값을 가져옵니다.
        _rb.linearVelocity = _input.MovementInput.normalized * statData.moveSpeed;
    }
}