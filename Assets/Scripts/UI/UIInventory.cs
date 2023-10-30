using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventoryTester;
    [SerializeField] private UIInventorySlot _uiInventorySlotPrefab;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _descriptionPanel;
 
    [SerializeField] private TextMeshProUGUI _UIItemTitleText;
    [SerializeField] private TextMeshProUGUI _UIItemDescriptionText;

    [SerializeField] private Button _useItemButton;
    [SerializeField] private Button _dropItemButton;

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
        for (int i = 0; i < inventory.capacity; i++)
        {
            Instantiate(_uiInventorySlotPrefab, transform);
        }

        uiSlots = GetComponentsInChildren<UIInventorySlot>();

        StartCoroutine(GridSorting());
    }

    private IEnumerator GridSorting()
    {
        _gridLayoutGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        _gridLayoutGroup.enabled = false;
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

        if(_currentSlot.item.state.isEquipped)
            return;

        //inventoryTester.Player.UseItem(_currentSlot.item);
        inventoryTester.EquipItem(_currentSlot);
        
        _curentSlotUI.UIInventoryItem.Refresh(_currentSlot);
        RefreshItemDescriptionPanel();
    }

    public void DropItem()
    {
        if (_currentSlot.isEmpty)
        {
            RefreshItemDescriptionPanel();
            return;
        }

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

        _UIItemTitleText.text = _currentSlot.item.info.title;
        _UIItemDescriptionText.text = _currentSlot.item.info.description;

        bool showUseButton = _currentSlot.item.state.isEquipped ? true: false;

        _useItemButton.gameObject.SetActive(!showUseButton);
        _dropItemButton.gameObject.SetActive(true);
    }

    public void DisableItemDescriptionPanel()
    {
        _descriptionPanel.SetActive(false);

        _useItemButton.gameObject.SetActive(false);
        _dropItemButton.gameObject.SetActive(false);
    }
}