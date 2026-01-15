using UnityEngine;

/// <summary>
/// 농작물의 성장 단계 및 정보를 담는 데이터 에셋.
/// ItemData를 상속받아 인벤토리와 호환되게 합니다.
/// </summary>
[CreateAssetMenu(fileName = "New Seed", menuName = "Farm/Item/Crop")]
public class CropData : ItemData // ScriptableObject 대신 ItemData 상속
{
    // [중요] cropName은 부모인 ItemData의 itemName과 중복되므로 삭제하거나 
    // 기본 itemName을 사용하도록 리팩토링합니다.

    [Header("Growth Info")]
    public float growthTimePerStage; // 단계별 성장 소요 시간

    [Header("Growth Assets")]
    // 성장 단계별 스프라이트 (0-씨앗, 1-새싹, 2-성장, 3-수확가능)
    public Sprite[] growthSprites;

    [Header("Harvest Info")]
    public ItemData harvestItem;     // 수확 시 획득할 아이템
    public int minHarvestCount = 1;  // 최소 수확량
    public int maxHarvestCount = 3;  // 최대 수확량

    /// <summary>
    /// ItemData의 추상 메서드 구현
    /// </summary>
    public override void Use()
    {
        // 씨앗을 인벤토리에서 사용할 때의 로직 (예: 장착)
        Debug.Log($"{itemName} 씨앗을 선택했습니다.");
    }
}