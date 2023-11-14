using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventoryTester;
    [SerializeField] private UIInventorySlot _uiInventorySlotPrefab;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    [SerializeField] private GameObject _uiArmorSlots;
    [SerializeField] private GridLayoutGroup _armorGridLayoutGroup;
    [SerializeField] private GameObject _uiEquipmentSlots;
    [SerializeField] private GridLayoutGroup _equippedGridLayoutGroup;

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _descriptionPanel;
    [SerializeField] private GameObject _actionsPanel;
 
    [SerializeField] private TextMeshProUGUI _UIItemTitleText;
    [SerializeField] private TextMeshProUGUI _UIItemDescriptionText;
    [SerializeField] private Image _uiItemIcon;

    public InventoryWithSlots inventory => inventoryTester.inventory;
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
        for (int i = 0; i < inventory.capacity - inventoryTester.EquipmentSlotsCount - inventoryTester.ArmorSLotsCount; i++)
        {
            Instantiate(_uiInventorySlotPrefab, transform);
        }

        for (int i = 0; i < inventoryTester.EquipmentSlotsCount; i++)
        {
            Instantiate(_uiInventorySlotPrefab, _uiEquipmentSlots.transform);
        }

        for (int i = 0; i < inventoryTester.ArmorSLotsCount; i++)
        {
            Instantiate(_uiInventorySlotPrefab, _uiArmorSlots.transform);
        }

        uiSlots = GetComponentsInChildren<UIInventorySlot>();

        var armorUISlots = _uiArmorSlots.GetComponentsInChildren<UIInventorySlot>();
        var equipmentUISlots = _uiEquipmentSlots.GetComponentsInChildren<UIInventorySlot>();

        UIInventorySlot[] allSlots = new UIInventorySlot[uiSlots.Length + armorUISlots.Length + equipmentUISlots.Length];

        allSlots = uiSlots.Concat(equipmentUISlots.Concat(armorUISlots)).ToArray();

        uiSlots = allSlots;

        foreach (var slot in uiSlots)
        {
            slot.SetUIInventory(this);
        }

        StartCoroutine(GridSorting());
    }

    private IEnumerator GridSorting()
    {
        _gridLayoutGroup.enabled = true;
        _armorGridLayoutGroup.enabled = true;
        _equippedGridLayoutGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        _gridLayoutGroup.enabled = false;
        _armorGridLayoutGroup.enabled = false;
        _equippedGridLayoutGroup.enabled = false;
    }

    private void OnEnable()
    {
        if(inventory != null)
            inventoryTester.inventory.OnInventoryStateChangedEvent += Inventory_OnInventoryStateChangedEvent;
    }

    private void OnDisable()
    {
        inventoryTester.inventory.OnInventoryStateChangedEvent -= Inventory_OnInventoryStateChangedEvent;
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

        inventoryTester.EquipItem(_currentSlot);
        
        _curentSlotUI.UIInventoryItem.Refresh(_currentSlot);
        RefreshItemDescriptionPanel();
    }

    public void UnequipItem()
    {
        inventoryTester.UnequipItem(_currentSlot.item);
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
            inventoryTester.UnequipItem(_currentSlot.item);

        inventoryTester.inventory.RemoveFromSlot(this, _currentSlot, _currentSlot.item.state.amount);
        _currentSlot = null;
        RefreshItemDescriptionPanel();
    }

    public void InteractWithInventory()
    {
        if (_inventory.activeSelf)
            CloseInventory();
        else
            OpenInventory();
    }

    public void OpenInventory()
    {
        _inventory.SetActive(true);

        if (_curentSlotUI == null)
            return;

        _curentSlotUI.CancelSelect();
        _curentSlotUI = null;
        _currentSlot = null;
        RefreshItemDescriptionPanel();
    }

    public void CloseInventory()
    {
        _inventory.SetActive(false);
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

        _UIItemTitleText.text = _currentSlot.item.info.title;
        _UIItemDescriptionText.text = _currentSlot.item.info.description;
        _uiItemIcon.sprite = _currentSlot.item.info.icon;
    }

    public void DisableItemDescriptionPanel()
    {
        _descriptionPanel.SetActive(false);
        _actionsPanel.SetActive(false);
    }
}