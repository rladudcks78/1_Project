using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Farm/ToolData")]
public class ToolData : ItemData
{
    // 도구만의 특성(예: 내구도)이 필요하다면 여기에 추가하세요.
    public override void Use()
    {
        Debug.Log($"{itemName} 도구를 사용 중입니다.");
    }
}