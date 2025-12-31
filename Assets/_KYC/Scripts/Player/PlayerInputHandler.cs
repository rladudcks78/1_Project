using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _rawInput;
    public Vector2 MovementInput { get; private set; }
    public bool IsMoving => MovementInput.sqrMagnitude > 0.01f;

    public void OnMove(InputValue value)
    {
        _rawInput = value.Get<Vector2>();

        // [팩트 체크] 너무 작은 입력은 무시 (데드존 처리)
        if (_rawInput.sqrMagnitude < 0.01f)
        {
            MovementInput = Vector2.zero;
            return;
        }

        // 입력의 x, y 비중을 비교하되, 부드러운 전환을 위해 원본 값을 약간 유지하거나
        // 방향 전환 시의 '입력 튐'을 방지하기 위해 정규화된 축만 선택합니다.
        if (Mathf.Abs(_rawInput.x) > Mathf.Abs(_rawInput.y))
        {
            // x축 이동이 우세할 때
            MovementInput = new Vector2(_rawInput.x > 0 ? 1 : -1, 0);
        }
        else
        {
            // y축 이동이 우세할 때
            MovementInput = new Vector2(0, _rawInput.y > 0 ? 1 : -1);
        }
    }
}