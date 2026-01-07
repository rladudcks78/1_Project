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
                // 1. 인자로 받은 데이터가 있으면 그것을 사용, 없으면 테스트용 데이터 사용
                CropData dataToPlant = (cropData != null) ? cropData : _testCropData;
                PlantSeed(gridPos, dataToPlant);
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
    private void PlantSeed(Vector3Int gridPos, CropData data)
    {
        // 방어 코드: 데이터나 프리팹이 없으면 중단
        if (data == null) { Debug.LogError("심을 작물 데이터가 없습니다!"); return; }
        if (cropPrefab == null) { Debug.LogError("Crop Prefab이 할당되지 않았습니다!"); return; }

        TileBase currentTile = _interactableTilemap.GetTile(gridPos);

        // 개간된 땅 위에서만 심기 가능
        if (currentTile == _tilledTile)
        {
            // 1. 타일맵 시각 효과 (바닥에 씨앗이 뿌려진 모습)
            _interactableTilemap.SetTile(gridPos, _seedTile);

            // 2. 실제 성장 로직을 담당할 프리팹 소환
            Vector3 worldPos = _interactableTilemap.GetCellCenterWorld(gridPos);
            GameObject go = Instantiate(cropPrefab, worldPos, Quaternion.identity);

            // 3. [핵심] 소환된 Crop 컴포넌트에 데이터 주입
            if (go.TryGetComponent<Crop>(out Crop crop))
            {
                crop.Init(data);
                Debug.Log($"{gridPos}에 {data.itemName}을(를) 심었습니다.");
            }
        }
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