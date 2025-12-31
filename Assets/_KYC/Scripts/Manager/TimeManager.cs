using System.Collections;
using UnityEngine;
using System; // Action(이벤트) 사용을 위해 필요

/// <summary>
/// 게임의 시간을 관리하는 매니저입니다.
/// 시간의 흐름에 따라 이벤트를 발생시켜 다른 시스템(날씨, 작물 성장)과 통신합니다.
/// </summary>
public class TimeManager : MonoBehaviour
{
    [Header("시간 설정")]
    [SerializeField] private float _timeScale = 1.0f; // 현실 시간 대비 게임 시간 속도

    private int _hour = 6;   // 시작 시간 (오전 6시)
    private int _minute = 0;
    private int _day = 1;

    // 프로퍼티를 통해 외부에서 시간 정보를 읽기만 가능하게 제한 (캡슐화)
    public int Hour => _hour;
    public int Minute => _minute;
    public int Day => _day;

    // 다른 클래스들이 구독할 수 있는 이벤트 (유지보수성: 매니저 간 의존성 제거)
    public event Action OnMinuteChanged;
    public event Action OnHourChanged;
    public event Action OnDayChanged;

    private bool _isPaused = true;

    /// <summary>
    /// MasterManager에서 호출할 초기화 함수입니다.
    /// </summary>
    public void Initialize()
    {
        Debug.Log("TimeManager 초기화 중...");
        _isPaused = false;

        // 시간 흐름 코루틴 시작
        StartCoroutine(UpdateTimeRoutine());
    }

    /// <summary>
    /// 실제 시간이 흐르는 로직입니다.
    /// </summary>
    private IEnumerator UpdateTimeRoutine()
    {
        // 20년 차 팁: WaitForSeconds를 캐싱하거나 상단에 선언하여 가비지 컬렉션을 방지합니다.
        var wait = new WaitForSeconds(0.1f * _timeScale);

        while (true)
        {
            if (!_isPaused)
            {
                _minute++;
                if (_minute >= 60)
                {
                    _minute = 0;
                    _hour++;
                    OnHourChanged?.Invoke(); // 정각 알림

                    if (_hour >= 24)
                    {
                        _hour = 0;
                        _day++;
                        OnDayChanged?.Invoke(); // 다음 날 알림
                    }
                }
                OnMinuteChanged?.Invoke(); // 분 단위 알림
            }
            yield return wait;
        }
    }

    public void PauseTime(bool pause) => _isPaused = pause;
}