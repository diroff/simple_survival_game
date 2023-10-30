using UnityEngine;

public class Player : Creature
{
    [Header("Data")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private ItemInEquipment _equippedItem;
    [SerializeField] private Weapon _weapon;

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
        switch (item.info.type)
        {
            case InventoryItemType.heal:
                Heal(item);
                break;

            case InventoryItemType.weapon:
                Shoot(item);
                break;

            default:
                Debug.Log("Can't be used!");
                break;
        }
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
        if (_equippedItem != null)
        {
            _inventory.UnequipItem(_equippedItem.ItemData);
            UnequipItem(item);
        }

        _equippedItem.SetItem(item);
    }

    public void UnequipItem(IInventoryItem item)
    {
        if (_weapon.isActiveAndEnabled)
            UnequipWeapon();
    }

    private void UnequipWeapon()
    {
        _weapon.SetWeapon(null);
    }
}