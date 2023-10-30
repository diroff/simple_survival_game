using UnityEngine;

public class ItemInEquipment : Item
{
    public override void SetItem(IInventoryItem item)
    {
        if (item == null)
            ClearItem();

        base.SetItem(item);
    }

    private void ClearItem()
    {
        _info = null;
        _state = null;
        UpdateSprite();
    }
}