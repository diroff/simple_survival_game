using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new WeaponInfo")]
public class WeaponItemInfo : InventoryItemInfo
{
    [SerializeField] private int _damage;
    [SerializeField] private int _magazineCapacity;
    [SerializeField] private float _delay;
    [SerializeField] private float _shootingPower;
    [SerializeField] private Bullet _bulletPrefab;

    public int Damage => _damage;
    public int MagazineCapacity => _magazineCapacity;
    public float Delay => _delay;
    public float ShootingPower => _shootingPower;
    public Bullet BulletPrefab => _bulletPrefab;
}