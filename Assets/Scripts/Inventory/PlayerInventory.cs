using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private int _capacity;

    public InventoryWithSlots inventory { get; private set; }
    public Player Player { get; private set; }
    public UIInventory UIInventory => uiInventory;

    private UIInventorySlot[] _uiSlots;
    
    private void Start()
    {
        inventory = new InventoryWithSlots(_capacity);
        inventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;

        _uiSlots = uiInventory.uiSlots;
        FillSlots();

        Player = GetComponent<Player>();

        _inventory.SetActive(false);
    }

    public void FillSlots()
    {
        SetUpInventoryUI(inventory);
    }

    private void SetUpInventoryUI(InventoryWithSlots inventory)
    {
        var allSlots = inventory.GetAllSlots();
        var allSlotsCount = allSlots.Length;

        for (int i = 0; i < allSlotsCount; i++)
        {
            var slot = allSlots[i];
            var uiSlot = _uiSlots[i];
            uiSlot.SetSlot(slot);
            uiSlot.Refresh();
        }
    }

    public void AddItem(object sender, IInventorySlot slot, IInventoryItem item)
    {
        inventory.TryToAddToSlot(this, slot, item);
    }

    public void UseItem()
    {
        OnInventoryStateChanged(this);
    }

    private void OnInventoryStateChanged(object sender)
    {
        foreach (var uiSlot in _uiSlots)
        {
            uiSlot.Refresh();
        }
    }
}