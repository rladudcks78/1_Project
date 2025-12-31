using System.Collections;
using UnityEngine;

// 가독성을 위해 상단에 별칭(Alias)이나 사용을 명시하는 습관은 좋습니다.
using WaitTime = UnityEngine.WaitForSeconds;

public class CropController : MonoBehaviour
{
    [SerializeField] private ItemData _cropData;
    private bool _isGrown = false;

    // 캐싱을 통한 성능 최적화 (매번 new를 생성하지 않음)
    private WaitTime _growthCycleWait = new WaitTime(1.0f);

    private void Start()
    {
        // 2주 완성 및 출시를 위해 코루틴으로 성장을 관리
        StartCoroutine(ProcessGrowth());
    }

    /// <summary>
    /// 작물의 성장을 관리하는 코루틴입니다.
    /// </summary>
    private IEnumerator ProcessGrowth()
    {
        Debug.Log($"{_cropData.itemName} 성장을 시작합니다.");

        // 예시: 5단계 성장 시뮬레이션
        int currentStage = 0;
        int maxStage = 5;

        while (currentStage < maxStage)
        {
            yield return _growthCycleWait;
            currentStage++;
            // 여기서 PlayerAnimationHandler처럼 애니메이션이나 스프라이트 변경 호출
            UpdateCropVisual(currentStage);
        }

        _isGrown = true;
        Debug.Log($"{_cropData.itemName} 수확 가능!");
    }

    private void UpdateCropVisual(int stage)
    {
        // 스프라이트 변경 로직 (시각적 피드백)
    }
}