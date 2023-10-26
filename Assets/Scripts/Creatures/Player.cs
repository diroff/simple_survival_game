using UnityEngine;

public class Player : Creature
{
    [Header("Data")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private Weapon _weapon;

    protected override void Update()
    {
        base.Update();
        GetInput();

        if (_weapon != null && Input.GetKeyDown(KeyCode.Space))
            _weapon.Shoot();

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

    public void UseItem(IInventoryItem item)
    {
        if (item.info.canEquipped)
        {
            EquipItem(item);
            return;
        }

        switch (item.info.type)
        {
            case InventoryItemType.heal:
                Debug.Log("Tasty! + health from " + item.info.title);
                break;

            default:
                Debug.Log("Can't be used!");
                break;
        }
    }

    private void GetInput()
    {
        InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    internal void EquipItem(IInventoryItem item)
    {
        SetWeapon(item);
    }
}