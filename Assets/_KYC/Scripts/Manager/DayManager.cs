using UnityEngine;
using System;

/// <summary>
/// 게임의 시간을 관리하고 아침/낮/저녁/밤 상태를 결정합니다.
/// </summary>
public class DayManager : MonoBehaviour
{
    public enum DayPhase { Morning, Day, Evening, Night }

    [Header("Time Settings")]
    [Tooltip("실제 시간으로 하루가 몇 초인지 설정합니다.")]
    public float dayDurationInSeconds = 120f;

    [Range(0, 1)]
    public float currentTimeNormalized = 0.25f; // 0.25(6시 아침), 0.5(12시 낮) ...

    public DayPhase currentPhase;

    // 가로등, 구름 등 다른 시스템이 구독할 이벤트
    public event Action<DayPhase> OnPhaseChanged;

    public void Init()
    {
        Debug.Log("DayManager 초기화 완료");
        UpdatePhase();
    }

    private void Update()
    {
        // 시간 흐름 계산
        currentTimeNormalized += Time.deltaTime / dayDurationInSeconds;
        if (currentTimeNormalized >= 1f) currentTimeNormalized = 0f;

        UpdatePhase();
    }

    private void UpdatePhase()
    {
        DayPhase previousPhase = currentPhase;

        // 시간에 따른 상태 구분 (기획에 따라 범위 조절 가능)
        if (currentTimeNormalized >= 0.2f && currentTimeNormalized < 0.4f) currentPhase = DayPhase.Morning;
        else if (currentTimeNormalized >= 0.4f && currentTimeNormalized < 0.7f) currentPhase = DayPhase.Day;
        else if (currentTimeNormalized >= 0.7f && currentTimeNormalized < 0.9f) currentPhase = DayPhase.Evening;
        else currentPhase = DayPhase.Night;

        // 상태가 변했을 때만 이벤트 실행
        if (previousPhase != currentPhase)
        {
            OnPhaseChanged?.Invoke(currentPhase);
            Debug.Log($"시간대 변경: {currentPhase}");
        }
    }

    // 0~1 사이의 현재 시간 비율 반환 (광원 조절용)
    public float GetTimeNormalized() => currentTimeNormalized;
}