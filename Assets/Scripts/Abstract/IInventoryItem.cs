public interface IInventoryItem
{
    IInventoryItemInfo info { get; }
    IInventoryItemState state { get; }

    string ID { get; }

    IInventoryItem Clone();
}