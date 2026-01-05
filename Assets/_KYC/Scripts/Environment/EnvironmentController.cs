using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

/// <summary>
/// 시간과 날씨에 따라 전역 광원의 색상과 강도를 제어합니다.
/// </summary>
[RequireComponent(typeof(Light2D))]
public class EnvironmentColorController : MonoBehaviour
{
    [System.Serializable]
    public struct WeatherColorPreset
    {
        public string weatherName;
        public Gradient dayCycleGradient;
        public AnimationCurve intensityCurve; // 시간에 따른 밝기 변화
    }

    [Header("Weather Presets")]
    public WeatherColorPreset clearSky;  // 맑은 날
    public WeatherColorPreset rainyDay;  // 비 오는 날

    [Header("Transition Settings")]
    public float weatherTransitionDuration = 2.0f;

    private Light2D globalLight;
    private WeatherColorPreset currentPreset;
    private WeatherColorPreset targetPreset;
    private float transitionProgress = 1f;

    void Awake()
    {
        globalLight = GetComponent<Light2D>();
        currentPreset = clearSky; // 기본값은 맑은 날
    }

    void Update()
    {
        if (MasterManager.Day == null) return;

        float time = MasterManager.Day.GetTimeNormalized();

        // 현재 프리셋과 타겟 프리셋 사이의 값을 보간(Lerp)하여 최종 색상과 강도 결정
        Color targetColor = currentPreset.dayCycleGradient.Evaluate(time);
        float targetIntensity = currentPreset.intensityCurve.Evaluate(time);

        // 날씨 전환 중일 때 처리
        if (transitionProgress < 1f)
        {
            // 이 부분은 나중에 날씨 시스템 연동 시 활성화 (현재는 단일 프리셋 우선 적용)
        }

        globalLight.color = targetColor;
        globalLight.intensity = targetIntensity;
    }

    /// <summary>
    /// 외부(DayManager 등)에서 날씨를 변경할 때 호출합니다.
    /// </summary>
    public void ChangeWeather(string weatherName)
    {
        if (weatherName == "Rain") currentPreset = rainyDay;
        else currentPreset = clearSky;

        Debug.Log($"날씨 광원 변경: {weatherName}");
    }
}