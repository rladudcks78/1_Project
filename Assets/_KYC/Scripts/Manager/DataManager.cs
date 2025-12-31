using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData; // 인스펙터에서 위에서 만든 SO 파일 할당

    // 외부에서 데이터를 읽을 때는 이 프로퍼티를 사용 (캡슐화)
    public PlayerData Player => _playerData;

    public void Initialize()
    {
        // 게임 시작 시 필요한 데이터 로드 로직
        if (_playerData == null)
        {
            Debug.LogError("PlayerData SO가 할당되지 않았습니다!");
        }
        Debug.Log("DataManager 초기화 완료.");
    }

    // 예: 돈을 추가하는 공용 메서드
    public void AddGold(int amount)
    {
        _playerData.gold += amount;
        // 여기서 UI 업데이트 이벤트를 발생시키면 완벽합니다.
    }
}