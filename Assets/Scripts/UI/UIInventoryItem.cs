using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : UIItem
{
    [SerializeField] private Image _imageIcon;
    [SerializeField] private TextMeshProUGUI _textAmount;
    [SerializeField] private Image _equippedIndicator;

    public IInventoryItem item { get; private set; }

    public void Refresh(IInventorySlot slot)
    {
        if (slot.isEmpty || slot == null)
        {
            _equippedIndicator.gameObject.SetActive(false);
            CleanUp();
            return;
        }

        item = slot.item;
        _imageIcon.gameObject.SetActive(true);
        _imageIcon.sprite = item.info.icon;

        var textAmountEnabled = slot.amount > 1;
        _textAmount.gameObject.SetActive(textAmountEnabled);

        if(textAmountEnabled)
            _textAmount.text = "x" + slot.amount.ToString();

        if (item.state.isEquipped)
            _equippedIndicator.gameObject.SetActive(true);
        else
            _equippedIndicator.gameObject.SetActive(false);
    }

    private void CleanUp()
    {
        _imageIcon.gameObject.SetActive(false);
        _textAmount.gameObject.SetActive(false);
    }
}
