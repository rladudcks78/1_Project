using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap _groundTilemap;      // 기본 풀밭
    [SerializeField] private Tilemap _interactableTilemap; // 개간된 땅 & 작물 레이어

    [Header("Tiles")]
    [SerializeField] private TileBase _tilledTile; // 개간된 흙 타일
    [SerializeField] private TileBase _seedTile;   // 씨앗 타일 (또는 작물 1단계)

    public void Init()
    {
        Debug.Log("<color=green>TileManager: 초기화 완료</color>");
    }

    public void HandleInteraction(Vector3 worldPos, string toolType)
    {
        if (_groundTilemap == null || _interactableTilemap == null) return;

        Vector3Int gridPos = _groundTilemap.WorldToCell(worldPos);

        // [추가] Ground 타일맵에 타일이 있는 곳인지 확인 (절벽이나 물 위 개간 방지)
        if (!_groundTilemap.HasTile(gridPos))
        {
            Debug.Log("여기는 땅이 아닙니다.");
            return;
        }

        switch (toolType)
        {
            case "Hoe":
                TillGround(gridPos);
                break;
            case "Seed":
                PlantSeed(gridPos);
                break;
            case "None":
                Debug.Log("맨손으로는 땅을 파거나 씨앗을 심을 수 없습니다.");
                break;
        }
    }

    private void TillGround(Vector3Int gridPos)
    {
        // 1. 이미 개간되었거나 무엇인가 심어져 있다면 무시
        if (_interactableTilemap.HasTile(gridPos)) return;

        // 2. 개간된 흙 타일로 교체
        _interactableTilemap.SetTile(gridPos, _tilledTile);
        Debug.Log($"{gridPos} 위치 개간 완료!");
    }

    private void PlantSeed(Vector3Int gridPos)
    {
        // 1. 개간된 땅(tilledTile) 위에서만 씨앗을 심을 수 있음
        TileBase currentTile = _interactableTilemap.GetTile(gridPos);

        if (currentTile == _tilledTile)
        {
            // 2. 씨앗 타일로 교체 (실제로는 작물 오브젝트를 소환하거나 타일을 덮어씀)
            _interactableTilemap.SetTile(gridPos, _seedTile);
            Debug.Log($"{gridPos} 위치에 씨앗을 심었습니다!");
        }
        else
        {
            Debug.Log("여기는 먼저 괭이로 갈아야 합니다.");
        }
    }
}