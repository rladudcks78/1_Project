using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 시간과 날씨에 따라 전역 광원의 색상과 강도를 제어하며,
/// 밤 시간대 채도 저하(Hue Shift) 기능을 포함합니다.
/// </summary>
[RequireComponent(typeof(Light2D))]
public class EnvironmentController : MonoBehaviour
{
    [System.Serializable]
    public struct WeatherColorPreset
    {
        public string weatherName;
        public Gradient dayCycleGradient;
        public AnimationCurve intensityCurve;
    }

    [Header("Weather Presets")]
    public WeatherColorPreset clearSky;  // 맑은 날
    public WeatherColorPreset rainyDay;  // 비 오는 날

    private Light2D globalLight;
    private WeatherColorPreset currentPreset; // 여기서 선언된 이름을 Update에서 사용합니다.

    void Awake()
    {
        globalLight = GetComponent<Light2D>();
        currentPreset = clearSky; // 기본값 세팅
    }

    void Update()
    {
        if (MasterManager.Day == null) return;

        float time = MasterManager.Day.GetTimeNormalized();

        // 1. 현재 프리셋에서 시간대별 색상과 강도를 가져옴
        Color targetColor = currentPreset.dayCycleGradient.Evaluate(time);
        float targetIntensity = currentPreset.intensityCurve.Evaluate(time);

        // 2. [색도 변화 추가: 밤에는 채도를 낮춤]
        float h, s, v;
        Color.RGBToHSV(targetColor, out h, out s, out v);

        // Intensity가 낮을수록(밤일수록) 채도를 최대 40%까지 낮추는 수식
        // targetIntensity가 0.2(밤)이면 saturationModifier는 약 0.6이 됩니다.
        float saturationModifier = Mathf.Lerp(0.6f, 1.0f, targetIntensity);
        targetColor = Color.HSVToRGB(h, s * saturationModifier, v);

        // 3. 최종 값 적용
        globalLight.color = targetColor;
        globalLight.intensity = targetIntensity;
    }

    // 외부(예: WeatherManager)에서 날씨를 바꿀 때 호출할 메서드
    public void ChangeWeather(bool isRainy)
    {
        currentPreset = isRainy ? rainyDay : clearSky;
    }
}