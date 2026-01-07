using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Farm/Item/Tool")]
public class ToolItem : ItemData
{
    public int power; // 도구의 위력 (ex. 나무를 베는 속도)

    public override void Use()
    {
        // 도구 사용 애니메이션 재생 및 타일 상호작용
        Debug.Log($"{itemName} 도구를 사용합니다.");
    }
}