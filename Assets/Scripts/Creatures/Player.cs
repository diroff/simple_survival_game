using UnityEngine;
using UnityEngine.Events;

public class Player : Creature
{
    [Header("Data")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private ItemInEquipment _equippedItem;
    [SerializeField] private Weapon _weapon;

    private delegate void UseEquippedItem(IInventoryItem item);

    private UseEquippedItem _useEquippedItem;

    protected override void Update()
    {
        base.Update();
        GetInput();

        if(_equippedItem != null && Input.GetKeyDown(KeyCode.Space))
        {
            UseItem(_equippedItem.ItemData);
        }

        if (Input.GetKeyDown(KeyCode.R))
            _weapon.Reload();
    }

    public void SetWeapon(IInventoryItem itemData)
    {
        _weapon.SetWeapon(itemData);
    }

    public bool AddItem(object sender, IInventoryItem item)
    {
        return _inventory.inventory.TryToAdd(sender, item);
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
    }

    public void UseItem(IInventoryItem item)
    {
        _useEquippedItem(item);

        if (item.info == null)
            return;

        if(item.state.amount <= 0)
            UnequipItem();
    }

    private void Heal(IInventoryItem item)
    {
        HealItemData healItemData = item.info as HealItemData;

        HealthComponent.ModifyHealth(healItemData.HealthPower);
        _inventory.inventory.Remove(this, item.ID);
    }

    private void Shoot(IInventoryItem item)
    {
        if (_weapon.WeaponData == null)
            WeaponPreparing(item);

        Vector3 direction = NormalSprite ? Vector3.right : Vector3.left;
        _weapon.Shoot(direction);
    }

    private void UseNothing(IInventoryItem item)
    {
        Debug.Log("No items in my hands");
    }

    private void WeaponPreparing(IInventoryItem item)
    {
        _weapon.gameObject.SetActive(true);
        _weapon.SetWeapon(item);
    }

    private void GetInput()
    {
        InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void EquipItem(IInventoryItem item)
    {
        _equippedItem.gameObject.SetActive(true);

        if (_equippedItem.ItemData.info != null)
            _inventory.UnequipItem(_equippedItem.ItemData);

        _equippedItem.gameObject.SetActive(true);
        _equippedItem.SetItem(item);

        switch (item.info.type)
        {
            case InventoryItemType.heal:
                _useEquippedItem = Heal;
                break;

            case InventoryItemType.weapon:
                _useEquippedItem = Shoot;
                WeaponPreparing(item);
                break;

            default:
                _useEquippedItem = UseNothing;
                break;
        }
    }

    public void UnequipItem()
    {
        if (_weapon.isActiveAndEnabled)
            UnequipWeapon();

        _equippedItem.SetItem(null);
        _useEquippedItem = UseNothing;
        _equippedItem.gameObject.SetActive(false);
    }

    private void UnequipWeapon()
    {
        _weapon.SetWeapon(null);
    }
}