using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 상점에서 판매 수량을 정밀하게 조절하고 최종 확인을 담당하는 팝업 스크립트입니다.
/// </summary>
public class SellConfirmationPopup : MonoBehaviour
{
    [Header("UI - Display")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("UI - Controls")]
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;

    [Header("UI - Decision")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private ItemData _currentItem;
    private int _currentQuantity;
    private int _maxQuantity;

    /// <summary>
    /// 상점 인벤토리에서 우클릭 시 호출되어 팝업을 초기화합니다.
    /// </summary>
    public void OpenPopup(ItemData item, int inventoryCount)
    {
        _currentItem = item;
        _maxQuantity = inventoryCount;
        _currentQuantity = 1;

        // 초기 시각 정보 설정
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;

        // 슬라이더 설정
        quantitySlider.minValue = 1;
        quantitySlider.maxValue = _maxQuantity;
        quantitySlider.value = 1;

        // --- 이벤트 리스너 등록 (중복 방지를 위해 Clear 후 등록) ---

        // 1. 슬라이더 이벤트
        quantitySlider.onValueChanged.RemoveAllListeners();
        quantitySlider.onValueChanged.AddListener(val => UpdateQuantity((int)val));

        // 2. (+, -) 버튼 이벤트
        plusButton.onClick.RemoveAllListeners();
        plusButton.onClick.AddListener(() => ChangeQuantity(1));

        minusButton.onClick.RemoveAllListeners();
        minusButton.onClick.AddListener(() => ChangeQuantity(-1));

        // 3. 결정 버튼 이벤트
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmClick);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));

        UpdateUI();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 버튼을 통해 수량을 조절할 때 호출됩니다.
    /// </summary>
    private void ChangeQuantity(int amount)
    {
        // 슬라이더 값을 변경하면 onValueChanged에 의해 UpdateQuantity가 자동으로 호출됩니다.
        quantitySlider.value += amount;
    }

    /// <summary>
    /// 슬라이더나 버튼에 의해 최종 수량이 변경되었을 때 실행됩니다.
    /// </summary>
    private void UpdateQuantity(int newQuantity)
    {
        _currentQuantity = Mathf.Clamp(newQuantity, 1, _maxQuantity);
        UpdateUI();
    }

    /// <summary>
    /// 텍스트 정보를 갱신합니다.
    /// </summary>
    private void UpdateUI()
    {
        countText.text = $"{_currentQuantity} / {_maxQuantity}";
        int totalPrice = _currentQuantity * _currentItem.sellPrice;
        priceText.text = $"총 합계: {totalPrice} G";
    }

    private void OnConfirmClick()
    {
        // 20년 차 팁: 실제 판매 로직은 Manager에게 맡겨 책임을 분리합니다.
        MasterManager.Shop.SellItem(_currentItem, _currentQuantity);
        gameObject.SetActive(false);
    }
}