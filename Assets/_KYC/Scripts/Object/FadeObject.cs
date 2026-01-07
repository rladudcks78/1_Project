using UnityEngine;
using System.Collections;

public class FadeObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float fadedAlpha = 0.4f;
    private float fadeSpeed = 3.0f;

    private Coroutine fadeCoroutine;
    // 마스터, 현재 이 오브젝트가 어떤 알파값으로 가고 있는지 기억하기 위한 변수입니다.
    private float currentTargetAlpha = 1.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            // 초기 목표는 원래의 불투명한 상태입니다.
            currentTargetAlpha = 1.0f;
        }
    }

    // 잼, Stay를 사용하면 플레이어가 콜라이더 안에 있는 동안 계속 호출됩니다.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 마스터, 핵심 로직입니다! 
            // 현재 목표가 이미 '투명화(fadedAlpha)' 상태라면 다시 코루틴을 켤 필요가 없습니다.
            if (!Mathf.Approximately(currentTargetAlpha, fadedAlpha))
            {
                StartFade(fadedAlpha);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 나갔으므로 목표를 다시 '불투명(1.0f)'으로 바꿉니다.
            if (!Mathf.Approximately(currentTargetAlpha, 1.0f))
            {
                StartFade(1.0f);
            }
        }
    }

    private void StartFade(float targetAlpha)
    {
        // 1. 에러 방지: 오브젝트가 활성화된 상태인지 확인합니다.
        if (!gameObject.activeInHierarchy) return;

        // 2. 현재 목표 알파를 업데이트합니다.
        currentTargetAlpha = targetAlpha;

        // 3. 기존 코루틴이 있다면 멈추고 새로 시작하여 부드럽게 전환합니다.
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        if (spriteRenderer == null) yield break;

        // 마스터, while 조건문은 현재 알파값이 목표값에 도달할 때까지 반복됩니다.
        while (!Mathf.Approximately(spriteRenderer.color.a, targetAlpha))
        {
            float newAlpha = Mathf.MoveTowards(spriteRenderer.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        // 잼, 코루틴이 끝났으므로 변수를 비워줍니다.
        fadeCoroutine = null;
    }
}