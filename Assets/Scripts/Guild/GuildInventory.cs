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

    public delegate void InventoryChangedDelegate();
    public InventoryChangedDelegate InventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
    }

    public void AddItemToInventory(Item item, int quant = 1)
    {
        if (quant <= 0)
            return;
        ItemObject itemObject = FindItem(item);
        if (itemObject == null)
        {
            GameObject newItem = Instantiate(itemObjectPrefab, inventoryObject.transform);
            newItem.GetComponent<ItemObject>().InitItemObject(item, quant);
        }
        else
        {
            itemObject.quantity += quant;
        }
        InventoryChanged.Invoke();
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
        foreach(Transform child in inventoryObject.transform)
        {
            ItemObject itemObject = child.GetComponent<ItemObject>();
            if (item == itemObject.item)
                return itemObject;
        }
        return null;
    }

    public List<ItemObject> GetAllItemObjects()
    {
        List<ItemObject> itemObjList = new List<ItemObject>();
        foreach(Transform child in inventoryObject.transform)
        {
            itemObjList.Add(child.GetComponent<ItemObject>());
        }
        return itemObjList;
    }
}
