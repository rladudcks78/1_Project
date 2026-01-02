using UnityEngine;

/// <summary>
/// 플레이어 데이터 및 게임 상태를 실시간으로 관리하는 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public PlayerData Player => _playerData;

    public void Init()
    {
        Debug.Log("DataManager: 데이터 시스템 초기화 완료.");
        // 필요 시 여기서 PlayerData의 초기화(Reset)나 세이브 파일 로드를 진행합니다.
    }

    // 포트폴리오 확장성: 돈 추가/차감 로직을 여기에 두어 중앙 집중식으로 관리합니다.
    public void AddGold(int amount)
    {
        _playerData.gold += amount;
        Debug.Log($"보유 금액 변경: {_playerData.gold}");
    }
}