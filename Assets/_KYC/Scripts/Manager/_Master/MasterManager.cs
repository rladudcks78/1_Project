using UnityEngine;

/// <summary>
/// Every Ville 프로젝트의 통합 컨트롤 타워.
/// 기획서에 명시된 GameManager(시간/날씨)를 포함한 모든 시스템을 관리합니다.
/// </summary>
public class MasterManager : MonoBehaviour
{
    private static MasterManager _instance;
    public static MasterManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<MasterManager>();
            return _instance;
        }
    }

    [Header("Core Data & Input")]
    [SerializeField] private DataManager _data;
    [SerializeField] private InputManager _input;
    [SerializeField] private SaveManager _save;

    [Header("Inventory & Items")]
    [SerializeField] private InventoryManager _inventory;
    [SerializeField] private ItemManager _item;

    [Header("World Systems")]
    [SerializeField] private DayManager _day; // 기획서의 GameManager (시간, 날씨)
    [SerializeField] private TileManager _tile;

    [Header("Interface & Audio")]
    [SerializeField] private UIManager _ui;
    [SerializeField] private SoundManager _sound;

    // --- 기획서 명칭 기반 정적 프로퍼티 ---
    public static DataManager Data => Instance._data;
    public static InputManager Input => Instance._input;
    public static SaveManager Save => Instance._save;
    public static InventoryManager Inventory => Instance._inventory;
    public static ItemManager Item => Instance._item;
    public static DayManager Day => Instance._day; 
    public static TileManager Tile => Instance._tile;
    public static UIManager UI => Instance._ui;
    public static SoundManager Sound => Instance._sound;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitAllManagers();
    }

    private void InitAllManagers()
    {
        Debug.Log("<color=yellow>Every Ville: 시스템 초기화 시작</color>");

        // 초기화 순서: 데이터 -> 조작 -> 월드 상태 -> 출력
        if (_item != null) _item.Init();
        if (_data != null) _data.Init();
        if (_save != null) _save.Init();

        if (_input != null) _input.Init();
        if (_tile != null) _tile.Init();

        if (_day != null) _day.Init(); 
        if (_inventory != null) _inventory.Init();

        if (_sound != null) _sound.Init();
        if (_ui != null) _ui.Init();

        Debug.Log("<color=cyan>Every Ville: 기획서 일치 확인 및 초기화 완료</color>");
    }
}