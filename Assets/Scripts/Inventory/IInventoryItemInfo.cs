using UnityEngine;

public interface IInventoryItemInfo
{
    string id { get; }
    string title { get; }
    string description { get; }
    
    bool canUsed { get; }
    bool canEquipped { get; }
    InventoryItemType type { get; }

    int maxItemsInInventorySlot { get; }
    Sprite icon { get; }
}