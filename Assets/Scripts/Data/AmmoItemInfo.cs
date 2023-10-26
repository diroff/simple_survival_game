using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new AmmoInfo")]
public class AmmoItemInfo : InventoryItemInfo
{
    [SerializeField] private int _damage;

    public int Damage => _damage;
}