using UnityEngine;

/// <summary>
/// 플레이어가 장착한 아이템의 아이콘을 머리 위에 표시합니다. 
/// </summary>
public class PlayerEquipDisplay : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerData _playerData; // 참조 

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_playerData.currentEquippedItem != null)
        {
            // 장착된 아이템의 아이콘을 표시 [cite: 45]
            _spriteRenderer.sprite = _playerData.currentEquippedItem.icon;
            _spriteRenderer.enabled = true;
        }
        else
        {
            _spriteRenderer.enabled = false;
        }
    }
}