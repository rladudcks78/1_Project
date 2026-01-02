using UnityEngine;

public class ToolVisualizer : MonoBehaviour
{
    [Header("Tool Prefabs")]
    [SerializeField] private GameObject _hoePrefab;
    [SerializeField] private GameObject _seedPrefab;

    private GameObject _currentToolObject; // 현재 생성된 도구 오브젝트 저장

    public void UpdateVisual(string toolName)
    {
        // 1. 기존에 들고 있던 도구가 있다면 삭제
        if (_currentToolObject != null)
        {
            Destroy(_currentToolObject);
            _currentToolObject = null;
        }

        // 2. 새로운 도구 프리팹 결정
        GameObject selectedPrefab = null;
        switch (toolName)
        {
            case "Hoe":
                selectedPrefab = _hoePrefab;
                break;
            case "Seed":
                selectedPrefab = _seedPrefab;
                break;
        }

        // 3. 프리팹 생성 및 자식으로 설정
        if (selectedPrefab != null)
        {
            _currentToolObject = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
            _currentToolObject.transform.SetParent(this.transform); // SpawnPoint의 자식으로 고정

            // 프리팹의 로컬 좌표를 (0,0,0)으로 초기화하여 부모 위치에 딱 맞게 설정
            _currentToolObject.transform.localPosition = Vector3.zero;
            _currentToolObject.transform.localRotation = Quaternion.identity;
        }
    }
}