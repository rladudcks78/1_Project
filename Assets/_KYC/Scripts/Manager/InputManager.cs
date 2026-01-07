using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMove;
    public static event Action OnInteract;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _interactAction;

    public void Init()
    {
        _playerInput = GetComponent<PlayerInput>();

        // 1. 액션 맵 로드
        var map = _playerInput.actions.FindActionMap("Player");

        if (map != null)
        {
            // 2. 액션 찾기
            _moveAction = map.FindAction("Move");
            _interactAction = map.FindAction("Interact");

            if (_interactAction == null)
            {
                Debug.LogError("<color=red>InputManager: 'Interact' 액션을 찾을 수 없습니다! Input Action Asset의 이름을 확인하세요.</color>");
            }

            // 3. 바인딩 (Init에서 직접 호출)
            BindActions();
            Debug.Log("<color=green>InputManager: 입력 시스템 초기화 및 바인딩 완료.</color>");
        }
        else
        {
            Debug.LogError("<color=red>InputManager: 'Player' 액션 맵을 찾을 수 없습니다!</color>");
        }
    }

    private void BindActions()
    {
        // 중복 바인딩 방지를 위해 먼저 해제 후 등록
        UnbindActions();

        if (_moveAction != null)
        {
            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled += OnMoveCanceled;
        }

        if (_interactAction != null)
        {
            _interactAction.performed += OnInteractPerformed;
        }
    }

    private void UnbindActions()
    {
        if (_moveAction != null)
        {
            _moveAction.performed -= OnMovePerformed;
            _moveAction.canceled -= OnMoveCanceled;
        }

        if (_interactAction != null)
        {
            _interactAction.performed -= OnInteractPerformed;
        }
    }

    private void OnDisable() => UnbindActions();

    // 콜백 함수들
    private void OnMovePerformed(InputAction.CallbackContext ctx) => OnMove?.Invoke(ctx.ReadValue<Vector2>());
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => OnMove?.Invoke(Vector2.zero);

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("<color=yellow>InputManager: 상호작용(마우스 클릭) 입력 감지!</color>");
        OnInteract?.Invoke();
    }
}