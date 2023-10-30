using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWithSlots : IInventory
{
    public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
    public event Action<object, string, int> OnInventoryItemRemovedEvent;
    public event Action<object> OnInventoryStateChangedEvent;

    public int capacity { get; set; }
    public bool isFull => _slots.All(slot => isFull);

    private List<IInventorySlot> _slots;

    public InventoryWithSlots(int capacity)
    {
        this.capacity = capacity;
        _slots = new List<IInventorySlot>(capacity);

        for (int i = 0; i < capacity; i++)
            _slots.Add(new InventorySlot());
    }

    public IInventoryItem GetItem(string id)
    {
        var item = _slots.Find(slot => slot.itemId == id).item;

        return item;
    }

    public IInventoryItem[] GetAllItems()
    {
        var allItems = new List<IInventoryItem>();
        foreach (var slot in _slots)
            if (!slot.isEmpty)
                allItems.Add(slot.item);

        return allItems.ToArray();
    }

    public IInventoryItem[] GetAllItems(string id)
    {
        var allItems = new List<IInventoryItem>();
        var slotsOfType = _slots.FindAll(slot => !slot.isEmpty && slot.itemId == id);

        foreach (var slot in slotsOfType)
            allItems.Add(slot.item);

        return allItems.ToArray();
    }

    public IInventoryItem[] GetEquippedItems()
    {
        var requiredSlots = _slots.FindAll(slot => !slot.isEmpty && slot.item.state.isEquipped);
        var equippedItems = new List<IInventoryItem>();

        foreach (var slot in requiredSlots)
            equippedItems.Add(slot.item);

        return equippedItems.ToArray();
    }

    public int GetItemAmount(string id)
    {
        var amount = 0;
        var allItemsSlots = _slots.FindAll(slot => !slot.isEmpty && slot.itemId == id);

        foreach (var itemSlot in allItemsSlots)
            amount += itemSlot.amount;

        return amount;
    }

    public bool TryToAdd(object sender, IInventoryItem item)
    {
        var slotWithSameItemButNotEmpty = _slots.Find(slot => !slot.isEmpty && slot.itemId == item.ID && !slot.isFull);

        if (slotWithSameItemButNotEmpty != null)
            return (TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item));

        var emptySlot = _slots.Find(slot => slot.isEmpty);

        if (emptySlot != null)
            return TryToAddToSlot(sender, emptySlot, item);

        Debug.Log("Inventory is Full!");
        return false;
    }

    public bool TryToAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
    {
        var fits = slot.amount + item.state.amount <= item.info.maxItemsInInventorySlot;
        var amountToAdd = fits ? item.state.amount : item.info.maxItemsInInventorySlot - slot.amount;
        var amountLeft = item.state.amount - amountToAdd;

        var clonedItem = item.Clone();
        clonedItem.state.amount = amountToAdd;

        if (slot.isEmpty)
            slot.SetItem(clonedItem);
        else
            slot.item.state.amount += amountToAdd;

        OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
        OnInventoryStateChangedEvent?.Invoke(sender);

        if (amountLeft <= 0)
            return true;

        item.state.amount = amountLeft;
        return TryToAdd(sender, item);
    }

    public void TransitFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
    {
        if (fromSlot.itemId != toSlot.itemId && !fromSlot.isEmpty && !toSlot.isEmpty)
        {
            ExchandedItemsFromSlots(this, fromSlot, toSlot);
            return;
        }

        if (fromSlot.isEmpty)
            return;

        if (toSlot.isFull)
            return;

        if (!toSlot.isEmpty && fromSlot.itemId != toSlot.itemId)
            return;

        if (fromSlot == toSlot)
            return;

        var slotCapacity = fromSlot.capacity;
        var fits = fromSlot.amount + toSlot.amount <= slotCapacity;
        var amountToAdd = fits ? fromSlot.amount : slotCapacity - toSlot.amount;
        var amountLeft = fromSlot.amount - amountToAdd;

        if (toSlot.isEmpty)
        {
            toSlot.SetItem(fromSlot.item);
            fromSlot.Clear();
            OnInventoryStateChangedEvent?.Invoke(sender);
        }

        toSlot.item.state.amount += amountToAdd;
        if (fits)
            fromSlot.Clear();
        else
            fromSlot.item.state.amount = amountLeft;

        OnInventoryStateChangedEvent?.Invoke(sender);
    }

    public void ExchandedItemsFromSlots(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
    {
        var fromItem = fromSlot.item;
        var toItem = toSlot.item;

        var toSlotAmount = toSlot.amount;
        var fromSlotAmount =  fromSlot.amount;

        fromSlot.Clear();
        toSlot.Clear();

        fromItem.state.amount = fromSlotAmount;
        toItem.state.amount = toSlotAmount;

        toSlot.SetItem(fromItem);
        fromSlot.SetItem(toItem);

        OnInventoryStateChangedEvent?.Invoke(sender);
    }

    public void Remove(object sender, string id, int amount = 1)
    {
        var slotsWithItem = GetAllSlots(id);

        if (slotsWithItem.Length == 0)
            return;

        var amountToRemove = amount;
        var count = slotsWithItem.Length;

        for (int i = count - 1; i >= 0; i--)
        {
            var slot = slotsWithItem[i];

            if (slot.amount >= amountToRemove)
            {
                slot.item.state.amount -= amountToRemove;

                if (slot.amount <= 0)
                    slot.Clear();

                OnInventoryItemRemovedEvent?.Invoke(sender, id, amountToRemove);
                OnInventoryStateChangedEvent?.Invoke(sender);
                Debug.Log($"Item removed from inventory. ItemType: {id}, amount:{amountToRemove}");
                break;
            }

            var amountRemoved = slot.amount;
            amountToRemove -= slot.amount;
            slot.Clear();
            OnInventoryItemRemovedEvent?.Invoke(sender, id, amountRemoved);
            OnInventoryStateChangedEvent?.Invoke(sender);
            Debug.Log($"Item removed from inventory. ItemType: {id}, amount:{amountRemoved}");
        }
    }

    public void RemoveFromSlot(object sender, IInventorySlot slot, int amount = 1)
    {
        if (slot.isEmpty)
            return;

        slot.item.state.amount -= amount;
        
        if (slot.amount <= 0)
            slot.Clear();

        OnInventoryItemRemovedEvent?.Invoke(sender, slot.itemId, amount);
        OnInventoryStateChangedEvent?.Invoke(sender);

        Debug.Log($"Item removed from inventory. ItemType: {slot.itemId}, amount:{amount}");
    }

    public bool HasItem(string id, out IInventoryItem item)
    {
        item = GetItem(id);

        if (item == null)
            Debug.Log("Has no item in invent");

        return item != null;
    }

    public IInventorySlot[] GetAllSlots(string id)
    {
        return _slots.FindAll(slot => !slot.isEmpty && slot.itemId == id).ToArray();
    }

    public IInventorySlot[] GetAllSlots()
    {
        return _slots.ToArray();
    }

    public bool UseItem(object sender, string id)
    {
        var item = GetItem(id);

        if (item == null)
        {
            Debug.Log("No available items");
            return false;
        }

        Remove(this, id);
        return true;
    }

    public bool UseItemFromSlot(object sender, IInventorySlot slot)
    {
        var item = slot.item;

        if (item == null || slot.isEmpty)
        {
            Debug.Log("No available items");
            return false;
        }

        RemoveFromSlot(this, slot);
        return true;
    }

    public bool EquipItem(object sender, IInventoryItem item)
    {
        var desiredItem = GetItem(item.ID);

        if (desiredItem == null)
            return false;

        desiredItem.state.isEquipped = true;
        return true;
    }

    public bool EquipItemFromSlot(object sender, IInventorySlot slot)
    {
        var item = slot.item;

        if (slot.isEmpty || !item.info.canEquipped)
            return false;

        item.state.isEquipped = true;
        return true;
    }

    public bool UnequipItem(object sender, IInventoryItem item)
    {
        if (!item.state.isEquipped)
            return false;

        item.state.isEquipped = false;
        return true;
    }

    public bool UnequipItemFromSlot(object sender, IInventorySlot slot)
    {
        var item = slot.item;

        if (slot.isEmpty || !item.state.isEquipped)
            return false;

        item.state.isEquipped = false;
        return true;
    }
}