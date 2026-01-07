using UnityEngine;
using System.Collections;

/// <summary>
/// ���� ���� �׸����� �̵�, ���� ũ��, ���̵� ȿ���� �����մϴ�.
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
    /// �Ŵ����κ��� �ʱ� ������ �޾� Ȱ��ȭ�մϴ�.
    /// </summary>
    public void Initialize(float speed, Vector3 direction, float alpha, float randomScale)
    {
        moveSpeed = speed;
        moveDirection = direction.normalized;
        targetAlpha = alpha;

        // 2�� ��û����: ũ�� ���� ����
        transform.localScale = Vector3.one * randomScale;

        // 3�� ��û����: ���̵� �� ����
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0, targetAlpha, 2.0f));
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // ȭ�� ������ �ָ� ������ ��Ȱ��ȭ (Ǯ�� ��ȯ)
        if (transform.position.magnitude > 60f)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeRoutine(float start, float end, float duration)
    {
        float elapsed = 0;
        Color color = Color.black; // �׸��ڴ� ������ ����

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(start, end, elapsed / duration);
            color.a = currentAlpha;
            if (spriteRenderer != null) spriteRenderer.color = color;
            yield return null;
        }
    }

    // ��Ȱ��ȭ�� ��(ȭ�� ��) �ε巴�� ������� �ϰ� �ʹٸ� ������ ������ �ʿ�������, 
    // ������ ���� ȭ�� ������ ���� ��(In)�� ���̵带 �ִ� ���� ȿ�����Դϴ�.
}