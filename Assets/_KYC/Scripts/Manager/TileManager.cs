using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private Tilemap _interactableTilemap;

    [Header("Tiles")]
    [SerializeField] private TileBase _tilledTile; // 개간된 흙
    [SerializeField] private TileBase _seedTile;   // 씨앗 타일 (바닥 시각 효과)

    [Header("Crop Settings")]
    [SerializeField] private GameObject cropPrefab; // Crop 스크립트가 붙은 프리팹
    [SerializeField] private CropData _testCropData; // 테스트용 데이터 (인스펙터 할당)

    public void Init()
    {
        Debug.Log("<color=green>TileManager: 초기화 완료</color>");
    }

    // [수정] 외부(PlayerInteract)에서 CropData를 인자로 넘겨줄 수 있도록 확장
    public void HandleInteraction(Vector3 worldPos, string toolType, CropData cropData = null)
    {
        if (_groundTilemap == null || _interactableTilemap == null) return;

        Vector3Int gridPos = _groundTilemap.WorldToCell(worldPos);

        if (!_groundTilemap.HasTile(gridPos)) return;

        switch (toolType)
        {
            case "Hoe":
                TillGround(gridPos);
                break;
            case "Seed":
                CropData dataToPlant = (cropData != null) ? cropData : _testCropData;

                // [핵심] 심기에 실제로 성공했을 때만 인벤토리에서 차감
                if (PlantSeed(gridPos, dataToPlant))
                {
                    MasterManager.Inventory.ConsumeSelectedSlotItem();
                }
                break;
            case "None":
                Debug.Log("맨손 상태");
                break;
        }
    }

    private void TillGround(Vector3Int gridPos)
    {
        // 1. 기본 땅(_groundTilemap)이 있는지 먼저 확인
        if (!_groundTilemap.HasTile(gridPos)) return;

        // 2. 상호작용 레이어에서 현재 타일 정보를 가져옴
        TileBase currentInteractTile = _interactableTilemap.GetTile(gridPos);

        // 3. 만약 이미 개간된 타일(_tilledTile)이라면 다시 팔 필요 없음
        if (currentInteractTile == _tilledTile) return;

        // 4. 씨앗이 이미 심어져 있는 타일(_seedTile)도 건드리지 않음
        if (currentInteractTile == _seedTile) return;

        // 그 외의 경우(비어 있거나 다른 타일이 있는 경우) 개간 진행
        _interactableTilemap.SetTile(gridPos, _tilledTile);
        Debug.Log($"<color=yellow>{gridPos} 위치 개간 성공!</color>");
    }

    // [수정] CropData를 인자로 받아 실제 오브젝트에 주입합니다.
    private bool PlantSeed(Vector3Int gridPos, CropData data)
    {
        if (data == null || cropPrefab == null) return false;

        TileBase currentTile = _interactableTilemap.GetTile(gridPos);

        // 개간된 땅(_tilledTile) 위에서만 심기 가능
        if (currentTile == _tilledTile)
        {
            // 타일맵 시각 효과 설정
            _interactableTilemap.SetTile(gridPos, _seedTile);

            // 작물 프리팹 생성 및 데이터 주입
            Vector3 worldPos = _interactableTilemap.GetCellCenterWorld(gridPos);
            GameObject go = Instantiate(cropPrefab, worldPos, Quaternion.identity);

            if (go.TryGetComponent<Crop>(out Crop crop))
            {
                crop.Init(data);
                return true; // 심기 성공!
            }
        }
        return false; // 심기 실패 (땅이 개간되지 않았음 등)
    }

    public void ResetToTilled(Vector3 worldPos)
    {
        // 월드 좌표를 타일맵 좌표로 변환
        Vector3Int gridPos = _interactableTilemap.WorldToCell(worldPos);

        // 해당 위치를 다시 '개간된 흙 타일'로 변경
        _interactableTilemap.SetTile(gridPos, _tilledTile);

        Debug.Log($"<color=yellow>{gridPos} 위치가 다시 개간된 상태로 복구되었습니다.</color>");
    }
}