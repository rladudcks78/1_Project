using UnityEngine;
using System.Collections;

/// <summary>
/// 개별 구름 그림자의 이동, 랜덤 크기, 페이드 효과를 관리합니다.
/// </summary>
public class CloudShadow : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;
    private SpriteRenderer spriteRenderer;
    private float targetAlpha;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 매니저로부터 초기 설정을 받아 활성화합니다.
    /// </summary>
    public void Initialize(float speed, Vector3 direction, float alpha, float randomScale)
    {
        moveSpeed = speed;
        moveDirection = direction.normalized;
        targetAlpha = alpha;

        // 2번 요청사항: 크기 랜덤 적용
        transform.localScale = Vector3.one * randomScale;

        // 3번 요청사항: 페이드 인 시작
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0, targetAlpha, 2.0f));
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 화면 밖으로 멀리 나가면 비활성화 (풀링 반환)
        if (transform.position.magnitude > 60f)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeRoutine(float start, float end, float duration)
    {
        float elapsed = 0;
        Color color = Color.black; // 그림자는 검은색 고정

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(start, end, elapsed / duration);
            color.a = currentAlpha;
            if (spriteRenderer != null) spriteRenderer.color = color;
            yield return null;
        }
    }

    // 비활성화될 때(화면 밖) 부드럽게 사라지게 하고 싶다면 별도의 로직이 필요하지만, 
    // 성능을 위해 화면 안으로 들어올 때(In)만 페이드를 주는 것이 효율적입니다.
}