using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 대각선 이동 신호를 물리적으로 원천 차단하는 입력 핸들러
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _rawInput;
    public Vector2 MovementInput { get; private set; }
    public bool IsMoving => MovementInput.sqrMagnitude > 0.01f;

    public void OnMove(InputValue value)
    {
        _rawInput = value.Get<Vector2>();

        // [팩트 체크] 입력 즉시 축 고정
        // x와 y중 절대값이 더 큰 쪽만 살리고 나머지는 완전히 0으로 소멸시킵니다.
        if (Mathf.Abs(_rawInput.x) > Mathf.Abs(_rawInput.y))
        {
            MovementInput = new Vector2(_rawInput.x > 0 ? 1 : -1, 0);
        }
        else if (Mathf.Abs(_rawInput.y) > Mathf.Abs(_rawInput.x))
        {
            MovementInput = new Vector2(0, _rawInput.y > 0 ? 1 : -1);
        }
        else
        {
            // 값이 완전히 같거나 입력이 없을 때
            MovementInput = Vector2.zero;
        }
    }
}