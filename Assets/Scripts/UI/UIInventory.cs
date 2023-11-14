using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [Space]

    [Header("Inventory Grid")]
    [SerializeField] UIInventoryGrid _uiItemsGrid;
    [SerializeField] UIInventoryGrid _uiEquippedGrid;
    [SerializeField] UIInventoryGrid _uiArmorGrid;
    [Space]

    [SerializeField] private GameObject _uiInventoryPanel;
    [SerializeField] private GameObject _descriptionPanel;
    [SerializeField] private GameObject _actionsPanel;

    [Header("Description panel")]
    [SerializeField] private TextMeshProUGUI _uiItemTitleText;
    [SerializeField] private TextMeshProUGUI _uiItemDescriptionText;
    [SerializeField] private Image _uiItemIcon;

    public InventoryWithSlots inventory => _playerInventory.inventory;
    public UIInventorySlot[] uiSlots { get; private set; }

    private IInventorySlot _currentSlot;
    private UIInventorySlot _curentSlotUI;

    public void ChooseItem(IInventorySlot slot, UIInventorySlot uiSlot)
    {
        if (_currentSlot != null)
            _curentSlotUI.CancelSelect();

        _currentSlot = slot;
        _curentSlotUI = uiSlot;
        RefreshItemDescriptionPanel();
    }

    public void CreateInventoryUI()
    {
        for (int i = 0; i < inventory.capacity - _playerInventory.EquipmentSlotsCount - _playerInventory.ArmorSLotsCount; i++)
            _uiItemsGrid.CreateSlot();

        for (int i = 0; i < _playerInventory.EquipmentSlotsCount; i++)
            _uiEquippedGrid.CreateSlot();

        for (int i = 0; i < _playerInventory.ArmorSLotsCount; i++)
            _uiArmorGrid.CreateSlot();

        var itemsSlot = _uiItemsGrid.GetComponentsInChildren<UIInventorySlot>();
        var equipmentUISlots = _uiEquippedGrid.GetComponentsInChildren<UIInventorySlot>();
        var armorUISlots = _uiArmorGrid.GetComponentsInChildren<UIInventorySlot>();

        UIInventorySlot[] allSlots = itemsSlot.Concat(equipmentUISlots.Concat(armorUISlots)).ToArray();
        uiSlots = allSlots;

        foreach (var slot in uiSlots)
            slot.SetUIInventory(this);

        StartCoroutine(SortInventory());
    }

    private IEnumerator SortInventory()
    {
        _uiItemsGrid.SortInventory();
        _uiEquippedGrid.SortInventory();
        _uiArmorGrid.SortInventory();

        yield return new WaitForEndOfFrame();

        CloseInventory();
    }

    private void OnEnable()
    {
        if(inventory != null)
            _playerInventory.inventory.OnInventoryStateChangedEvent += Inventory_OnInventoryStateChangedEvent;
    }

    private void OnDisable()
    {
        _playerInventory.inventory.OnInventoryStateChangedEvent -= Inventory_OnInventoryStateChangedEvent;
    }

    private void Inventory_OnInventoryStateChangedEvent(object obj)
    {
        RefreshItemDescriptionPanel();
    }

    public void UseItem()
    {
        if (_currentSlot == null || _currentSlot.isEmpty)
            return;

        if (_currentSlot.item.state.isEquipped)
        {
            UnequipItem();
            _curentSlotUI.UIInventoryItem.Refresh(_currentSlot);
            return;
        }

        _playerInventory.EquipItem(_currentSlot);
        
        _curentSlotUI.UIInventoryItem.Refresh(_currentSlot);
        RefreshItemDescriptionPanel();
    }

    public void UnequipItem()
    {
        _playerInventory.UnequipItem(_currentSlot.item);
        RefreshItemDescriptionPanel();
        _curentSlotUI.UIInventoryItem.Refresh(_currentSlot);
    }

    public void DropItem()
    {
        if (_currentSlot.isEmpty)
        {
            RefreshItemDescriptionPanel();
            return;
        }

        if (_currentSlot.item.state.isEquipped)
            _playerInventory.UnequipItem(_currentSlot.item);

        _playerInventory.inventory.RemoveFromSlot(this, _currentSlot, _currentSlot.item.state.amount);
        _currentSlot = null;
        RefreshItemDescriptionPanel();
    }

    public void InteractWithInventory()
    {
        if (_uiInventoryPanel.activeSelf)
            CloseInventory();
        else
            OpenInventory();
    }

    public void OpenInventory()
    {
        _uiInventoryPanel.SetActive(true);

        if (_curentSlotUI == null)
            return;

        _curentSlotUI.CancelSelect();
        _curentSlotUI = null;
        _currentSlot = null;
        RefreshItemDescriptionPanel();
    }

    public void CloseInventory()
    {
        _uiInventoryPanel.SetActive(false);
    }

    public void RefreshItemDescriptionPanel()
    {
        if(_currentSlot == null)
        {
            DisableItemDescriptionPanel();
            return;
        }

        if (_currentSlot.isEmpty)
        {
            DisableItemDescriptionPanel();
            _curentSlotUI.CancelSelect();
            return;
        }

        _curentSlotUI.Select();
        _descriptionPanel.SetActive(true);
        _actionsPanel.SetActive(true);

        _uiItemTitleText.text = _currentSlot.item.info.title;
        _uiItemDescriptionText.text = _currentSlot.item.info.description;
        _uiItemIcon.sprite = _currentSlot.item.info.icon;
    }

    public void DisableItemDescriptionPanel()
    {
        _descriptionPanel.SetActive(false);
        _actionsPanel.SetActive(false);
    }
}