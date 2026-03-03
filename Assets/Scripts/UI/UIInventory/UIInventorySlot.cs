using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GameObject draggedItem; 

    public TextMeshProUGUI tmpUGUI;
    public Image inventorySlotImage;    
    public Image inventorySlotHighlight;

    [HideInInspector] public bool isSelected = false;

    [SerializeField] private UIInventoryToolTip tooltipPrefab = null;
    private UIInventoryToolTip tooltipInstance;
    [SerializeField] private UIInventoryBar inventoryBar = null;
    [HideInInspector] public ItemsData itemDetails;
    [SerializeField] private GameObject itemPrefab = null;
    [HideInInspector] public int itemQuantity;
    private int slotNumber;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        // Atribui o número do slot com base na hierarquia dos objetos
        slotNumber = transform.GetSiblingIndex();
    }


    private void Start()
    {
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            Player.Instance.DisablePlayerInputAndResetMovement();

            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            SetSelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                ClearSelectedItem();
            }

            else
            {
                if (itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }

            Player.Instance.EnablePlayerInput();
        }
    }

    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            WorldItem item = itemGameObject.GetComponent<WorldItem>();
            item.ItemCode = itemDetails.itemCode;

            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

            // If no more of item then clear selected
            // este foi o ultimo método criado que se tratava de highlight, e foi apenas com ele que o highlight foi desselecionado ao ter o estoque do item zerado
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
            {
                ClearSelectedItem();
            }
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemQuantity != 0)
        {

            // Instantiate inventory text box
            tooltipInstance = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
            tooltipInstance.transform.SetParent(parentCanvas.transform, false);

            tooltipInstance.SetTextboxText(
                itemDetails.itemDescription,
                InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType),
                "",
                "",
                itemDetails.itemLongDescription,
                ""
            );

            // Set text box position according to inventory bar position
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                tooltipInstance.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                tooltipInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else
            {
                tooltipInstance.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                tooltipInstance.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance.gameObject);
            tooltipInstance = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // if left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // deselect if selected
            if (isSelected)
            {
                ClearSelectedItem();
            }
            else
            {
                if(itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    /// <summary>
    /// Sets this inventory slot item to be selected
    /// </summary>
    private void SetSelectedItem()
    {
        // Clear currently highlighted items
        inventoryBar.ClearHighlightOnInventorySlots();

        // Highlight item on inventory bar
        isSelected = true;

        // Set highlighted inventory slots
        inventoryBar.SetHighlightedInventorySlots();

        // Set item selected in inventory
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);

        if (itemDetails.canBeCarried)
        {
            Player.Instance.ShowCarriedItem(itemDetails.itemCode);
        }
        else
        {
            Player.Instance.ClearCarriedItem();
        }
    }

    private void ClearSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();

        // esse parece ser meio inutil
        // o que vc acha copilot?
        // Sim, parece ser meio redundante, já que a função ClearHighlightOnInventorySlots() já lida com a lógica de limpar os destaques. No entanto, manter a variável isSelected como false pode ser útil para garantir que o estado do slot seja consistente, especialmente se houver outras partes do código que dependam dessa variável para verificar se um item está selecionado ou não. Portanto, embora possa parecer um pouco redundante, pode ajudar a evitar bugs ou inconsistências no futuro.
        isSelected = false;

        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);

        Player.Instance.ClearCarriedItem();
    }

    /*
     * Dessa maneira dá o seguinte erro: NullReferenceException: Object reference not set to an instance of an object
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tooltipInstance.gameObject);
    }
    */
}
