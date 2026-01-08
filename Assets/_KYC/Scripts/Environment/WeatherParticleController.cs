using UnityEngine;

/// <summary>
/// 환경 컨트롤러와 연동되어 실제 비 파티클의 방출량을 조절합니다.
/// </summary>
public class WeatherParticleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem rainParticle; // 비 파티클 시스템

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 2.0f; // 비가 서서히 오거나 그치는 시간

    // 비를 켜고 끄는 로직 (EnvironmentController.ChangeWeather 호출 시 함께 사용 권장)
    public void SetRain(bool isRainy)
    {
        if (isRainy) rainParticle.Play();
        else rainParticle.Stop();

        // 포트폴리오용 팁: Emission Rate를 Lerp로 조절하면 훨씬 자연스럽습니다.
    }
}