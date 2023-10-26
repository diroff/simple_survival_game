using System;

public class InventorySlot : IInventorySlot
{
    public bool isFull => !isEmpty && amount == capacity;

    public bool isEmpty => item == null;

    public IInventoryItem item { get; private set; } 

    public int amount => isEmpty? 0 : item.state.amount;

    public int capacity { get; private set; }

    public string itemId => isEmpty? null : item.ID;

    public void SetItem(IInventoryItem item)
    {
        if (!isEmpty)
            return;

        this.item = item;
        this.capacity = item.info.maxItemsInInventorySlot;
    }

    public void Clear()
    {
        if (isEmpty)
            return;

        item.state.amount = 0;
        item = null;
    }
}