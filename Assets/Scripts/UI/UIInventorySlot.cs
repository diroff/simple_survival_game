using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : UISlot
{
    [SerializeField] private UIInventoryItem _uiInventoryItem;
    [SerializeField] private Image _activeHighlighter;

    public IInventorySlot slot { get; private set; }
    public UIInventoryItem UIInventoryItem => _uiInventoryItem;

    private UIInventory _uiInventory;

    public void SetUIInventory(UIInventory uiInventory)
    {
        _uiInventory = uiInventory;
    }

    public void SetSlot(IInventorySlot newSlot)
    {
        slot = newSlot;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        var otherSlot = otherSlotUI.slot;
        var inventory = _uiInventory.inventory;

        inventory.TransitFromSlotToSlot(this, otherSlot, slot);

        Refresh();
        otherSlotUI.Refresh();
        _uiInventory.ChooseItem(slot, this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        _uiInventory.ChooseItem(slot, this);
    }

    public void Refresh()
    {
        _uiInventoryItem.Refresh(slot);
    }

    public void Select()
    {
        _activeHighlighter.gameObject.SetActive(true);
    }

    public void CancelSelect()
    {
        _activeHighlighter.gameObject.SetActive(false);
    }
}