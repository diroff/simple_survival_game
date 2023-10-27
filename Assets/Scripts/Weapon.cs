using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSprite;
    [SerializeField] private PlayerInventory _playerInventory;

    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _bulletSpawnPoint;

    private WeaponItemInfo _weaponItemInfo;
    private IInventoryItem _weaponItemData;
    private float _timeFromLastShoot;
    private int _bulletCount;

    public WeaponItemInfo WeaponData => _weaponItemInfo;
    public IInventoryItem ItemData => _weaponItemData;
    public int BulletCount => _bulletCount;

    private void Update()
    {
        if (_weaponItemInfo == null)
            return;

        TimeChecker();
    }

    public void SetWeapon(IInventoryItem itemData)
    {
        if (_weaponItemInfo != null)
        {
            _weaponItemData.state.isEquipped = false;
            _playerInventory.UseItem();

            if (_bulletCount > 0)
            {
                var ammo = new ItemData(_weaponItemInfo.Ammo);
                ammo.state.amount = _bulletCount;

                _playerInventory.inventory.TryToAdd(this, ammo);
            }
        }

        _weaponItemData = itemData;
        _weaponItemInfo = _weaponItemData.info as WeaponItemInfo;

        _bulletCount = 0;
        _weaponSprite.sprite = _weaponItemInfo.Sprite;
        _timeFromLastShoot = _weaponItemInfo.Delay;
    }

    public void Shoot(Vector3 direction)
    {
        if (_weaponItemInfo == null)
            return;

        if (_timeFromLastShoot < _weaponItemInfo.Delay)
            return;

        if (_bulletCount <= 0)
        {
            Debug.Log("Empty magazine!");
            return;
        }

        _timeFromLastShoot = 0f;
        _bulletCount--;

        var bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.transform.position, Quaternion.identity);
        bullet.Move(direction);

        Debug.Log("Ammo:" + _bulletCount + "/" + _weaponItemInfo.MagazineCapacity);
    }

    public void Reload()
    {
        if (_weaponItemInfo == null)
            return;

        if (_bulletCount == _weaponItemInfo.MagazineCapacity)
        {
            Debug.Log("Magazine is full");
            return;
        }

        var ammoCount = _playerInventory.inventory.GetItemAmount(_weaponItemInfo.Ammo.id);

        if (ammoCount == 0)
        {
            Debug.Log("No ammo!");
            return;
        }

        var reloadedAmmoAmount = _bulletCount + ammoCount >= _weaponItemInfo.MagazineCapacity ? _weaponItemInfo.MagazineCapacity - _bulletCount : ammoCount;

        _bulletCount += reloadedAmmoAmount;
        _playerInventory.inventory.Remove(this, _weaponItemInfo.Ammo.id, reloadedAmmoAmount);

        Debug.Log("Ammo:" + _bulletCount + "/" + _weaponItemInfo.MagazineCapacity);
    }

    private void TimeChecker()
    {
        if (_timeFromLastShoot < _weaponItemInfo.Delay)
            _timeFromLastShoot += Time.deltaTime;
    }
}