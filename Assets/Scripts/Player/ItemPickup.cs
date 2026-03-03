using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldItem item = collision.GetComponent<WorldItem>();

        if (item != null)
        {
            // Get item details
            ItemsData itemDetails = InventoryManager.Instance.GetItemData(item.ItemCode);

            if (itemDetails.canBePickedUp)
            {
                InventoryManager.Instance.AddItem(InventoryLocation.player, item, collision.gameObject);


            }
        }
    }
}
