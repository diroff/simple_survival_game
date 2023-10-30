using System;

public interface IInventory
{
    int capacity { get; set; }
    bool isFull { get; }

    IInventoryItem GetItem(string id);
    IInventoryItem[] GetAllItems();
    IInventoryItem[] GetAllItems(string id);
    IInventoryItem[] GetEquippedItems();
    int GetItemAmount(string id);

    bool TryToAdd(object sender, IInventoryItem item);
    bool UseItem(object sender, string id);
    bool EquipItem(object sender, IInventoryItem item);
    bool UnequipItem(object sender, IInventoryItem item);
    void Remove(object sender, string id, int amount = 1);
    bool HasItem(string id, out IInventoryItem item);
}