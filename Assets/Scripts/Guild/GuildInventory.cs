using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildInventory : MonoBehaviour
{
    private static GuildInventory _instance;
    public static GuildInventory Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject inventoryObject;
    public GameObject itemObjectPrefab;
    public List<ItemObject> itemObjects;

    public delegate void InventoryChangedDelegate();
    public InventoryChangedDelegate InventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
    }


    public ItemObject AddItemToInventory(Item item, int quant = 1)
    {
        if (quant <= 0)
            return null;
        ItemObject itemObject = FindItem(item);
        if (itemObject == null)
        {
            GameObject newItem = Instantiate(itemObjectPrefab, inventoryObject.transform);
            ItemObject newItemObject = newItem.GetComponent<ItemObject>();
            newItemObject.InitItemObject(item, quant);
            itemObjects.Add(newItemObject);
        }
        else
        {
            itemObject.quantity += quant;
        }
        InventoryChanged.Invoke();

        return itemObject != null ? itemObject : itemObjects[itemObjects.Count - 1];
    }

    public void RemoveItemFromInventory(Item item, int quant = 1)
    {
        if (quant <= 0)
            return;
        ItemObject itemObject = FindItem(item);
        if (itemObject == null)
            return;
        if (quant >= itemObject.quantity)
        {
            if (GlobalInput.Instance.selectedItemObject == itemObject)
            {
                GlobalInput.Instance.SetSelectedItemObject(null);
            }
            itemObjects.Remove(itemObject);
            Destroy(itemObject.gameObject);
        }
        else
        {
            itemObject.quantity -= quant;
        }
        InventoryChanged.Invoke();
    }

    public ItemObject FindItem(Item item)
    {
        if (item == null)
            return null;
        foreach(ItemObject child in itemObjects)
        {
            if (item.itemID == child.item.itemID)
                return child;
        }
        return null;
    }

    ///<summary>
    /// Returns ItemObject which originates from Item parsed as argument.
    ///</summary>
    public ItemObject FindItemByID(string itemID)
    {
        if (itemID == null)
            return null;
        foreach (ItemObject child in itemObjects)
        {
            if (itemID == child.item.itemID)
                return child;
        }
        return null;
    }

    public List<ItemObject> GetAllItemObjects()
    {
        return itemObjects;
    }
}
