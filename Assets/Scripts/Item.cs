using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private InventoryItemInfo _info;
    [SerializeField] private InventoryItemState _state;

    private ItemData _itemData; 

    public ItemData ItemData => _itemData;

    private void Awake()
    {
        _itemData = new ItemData(_info);
        _itemData.state = _state;
    }

    private void Start()
    {
        if (_info != null)
            _itemData.info = _info;

        if(_state != null)
            _itemData.state = _state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        var player = collision.gameObject.GetComponent<Player>();

        if (player == null)
            return;

        if (player.AddItem(this, _itemData))
            Destroy(gameObject);
        else
            Debug.Log("Can't added");
    }
}