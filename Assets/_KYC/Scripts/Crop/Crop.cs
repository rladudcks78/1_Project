using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropData data;
    private int currentStage = 0;
    private float timer = 0;
    private SpriteRenderer sr;

    public void Init(CropData cropData)
    {
        data = cropData;
        sr = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    void Update()
    {
        // 데이터가 없으면 실행하지 않음 (Null 에러 방지)
        if (data == null) return;

        if (currentStage >= data.growthSprites.Length - 1) return;

        timer += Time.deltaTime;
        if (timer >= data.growthTimePerStage)
        {
            timer = 0;
            currentStage++;
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        if (data.growthSprites.Length > currentStage)
        {
            sr.sprite = data.growthSprites[currentStage];
        }
    }

    // 수확 가능한지 확인하는 메서드
    public bool CanHarvest() => currentStage >= data.growthSprites.Length - 1;

    public ItemData GetHarvestItem()
    {
        if (data == null)
        {
            Debug.LogError("Crop: 데이터(data)가 비어있습니다!");
            return null;
        }

        if (data.harvestItem == null)
        {
            Debug.LogError($"Crop: {data.itemName}의 Harvest Item이 인스펙터에서 설정되지 않았습니다!");
        }

        return data.harvestItem;
    }
}