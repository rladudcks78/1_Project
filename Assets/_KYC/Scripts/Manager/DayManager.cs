using UnityEngine;
using System;

public class DayManager : MonoBehaviour
{
    public enum DayPhase { Night, Morning, Day, Evening }

    [Header("Time Settings")]
    [Tooltip("하루가 실제 시간으로 몇 초 동안 흐를지 설정합니다.")]
    public float dayDurationInSeconds = 120f;

    [Range(0, 1)]
    [SerializeField] private float currentTimeNormalized = 0.25f; // 0.25가 오전 6시 가정

    public DayPhase currentPhase;
    public event Action<DayPhase> OnPhaseChanged;

    public void Init()
    {
        Debug.Log("<color=green>DayManager: 초기화 완료</color>");
    }

    private void Update()
    {
        // 시간 흐름 (유지보수를 위해 Time.deltaTime 사용)
        currentTimeNormalized += Time.deltaTime / dayDurationInSeconds;
        if (currentTimeNormalized >= 1f) currentTimeNormalized = 0f;

        UpdatePhase();
    }

    private void UpdatePhase()
    {
        DayPhase previousPhase = currentPhase;

        // 리얼한 시간대 분할 (0:자정, 0.25:새벽/아침, 0.5:낮, 0.75:저녁)
        if (currentTimeNormalized >= 0.2f && currentTimeNormalized < 0.45f) currentPhase = DayPhase.Morning;
        else if (currentTimeNormalized >= 0.45f && currentTimeNormalized < 0.7f) currentPhase = DayPhase.Day;
        if (currentTimeNormalized >= 0.7f && currentTimeNormalized < 0.85f) currentPhase = DayPhase.Evening;
        else if (currentTimeNormalized >= 0.85f || currentTimeNormalized < 0.2f) currentPhase = DayPhase.Night;
        else currentPhase = DayPhase.Night;

        if (previousPhase != currentPhase)
        {
            OnPhaseChanged?.Invoke(currentPhase);
        }
    }

    public float GetTimeNormalized() => currentTimeNormalized;
}