using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public GuildInventory guildInventory;
    public Item item;
    public int quantity;

    void Start()
    {
        guildInventory = GetComponentInParent<GuildInventory>();
    }

    public void InitItemObject(Item newItem, int quant = 1)
    {
        item = newItem;
        quantity = 1;
    }
}
