using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryListElement : MonoBehaviour
{
    public Image icon;

    public ItemObject itemObject;

    public void AssignItemObject(ItemObject iObject)
    {
        if (iObject == null)
        {
            itemObject = null;
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            itemObject = iObject;
            icon.sprite = iObject.item.itemIcon;
        }
    }

    public void OnItemClick()
    {
        GlobalInput.Instance.SetSelectedItemObject(itemObject);
    }
}
