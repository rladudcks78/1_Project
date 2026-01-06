using UnityEngine;

/// <summary>
/// 플레이어 데이터 및 게임 상태를 실시간으로 관리하는 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public PlayerData Player => _playerData;

    private Transform _playerTransform;
    public Transform PlayerTransform => _playerTransform;

    public void Init()
    {
        // 1. 씬에서 "Player" 태그를 가진 오브젝트를 찾아 Transform 할당
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
            Debug.Log("DataManager: 플레이어 트랜스폼 로드 완료.");
        }
        else
        {
            Debug.LogError("DataManager: 'Player' 태그를 가진 오브젝트를 씬에서 찾을 수 없습니다!");
        }

        Debug.Log("DataManager: 데이터 시스템 초기화 완료.");
    }

    // 포트폴리오 확장성: 돈 추가/차감 로직을 여기에 두어 중앙 집중식으로 관리합니다.
    public void AddGold(int amount)
    {
        _playerData.gold += amount;
        Debug.Log($"보유 금액 변경: {_playerData.gold}");
    }
}