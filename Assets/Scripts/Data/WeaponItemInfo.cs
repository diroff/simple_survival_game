using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new WeaponInfo")]
public class WeaponItemInfo : InventoryItemInfo
{
    [SerializeField] private int _damage;
    [SerializeField] private int _magazineCapacity;
    [SerializeField] private float _delay;
    [SerializeField] private AmmoItemInfo _ammo;
    [SerializeField] private Sprite _sprite;

    public int Damage => _damage;
    public int MagazineCapacity => _magazineCapacity;
    public float Delay => _delay;
    public AmmoItemInfo Ammo => _ammo;
    public Sprite Sprite => _sprite;
}