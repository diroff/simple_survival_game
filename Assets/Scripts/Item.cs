using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Item : MonoBehaviour
{
    [SerializeField] protected InventoryItemInfo _info;
    [SerializeField] protected InventoryItemState _state;

    protected ItemData itemData;
    protected SpriteRenderer spriteRenderer;

    public ItemData ItemData => itemData;

    public IInventoryItemInfo Info => _info;
    public IInventoryItemState State => _state;

    private void Awake()
    {
        CreateItemData();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (spriteRenderer.sprite == null)
            UpdateSprite();
    }

    private void CreateItemData()
    {
        itemData = new ItemData(_info);
        itemData.state = _state;
    }

    public void UpdateSprite()
    {
        if(itemData.info == null)
        {
            spriteRenderer.sprite = null;
            return;
        }

        spriteRenderer.sprite = itemData.info.sprite;
    }

    public virtual void SetItem(IInventoryItem item)
    {
        _info = item.info as InventoryItemInfo;
        _state = item.state as InventoryItemState;

        itemData.info = _info;
        itemData.state = _state;

        UpdateSprite();
    }

    public void SetInfo(InventoryItemInfo info)
    {
        _info = info;
    }

    public void SetState(InventoryItemState state)
    {
        _state = state;
    }
}