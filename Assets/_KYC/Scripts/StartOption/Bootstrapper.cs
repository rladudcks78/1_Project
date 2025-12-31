using UnityEngine;

/// <summary>
/// 게임의 시작점입니다. 
/// 모든 매니저의 생성 순서와 초기화를 중앙에서 제어하여 의존성 문제를 해결합니다.
/// </summary>
public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MasterManager _masterManagerPrefab;

    private void Awake()
    {
        // 씬에 마스터 매니저가 없다면 생성
        if (MasterManager.Instance == null)
        {
            Instantiate(_masterManagerPrefab);
        }

        // 여기서부터는 게임 시작 로직 (타이틀 화면 등)
    }
}