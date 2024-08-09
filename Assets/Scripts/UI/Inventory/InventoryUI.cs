using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 10)]
    [SerializeField] private int _horizontalSlotCount = 8;
    [Range(0, 10)]
    [SerializeField] private int _verticalSlotCount = 3;
    [SerializeField] private float _slotMargin = 8f;
    [SerializeField] private float _invenAreaPadding = 20f;
    [Range(32, 64)]
    [SerializeField] private float _slotSize = 64f;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform _InvenAreaRT;
    [SerializeField] private GameObject _slotUiPrefab;

    private void InitSlots()
    {
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);

        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
        {
            _slotUiPrefab.AddComponent<ItemSlotUI>();   
        }

        _slotUiPrefab.SetActive(false);

        Vector2 beginPos = new Vector2(_invenAreaPadding,-_invenAreaPadding);
        Vector2 curPos = beginPos;

        var _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);

        //슬롯 동적 생성
        for(int j=0; j<_verticalSlotCount; j++)
        {
            for(int i=0; i<_horizontalSlotCount; i++)
            {
                int slotIndex = (_horizontalSlotCount * j) + i;

                var slotRT = CloneSlot();
                slotRT.pivot = new Vector2(0f, 1f);
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                //slotUI.SetSlotIndex(slotIndex);  JI
                _slotUIList.Add(slotUI);

                curPos.x += (_slotMargin + _slotSize);
            }

            curPos.x = beginPos.x;
            curPos.y -= (_slotMargin + _slotSize);
        }

        if(_slotUiPrefab.scene.rootCount != 0)
        {
            Destroy(_slotUiPrefab);
        }


        RectTransform CloneSlot()
        {
            GameObject slotGo = Instantiate(_slotUiPrefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(_InvenAreaRT);

            return rt;
        }
    }

}
