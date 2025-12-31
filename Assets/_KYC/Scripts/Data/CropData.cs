using UnityEngine;

// CreateAssetMenu를 사용하면 유니티 에디터에서 마우스 우클릭만으로 새 작물 데이터를 만들 수 있어.
[CreateAssetMenu(fileName = "New Crop", menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    [Header("기본 정보")]
    public string cropName;         // 작물 이름
    public Sprite[] growthSprites;  // 성장 단계별 이미지 (0: 씨앗, 마지막: 수확 직전)

    [Header("성장 설정")]
    public int daysToGrow;          // 총 성장 기간 (일 단위)
    public int purchasePrice;       // 씨앗 구매 가격
    public int sellPrice;           // 수확물 판매 가격

    // 나중에 '비료'나 '계절' 시스템을 넣고 싶다면 여기에 변수만 추가하면 돼. 
    // 기존 코드를 고칠 필요가 없으니 유지보수에 아주 좋지.
}