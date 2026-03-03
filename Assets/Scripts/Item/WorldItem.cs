using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [ItemCodeDescription]
    [SerializeField]
    // a unity vai trasnformar esse "_itemCode" no inspector como "Item Code"
    private int _itemCode;

    private SpriteRenderer spriteRenderer;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if(ItemCode != 0)
        {
            Init(ItemCode);
        }
    }

    public void Init(int itemCodeParam)
    {
        if (itemCodeParam != 0)
        {
            ItemCode = itemCodeParam;
            ItemsData itemDetails = InventoryManager.Instance.GetItemData(itemCodeParam);

            spriteRenderer.sprite = itemDetails.itemSprite;

            if (itemDetails.itemType == ItemType.Reapleabol_scenary)
            {
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }
}

