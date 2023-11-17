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
        itemData.info = null;
        itemData.state = null;

        UpdateSprite();
    }
}