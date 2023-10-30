using UnityEngine;

public class ItemInEquipment : Item
{
    public override void SetItem(IInventoryItem item)
    {
        if (item == null)
        {
            ClearItem();
            return;
        }

        base.SetItem(item);
    }

    private void ClearItem()
    {
        _info = null;
        _state = null;

        ItemData.info = _info;
        ItemData.state = _state;

        UpdateSprite();
    }
}