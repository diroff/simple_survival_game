using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new ItemInfo")]
public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
{
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    
    [SerializeField] private InventoryItemType _itemType;
    
    [SerializeField] private int _maxItemsInInventorySlot;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _sprite;

    public string id => _id;

    public string title => _title;

    public string description => _description;

    public int maxItemsInInventorySlot => _maxItemsInInventorySlot;

    public Sprite icon => _icon;

    public InventoryItemType type => _itemType;

    public Sprite sprite => _sprite;
}