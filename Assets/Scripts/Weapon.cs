using UnityEngine;

public class Weapon : ItemInEquipment
{
    [SerializeField] private SpriteRenderer _weaponSprite;
    [SerializeField] private PlayerInventory _playerInventory;

    [SerializeField] private GameObject _bulletSpawnPoint;

    private WeaponItemInfo _weaponItemInfo;
    private IInventoryItem _weaponItemData;
    private float _timeFromLastShoot;
    private int _bulletCount;

    public WeaponItemInfo WeaponData => _weaponItemInfo;
    public int BulletCount => _bulletCount;

    private void Update()
    {
        if (_weaponItemInfo == null)
            return;

        TimeChecker();
    }

    public void SetWeapon(IInventoryItem itemData)
    {
        if(itemData == null)
        {
            UnequipWeapon();
            return;
        }

        if (_weaponItemInfo != null)
            UnloadWeapon();


        _weaponItemData = itemData;
        _weaponItemInfo = _weaponItemData.info as WeaponItemInfo;

        _bulletCount = 0;
        _weaponSprite.sprite = _weaponItemInfo.sprite;
        _timeFromLastShoot = _weaponItemInfo.Delay;
    }

    private void UnequipWeapon()
    {
        UnloadWeapon();

        _weaponItemData = null;
        _weaponItemInfo = null;
        _bulletCount = 0;
        gameObject.SetActive(false);
    }

    private void UnloadWeapon()
    {
        if (_bulletCount > 0)
        {
            var ammo = new ItemData(_weaponItemInfo.BulletPrefab.AmmoInfo);
            ammo.state.amount = _bulletCount;

            _playerInventory.inventory.TryToAdd(this, ammo);
        }
    }

    public void Shoot(Vector3 direction)
    {
        if (_weaponItemInfo == null)
        {
            Debug.Log("No weapon item info");
            return;
        }

        if (!ShootingDelayOver())
            return;

        if (IsMagazineEmpty())
        {
            Debug.Log("Empty magazine!");
            return;
        }

        _timeFromLastShoot = 0f;
        _bulletCount--;

        CreateBullet(direction);

        Debug.Log("Ammo:" + _bulletCount + "/" + _weaponItemInfo.MagazineCapacity);
    }

    private void CreateBullet(Vector3 direction)
    {
        var bullet = Instantiate(_weaponItemInfo.BulletPrefab, _bulletSpawnPoint.transform.position, Quaternion.identity);

        bullet.SetDamage(_weaponItemInfo.Damage);
        bullet.SetPower(_weaponItemInfo.ShootingPower);
        bullet.SetupAmmoInfo();

        bullet.Move(direction);
    }

    private bool ShootingDelayOver()
    {
        return _timeFromLastShoot >= _weaponItemInfo.Delay;
    }

    private bool IsMagazineEmpty()
    {
        return _bulletCount <= 0;
    }

    private bool IsMagazineFull()
    {
        return _bulletCount == _weaponItemInfo.MagazineCapacity;
    }

    public void Reload()
    {
        if (_weaponItemInfo == null)
            return;

        if (IsMagazineFull())
        {
            Debug.Log("Magazine is full");
            return;
        }

        var ammoCount = _playerInventory.inventory.GetItemAmount(_weaponItemInfo.BulletPrefab.AmmoInfo.id);

        if (ammoCount == 0)
        {
            Debug.Log("No ammo!");
            return;
        }

        var reloadedAmmoAmount = _bulletCount + ammoCount >= _weaponItemInfo.MagazineCapacity ? _weaponItemInfo.MagazineCapacity - _bulletCount : ammoCount;

        _bulletCount += reloadedAmmoAmount;
        _playerInventory.inventory.Remove(this, _weaponItemInfo.BulletPrefab.AmmoInfo.id, reloadedAmmoAmount);

        Debug.Log("Ammo:" + _bulletCount + "/" + _weaponItemInfo.MagazineCapacity);
    }

    private void TimeChecker()
    {
        if (_timeFromLastShoot < _weaponItemInfo.Delay)
            _timeFromLastShoot += Time.deltaTime;
    }
}