using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 구름 그림자를 더 리얼하게 생성하기 위해 영역 기반 랜덤 스폰을 관리합니다.
/// 포트폴리오용으로 '자연스러운 불규칙성'을 구현하는 데 초점을 맞췄습니다.
/// </summary>
public class CloudShadowManager : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    [SerializeField] private GameObject[] shadowPrefabs;

    [Header("Realism Spawn Settings")]
    // 1. 점이 아닌 영역(Area) 기반 스폰으로 변경
    public float spawnX = -30f;
    public float minY = -15f;
    public float maxY = 25f;

    [Header("Random Options")]
    public float spawnInterval = 4f;
    [Range(0f, 2f)] public float timeJitter = 1.5f; // 생성 주기에 무작위성 부여
    public float minSpeed = 0.5f;
    public float maxSpeed = 1.5f;
    public float minScale = 2.0f;
    public float maxScale = 5.0f;
    [Range(0f, 1f)] public float shadowAlpha = 0.25f;

    private List<CloudShadow> shadowPool = new List<CloudShadow>();
    private float nextSpawnTime;

    void Start()
    {
        InitializePool();
        SetNextSpawnTime();
    }

    private void InitializePool()
    {
        if (shadowPrefabs == null || shadowPrefabs.Length == 0) return;

        foreach (var prefab in shadowPrefabs)
        {
            for (int i = 0; i < 5; i++) // 초기 풀 사이즈를 넉넉하게 확장 
            {
                CreateNewCloudInstance(prefab);
            }
        }

        // 초기 풀 셔플로 다양성 확보
        shadowPool = shadowPool.OrderBy(x => Random.value).ToList();
    }

    private void CreateNewCloudInstance(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);
        CloudShadow shadow = obj.getOrAddComponent<CloudShadow>();
        obj.SetActive(false);
        shadowPool.Add(shadow);
    }

    void Update()
    {
        // 2. 일정한 타이머 대신 '다음 생성 시점'을 계산하여 불규칙성 부여
        if (Time.time >= nextSpawnTime)
        {
            SpawnCloud();
            SetNextSpawnTime();
        }
    }

    private void SetNextSpawnTime()
    {
        // 기본 간격에 +- jitter를 더해 리얼한 생성 주기 형성
        float randomDelay = Random.Range(-timeJitter, timeJitter);
        nextSpawnTime = Time.time + spawnInterval + randomDelay;
    }

    private void SpawnCloud()
    {
        var availableShadow = shadowPool.FirstOrDefault(s => !s.gameObject.activeInHierarchy);

        if (availableShadow == null)
        {
            CreateNewCloudInstance(shadowPrefabs[Random.Range(0, shadowPrefabs.Length)]);
            availableShadow = shadowPool.Last();
        }

        // 3. 리얼한 위치 선정: Y축 범위 내에서 완전 랜덤 + 약간의 X축 오프셋
        float randomY = Random.Range(minY, maxY);
        float randomXOffset = Random.Range(-2f, 2f); // 시작점도 살짝 다르게
        availableShadow.transform.position = new Vector3(spawnX + randomXOffset, randomY, 0);

        // 4. 리얼한 외형 설정
        float speed = Random.Range(minSpeed, maxSpeed);
        float scale = Random.Range(minScale, maxScale);

        // Sorting Order를 랜덤하게 주어 그림자끼리 겹칠 때 입체감 부여 (필요 시 SpriteRenderer 접근)
        //var sr = availableShadow.GetComponentInChildren<SpriteRenderer>();
        //if (sr != null) sr.sortingOrder = Random.Range(4, 5); // 배경보다는 위, 오브젝트보다는 아래 

        availableShadow.gameObject.SetActive(true);

        // 바람 방향에 약간의 '기류' 랜덤성 추가
        Vector3 windDirection = new Vector3(1f, Random.Range(-0.08f, 0.08f), 0f);
        availableShadow.Initialize(speed, windDirection, shadowAlpha, scale);
    }

    // 에디터에서 생성 영역을 시각화 (기획서 2-1 데이터 확인용) [cite: 8]
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Vector3 center = new Vector3(spawnX, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(1f, maxY - minY, 1f);
        Gizmos.DrawCube(center, size);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(spawnX, minY, 0), new Vector3(spawnX, maxY, 0));
    }
}