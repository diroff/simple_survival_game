using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemDataForInspector> _containedItems;

    public InventoryWithSlots Inventory { get; private set; }

    private void Start()
    {
        Inventory = new InventoryWithSlots(10);

        foreach (ItemDataForInspector data in _containedItems)
            AddItem(this, data.GetData());

        //Create UI
    }

    public void AddItem(object sender, ItemData item)
    {
        Inventory.TryToAdd(sender, item);
    }

    public void Interact()
    {
        Open();
    }

    public void Open()
    {
        LookAtItems();
    }

    public void LookAtItems()
    {
        var items = Inventory.GetAllItems();

        for (int i = 0; i < items.Length; i++)
        {
            Debug.Log($"{i + 1}. {items[i].ID} - {items[i].state.amount}");
        }
    }
}