using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Item> _containedItems;

    public InventoryWithSlots Inventory { get; private set; }

    private Item _item;

    private void Start()
    {
        Inventory = new InventoryWithSlots(10);

        _item = Instantiate(_containedItems[0]);

        foreach (var item in _containedItems)
        {
            AddItem(this, item);
        }

        Destroy(_item.gameObject);

        //Create UI
    }

    public void AddItem(object sender, Item item)
    {
        _item.SetInfo(item.Info as InventoryItemInfo);
        _item.SetState(item.State as InventoryItemState);

        Inventory.TryToAdd(this, _item.ItemData);
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