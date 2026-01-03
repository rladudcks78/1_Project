using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 구름 프리팹 풀링 및 랜덤 스폰 범위를 관리합니다.
/// </summary>
public class CloudShadowManager : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    [SerializeField] private GameObject[] shadowPrefabs;

    [Header("Spawn Area (Gizmos)")]
    public float spawnX = -25f;
    public float minY = -10f;
    public float maxY = 20f;

    [Header("Random Options")]
    public float spawnInterval = 4f;
    public float minSpeed = 0.7f;
    public float maxSpeed = 1.8f;
    public float minScale = 1.5f; // 구름 그림자는 보통 커야 자연스럽습니다.
    public float maxScale = 4.0f;
    [Range(0f, 1f)] public float shadowAlpha = 0.3f;

    private List<CloudShadow> shadowPool = new List<CloudShadow>();
    private float timer;

    void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (var prefab in shadowPrefabs)
        {
            for (int i = 0; i < 3; i++) // 종류별로 3개씩 풀링
            {
                GameObject obj = Instantiate(prefab, transform);
                CloudShadow shadow = obj.getOrAddComponent<CloudShadow>();
                obj.SetActive(false);
                shadowPool.Add(shadow);
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCloud();
            timer = 0f;
        }
    }

    private void SpawnCloud()
    {
        // 1. 현재 풀에서 비활성화된(사용 가능한) 모든 구름들을 리스트로 가져옵니다.
        List<CloudShadow> availableShadows = shadowPool.FindAll(s => !s.gameObject.activeInHierarchy);

        // 2. 사용 가능한 구름이 있다면 무작위로 하나를 선택합니다.
        if (availableShadows.Count > 0)
        {
            // 랜덤 인덱스 추출 (순서대로가 아닌 진짜 랜덤)
            int randomIndex = Random.Range(0, availableShadows.Count);
            CloudShadow shadow = availableShadows[randomIndex];

            // 3. 위치 랜덤 설정 (기존 로직 유지)
            float randomY = Random.Range(minY, maxY);
            shadow.transform.position = new Vector3(spawnX, randomY, 0);

            // 4. 속도 및 크기 랜덤 설정
            float speed = Random.Range(minSpeed, maxSpeed);
            float scale = Random.Range(minScale, maxScale);

            // 5. 활성화 및 초기화
            shadow.gameObject.SetActive(true);

            // 바람 방향을 살짝씩 틀어주면 더 자연스럽습니다 (Vector3.right에 랜덤 y값 추가)
            Vector3 randomDirection = new Vector3(1f, Random.Range(-0.2f, 0.05f), 0f);
            shadow.Initialize(speed, randomDirection, shadowAlpha, scale);
        }
        else
        {
            // 만약 모든 구름이 사용 중이라면 풀 사이즈를 늘려야 할 수도 있다는 로그 (디버깅용)
            Debug.LogWarning("모든 구름 프리팹이 화면에 표시 중입니다. Pool Size를 늘리는 것을 고려하세요.");
        }
    }

    // 에디터 가시성을 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(spawnX, minY, 0), new Vector3(spawnX, maxY, 0));
    }
}

// 확장 메서드: 컴포넌트가 없으면 붙여주는 기능 (유지보수용)
public static class GameObjectExtensions
{
    public static T getOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        return component != null ? component : obj.AddComponent<T>();
    }
}