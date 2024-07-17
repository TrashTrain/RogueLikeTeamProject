using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Tooltip("������ ������ �̹���")]
    [SerializeField] private Image _iconImage;

    [Tooltip("������ ��Ŀ���� �� ��Ÿ���� ���̶���Ʈ �̹���")]
    [SerializeField] private Image _highlightImage;

    [Space]
    [Tooltip("���̶���Ʈ �̹��� ���� ��")]
    [SerializeField] private float _highlightAlpha = 0.5f;

    [Tooltip("���̶���Ʈ �ҿ� �ð�")]
    [SerializeField] private float _highlightFadeDuration = 0.2f;

    // ������ �ε���
    public int Index {  get; private set; }

    // ������ �������� �����ϰ� �ִ��� ����
    public bool HasItem => _iconImage.sprite != null;

    // ���� �������� ����
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

    // ���� ���̶���Ʈ ���İ�

    private float _currentHLAlpha = 0f;


    private bool _isAccessibleSlot = true;  // ���� ���ٰ��� ����
    private bool _isAccessibleItem = true;  // ������ ���ٰ��� ����

    // ��Ȱ��ȭ�� ������ ����
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    public void SetSlotIndex(int index) => Index = index;

    // ���� ��ü�� Ȱ��ȭ/��Ȱ��ȭ ���� ����
    public void SetSlotAccessibleState(bool value)
    {
        // �ߺ� ó���� ����
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


    // ������ Ȱ��ȭ/��Ȱ��ȭ ���� ����
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

    // �ٸ� ���԰� ������ ������ ��ȯ
    public void SwapOrMoveIcon(ItemSlotUI other)
    {
        if(other == null) return;   
        if(other == this) return;
        if(!this.IsAccessible) return;
        if(!other.IsAccessible) return;

        var temp = _iconImage.sprite;

        // 1. ��� �������� �ִ� ��� : ��ȯ
        if (other.HasItem) SetItem(other._iconImage.sprite);

        // 2. ���� ��� : �̵�
        else RemoveItem();

        other.SetItem(temp);
    }

    // ���Կ� ������ ���
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

    // ���Կ� ������ ����
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