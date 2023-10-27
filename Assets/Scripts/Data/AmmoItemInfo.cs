using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new AmmoInfo")]
public class AmmoItemInfo : InventoryItemInfo
{
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;

    public int Damage => _damage;
    public float LifeTime => _lifeTime;
}