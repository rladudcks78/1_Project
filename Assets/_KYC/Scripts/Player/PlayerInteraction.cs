using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이어의 상호작용 및 도구 사용 로직을 전담하는 클래스
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    private PlayerMove _playerMove;
    private Animator _anim;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _anim = GetComponent<Animator>();
    }

    public void ExecuteAction(Vector2 facingDir)
    {
        StartCoroutine(ActionRoutine(facingDir));
    }

    private IEnumerator ActionRoutine(Vector2 facingDir)
    {
        // 1. 상태를 Acting으로 변경하여 이동 차단
        _playerMove.SetState(PlayerMove.PlayerState.Acting);

        // 2. 애니메이션 실행
        _anim.SetTrigger("doAction");

        // 3. 상호작용 지점 계산 (플레이어 위치 + 바라보는 방향 * 오프셋)
        Vector2 interactPos = (Vector2)transform.position + facingDir * 1.0f;

        Debug.Log($"{interactPos} 위치에서 상호작용 시도!");
        // TODO: 여기에 Tilemap 매니저를 호출해서 땅을 갈거나 물을 주는 로직 추가 가능

        // 4. 애니메이션 종료 대기 (가장 깔끔한 유지보수 방식은 Animation Event지만, 우선 코루틴 유지)
        yield return new WaitForSeconds(0.4f);

        // 5. 상태를 Idle로 복구
        _playerMove.SetState(PlayerMove.PlayerState.Idle);
    }
}