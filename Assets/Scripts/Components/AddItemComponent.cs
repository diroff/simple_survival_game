using UnityEngine;

public class AddItemComponent : MonoBehaviour
{
    [SerializeField] private Item _item;

    public void AddItem(GameObject target)
    {
        var player = target.GetComponent<Player>();
        
        if (player != null)
            player.AddItem(this, _item.ItemData);
    }
}