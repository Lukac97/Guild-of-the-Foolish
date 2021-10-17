using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    public int quantity;

    public void InitItemObject(Item newItem, int quant = 1)
    {
        item = newItem;
        quantity = quant;
    }
}
