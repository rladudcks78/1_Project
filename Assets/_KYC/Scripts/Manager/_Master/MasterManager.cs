using UnityEngine;

/// <summary>
/// 모든 시스템의 중앙 진입점 역할을 하는 마스터 매니저
/// </summary>
public class MasterManager : MonoBehaviour
{
    // 외부에서 접근할 때는 MasterManager.Instance.TimeManager 처럼 사용
    public static MasterManager Instance { get; private set; }

    [Header("Sub Managers")]
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private TileManager tileManager;
    // 필요한 매니저들을 여기에 계속 추가...

    // 읽기 전용 프로퍼티 (외부 접근용)
    public TimeManager Time => timeManager;
    public DataManager Data => dataManager;
    public TileManager Tile => tileManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitManagers()
    {
        // 1. 데이터 로드가 가장 먼저 (세이브 파일 읽기 등)
        //if (dataManager != null) dataManager.Init();

        // 2. 타일 시스템 초기화
        //if (tileManager != null) tileManager.Init();

        // 3. 시간 흐름 시작
        //if (timeManager != null) timeManager.Init();

        Debug.Log("모든 매니저 초기화 완료");
    }
}