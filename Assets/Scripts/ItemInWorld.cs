using UnityEngine;

public class ItemInWorld : Item
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        var player = collision.gameObject.GetComponent<Player>();

        if (player == null)
            return;

        if (player.AddItem(this, itemData))
            Destroy(gameObject);
        else
            Debug.Log("Can't added");
    }
}