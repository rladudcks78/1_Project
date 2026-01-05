using UnityEngine;

public class ToolVisualizer : MonoBehaviour
{
    [Header("Tool Prefabs")]
    [SerializeField] private GameObject _hoePrefab;
    [SerializeField] private GameObject _seedPrefab;

    private GameObject _currentToolObject; // 현재 생성된 도구 오브젝트 저장

    public void UpdateVisual(string toolName)
    {
        // 1. 기존 도구 삭제
        if (_currentToolObject != null)
        {
            Destroy(_currentToolObject);
            _currentToolObject = null;
        }

        // 2. 새로운 도구 프리팹 결정 (문자열 포함 여부로 체크)
        GameObject selectedPrefab = null;

        // toolName이 "괭이" 또는 "Hoe"를 포함하고 있는지 확인
        if (toolName.Contains("괭이") || toolName.ToLower().Contains("hoe"))
        {
            selectedPrefab = _hoePrefab;
        }
        else if (toolName.Contains("씨앗") || toolName.ToLower().Contains("seed"))
        {
            selectedPrefab = _seedPrefab;
        }

        // 3. 프리팹 생성
        if (selectedPrefab != null)
        {
            _currentToolObject = Instantiate(selectedPrefab, transform.position, Quaternion.identity, transform);

            // 위치 및 회전 초기화
            _currentToolObject.transform.localPosition = Vector3.zero;
            _currentToolObject.transform.localRotation = Quaternion.identity;
        }
    }
}