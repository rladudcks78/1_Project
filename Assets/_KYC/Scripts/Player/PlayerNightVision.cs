using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 밤이 되면 플레이어 주변 가시성을 확보하기 위해 전등을 켭니다.
/// </summary>
public class PlayerNightVision : MonoBehaviour
{
    private Light2D playerLight;
    
    [Header("Settings")]
    [SerializeField] private float maxIntensity = 1.0f; // 밤에 낼 최대 밝기
    [SerializeField] private float outerRadius = 5.0f;  // 불빛이 퍼지는 범위

    void Awake()
    {
        playerLight = GetComponent<Light2D>();
        playerLight.pointLightOuterRadius = outerRadius;
    }

    void Update()
    {
        if (MasterManager.Day == null) return;

        float time = MasterManager.Day.GetTimeNormalized();

        // [리얼리티 로직] 
        // 낮(0.5)에는 0, 밤(0.0 or 1.0)에는 maxIntensity가 되도록 계산
        // Mathf.Abs(time - 0.5f) * 2를 하면 낮엔 0, 밤엔 1이 나옵니다.
        float nightWeight = Mathf.Clamp01(Mathf.Abs(time - 0.5f) * 2.5f - 0.5f);
        playerLight.intensity = nightWeight * maxIntensity;
    }
}