public class ItemData : IInventoryItem
{
    public IInventoryItemInfo info { get; set; }

    public IInventoryItemState state { get; set; }

    public string ID => info.id;

    public ItemData(IInventoryItemInfo info)
    {
        this.info = info;
        state = new InventoryItemState();
    }

    public IInventoryItem Clone()
    {
        var clonedItem = new ItemData(info);
        clonedItem.state.amount = state.amount;
        return clonedItem;
    }
}