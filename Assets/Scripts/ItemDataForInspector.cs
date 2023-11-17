using System;
using UnityEngine;

[Serializable]
public class ItemDataForInspector
{
    [SerializeField] private InventoryItemInfo _info;
    [SerializeField] private InventoryItemState _state;

    private ItemData _itemData;

    public ItemData GetData()
    {
        if (_itemData == null)
            SetData();

        return _itemData;
    }

    private void SetData()
    {
        _itemData = new ItemData(_info);
        _itemData.state = _state;
    }
}