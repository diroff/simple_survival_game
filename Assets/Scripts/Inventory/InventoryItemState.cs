using System;

[Serializable]
public class InventoryItemState : IInventoryItemState
{
    public int itemAmount = 1;
    public bool isItemEquipped;

    public int amount { get => itemAmount; set => itemAmount = value; }
    public bool isEquipped { get => isItemEquipped; set => isItemEquipped = value; }

    public InventoryItemState() 
    {
        itemAmount = 1;
        isItemEquipped = false;
    }
}