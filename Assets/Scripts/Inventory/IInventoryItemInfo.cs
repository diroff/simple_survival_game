using UnityEngine;

public interface IInventoryItemInfo
{
    string id { get; }
    string title { get; }
    string description { get; }
    
    InventoryItemType type { get; }

    int maxItemsInInventorySlot { get; }
    Sprite icon { get; }
    Sprite sprite { get; }
}