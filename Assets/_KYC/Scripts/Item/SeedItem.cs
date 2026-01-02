using UnityEditor.ShaderGraph;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed", menuName = "Farm/Item/Seed")]
public class SeedItem : ItemData
{
    [Header("Seed Specific")]
    public CropData cropToPlant; // 심어질 작물의 성장 데이터

    public override void Use()
    {
        // 1. 플레이어가 타일을 바라보고 있는지 확인
        // 2. 해당 타일이 경작된 상태인지 확인
        // 3. 씨앗 심기 로직 실행
        Debug.Log($"{itemName}을(를) 밭에 심었습니다!");
    }
}