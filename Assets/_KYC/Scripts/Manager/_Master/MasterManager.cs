using UnityEngine;

/// <summary>
/// 서비스 로케이터 패턴의 단순화 버전입니다.
/// 각 매니저에 접근하는 통로 역할만 수행하며, 로직은 각 매니저가 담당합니다.
/// </summary>
public class MasterManager : MonoBehaviour
{
    public static MasterManager Instance { get; private set; }

    // 하위 매니저들에 대한 참조 (인스펙터에서 할당하거나 Awake에서 GetInChild)
    [field: SerializeField] public TimeManager Time { get; private set; }
    [field: SerializeField] public DataManager Data { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitAllManagers();
    }

    private void InitAllManagers()
    {
        // DataManager 초기화
        if (Data != null) Data.Initialize();

        // TimeManager 초기화
        if (Time != null) Time.Initialize();

        Debug.Log("모든 시스템 가동 준비 완료.");
    }
}