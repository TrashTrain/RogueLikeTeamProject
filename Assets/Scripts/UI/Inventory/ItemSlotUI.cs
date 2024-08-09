using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image _iconImage;

    [Tooltip("슬롯이 포커스될 때 나타나는 하이라이트 이미지")]
    [SerializeField] private Image _highlightImage;

    [Space]
    [Tooltip("하이라이트 이미지 알파 값")]
    [SerializeField] private float _highlightAlpha = 0.5f;

    [Tooltip("하이라이트 소요 시간")]
    [SerializeField] private float _highlightFadeDuration = 0.2f;

    // 슬롯의 인덱스
    public int Index {  get; private set; }

    // 슬롯이 아이템을 보유하고 있는지 여부
    public bool HasItem => _iconImage.sprite != null;

    // 접근 가능한지 여부
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;

    public RectTransform SlotRect => _slotRect;
    public RectTransform IconRect => _iconRect;

    private InventoryUI _inventoryUI;

    private RectTransform _slotRect;
    private RectTransform _iconRect;
    private RectTransform _highlightRect;

    private GameObject _iconGo;
    //private GameObject _textGo;
    private GameObject _highlightGo;

    private Image _slotImage;

    // 현재 하이라이트 알파값

    private float _currentHLAlpha = 0f;


    private bool _isAccessibleSlot = true;  // 슬롯 접근가능 여부
    private bool _isAccessibleItem = true;  // 아이템 접근가능 여부

    // 비활성화된 슬롯의 색상
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    public void SetSlotIndex(int index) => Index = index;

    // 슬롯 자체의 활성화/비활성화 여부 설정
    public void SetSlotAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if(_isAccessibleSlot == value) return;

        if (value)
        {
            _slotImage.color = Color.black;
        }
        else
        {
            _slotImage.color = InaccessibleSlotColor;
            HideIcon();
        }

        _isAccessibleSlot = value;
    }


    // 아이템 활성화/비활성화 여부 설정
    public void SetItemAccessibleState(bool value)
    {
        if(_isAccessibleItem == value) return;

        if (value)
        {
            _iconImage.color = Color.white;
        }
        else
        {
            _iconImage.color= InaccessibleIconColor;
        }

        _isAccessibleItem= value;
    }

    // 다른 슬롯과 아이템 아이콘 교환
    public void SwapOrMoveIcon(ItemSlotUI other)
    {
        if(other == null) return;   
        if(other == this) return;
        if(!this.IsAccessible) return;
        if(!other.IsAccessible) return;

        var temp = _iconImage.sprite;

        // 1. 대상에 아이템이 있는 경우 : 교환
        if (other.HasItem) SetItem(other._iconImage.sprite);

        // 2. 없을 경우 : 이동
        else RemoveItem();

        other.SetItem(temp);
    }

    // 슬롯에 아이템 등록
    public void SetItem(Sprite itemSprite)
    {
        if(itemSprite != null)
        {
            _iconImage.sprite = itemSprite;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }

    // 슬롯에 아이템 제거
    public void RemoveItem()
    {
        _iconImage.sprite=null;
        HideIcon();
    }
    public void SetIconAlpha(float alpha)
    {
        _iconImage.color = new Color(_iconImage.color.r, _iconImage.color.g, _iconImage.color.b, alpha);
    }
}
