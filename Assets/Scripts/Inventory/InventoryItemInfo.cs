using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new ItemInfo")]
public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
{
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    
    [SerializeField] private InventoryItemType _itemType;
    [SerializeField] private bool _canUsed;
    [SerializeField] private bool _canEquipped;
    
    [SerializeField] private int _maxItemsInInventorySlot;
    [SerializeField] private Sprite _icon;

    public string id => _id;

    public string title => _title;

    public string description => _description;

    public int maxItemsInInventorySlot => _maxItemsInInventorySlot;

    public Sprite icon => _icon;

    public InventoryItemType type => _itemType;

    public bool canUsed => _canUsed;

    public bool canEquipped => _canEquipped;
}