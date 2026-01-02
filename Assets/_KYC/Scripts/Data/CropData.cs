using UnityEngine;

/// <summary>
/// 농작물의 성장 단계 및 정보를 담는 데이터 에셋
/// </summary>
[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/CropData")]
public class CropData : ScriptableObject
{
    [Header("Crop Info")]
    public string cropName;          // 작물 이름 [cite: 46]
    public float growthTimePerStage; // 단계별 성장 소요 시간 [cite: 16]

    [Header("Growth Assets")]
    // 성장 단계별 스프라이트 (예: 0-씨앗, 1-새싹, 2-성장, 3-수확가능)
    public Sprite[] growthSprites; 

    [Header("Harvest Info")]
    public ItemData harvestItem;     // 수확 시 획득할 아이템 [cite: 16]
    public int minHarvestCount = 1;  // 최소 수확량
    public int maxHarvestCount = 3;  // 최대 수확량
}