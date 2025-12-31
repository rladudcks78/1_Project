using UnityEngine;

[CreateAssetMenu(fileName = "Crop_", menuName = "Farm/Item/Crop")]
public class CropData : ItemData
{
    [Header("성장 데이터")]
    public int totalGrowthStages;       // 총 성장 단계 (예: 4단계)
    public float timePerStage;          // 단계별 성장 시간(분 단위 혹은 게임 내 시간)
    public Sprite[] growthSprites;      // 단계별 스프라이트 배열

    [Header("수확 결과")]
    public ItemData harvestedItem;      // 수확 시 인벤토리에 들어올 아이템
    public int minHarvestAmount = 1;    // 최소 수확량
    public int maxHarvestAmount = 3;    // 최대 수확량
}