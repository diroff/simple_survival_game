using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new HealItemInfo")]
public class HealItemData : InventoryItemInfo
{
    [SerializeField] private int _healthPower;

    public int HealthPower => _healthPower;
}
