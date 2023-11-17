using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemDataForInspector _itemData;

    protected ItemData itemData;
    protected SpriteRenderer spriteRenderer;

    public ItemData ItemData => itemData;

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
        itemData = _itemData.GetData();
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
        itemData.info = item.info as InventoryItemInfo;
        itemData.state = item.state as InventoryItemState;

        UpdateSprite();
    }

    public void SetInfo(InventoryItemInfo info)
    {
        itemData.info = info;
    }

    public void SetState(InventoryItemState state)
    {
        itemData.state = state;
    }
}