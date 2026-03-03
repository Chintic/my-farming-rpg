using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehavior<InventoryManager>
{
    private Dictionary<int, ItemsData> itemDataDictionary;

    private int[] selectedInventoryItem;

    // seria bom ser private
    // a unity não serializa array de listas
    public List<InventoryItem>[] inventoriesByLocation;

    // é um array paralelo ao inventoryLists que irá usar como indice dentro dos [] a posição do inventário em questão do Enum InventoryLocation
    // melhor ser private   
    [HideInInspector] public int[] inventoryCapacityByLocation;

    // usado apenas para alimentar o dictionary de item details. 
    [SerializeField] private SO_ItemList itemList = null;

    protected override void Awake()
    {
        base.Awake();
        CreateInventoryLists();
        // Create item details dictionary (em Awake para estar pronto antes dos Itens usarem em Start)
        CreateItemDataDictionary();

        selectedInventoryItem = new int[(int)InventoryLocation.count];

        for (int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
            // é -1 para garantir que nenhum item esteja selecionado no início, já que o indice dos itens começa em 0. Assim, se for -1, sabemos que não tem item selecionado.
        }
    }

    private void CreateInventoryLists()
    {
        // Cria uma lista de listas em branco para receber depois as atribuições
        inventoriesByLocation = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < inventoriesByLocation.Length /* tamanho esse já definido acima*/; i++)
        {
            inventoriesByLocation[i] = new List<InventoryItem>();
        }

        // Já começa a definir o array que receberá o tamanho de cada lista de mesma posição
        inventoryCapacityByLocation = new int[(int)InventoryLocation.count];

        // no caso só estamos alimentando o inventário do player, mas a ideia é que cada tipo de inventário tenha uma capacidade diferente. Mas isso será atribuido depois
        inventoryCapacityByLocation[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    /// <summary>
    /// Populates the itemDataDictionary from the scriptable object item list
    /// </summary>

    private void CreateItemDataDictionary()
    {
        itemDataDictionary = new Dictionary<int, ItemsData>();

        foreach(ItemsData itemData in itemList.ItemData)
        {
            itemDataDictionary.Add(itemData.itemCode, itemData);
        }
    }
    
    public void AddItem(InventoryLocation inventoryLocation, WorldItem item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);
        Destroy(gameObjectToDelete);
    }

    /// <summary>
    /// Add an Item to the inventory list for the inventory location
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, WorldItem item)
    {
        int itemCode = item.ItemCode;

        // Aqui estamos usando a lista original. É uma referencia, não uma copia. Criamos esse objeto para ter uma referência mais curta, para não precisar ficar escrevendo toda hora inventoriesByLocation[(int)inventoryLocation]
        List<InventoryItem> inventoryList = inventoriesByLocation[(int)inventoryLocation];

        // Check if inventory already contain the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }

        // Send event that inventory has been updated
        EventHandler.CallInventoryUpdateEvent(inventoryLocation, inventoriesByLocation[(int)inventoryLocation]);
    }

    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoriesByLocation[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if(itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        EventHandler.CallInventoryUpdateEvent(inventoryLocation, inventoriesByLocation[(int)inventoryLocation]);
    }

    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[itemPosition].itemQuantity - 1;

        if (quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(itemPosition);
        }
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);

        //DebugPrintInventoryList(inventoryList);
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryList[position] = inventoryItem;

        //DebugPrintInventoryList(inventoryList);
    }

    private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        foreach(InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log("Item description: " + InventoryManager.Instance.GetItemData(inventoryItem.itemCode).itemDescription + " Item Quantity: " + inventoryItem.itemQuantity);
        }

        Debug.Log("################################################################################");
    }

    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoriesByLocation[(int)inventoryLocation];

        for (int i = 0; i< inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }

        }
        return -1;
    }

    /// <summary>
    /// Returns the itemData (from the SO_itemList) for the itemCode, or null if the item code doesn't exist
    /// </summary>

    public ItemsData GetItemData(int itemCode)
    {
        ItemsData itemData;

        if (itemDataDictionary.TryGetValue(itemCode, out itemData))
        {
            return itemData;
        }
        else
        {
            return null;
        }
    }

    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool;
                break;

            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool;
                break;

            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool;
                break;

            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool;
                break;

            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool;
                break;

            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool;
                break;

            default:
                itemTypeDescription = itemType.ToString();
                break;
        }

        return itemTypeDescription;
    }

    internal void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
         /* 
         existe uma tratativa para fazer com que ele impeça a tentativa de trocar um item por um indice 
         fora do range da lista, mas eu não fiz ainda para entender o motivo das tratativas, e não quero deixar muito complexo por enquanto.
         E também daria para usar tuplas.
         */

        InventoryItem temp = inventoriesByLocation[(int)inventoryLocation][fromItem];
        inventoriesByLocation[(int)inventoryLocation][fromItem] = inventoriesByLocation[(int)inventoryLocation][toItem];
        inventoriesByLocation[(int)inventoryLocation][toItem] = temp;
        EventHandler.CallInventoryUpdateEvent(inventoryLocation, inventoriesByLocation[(int)inventoryLocation]);
    }

    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }
    
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = -1;
    }
 


}
