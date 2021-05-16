using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryListElement : MonoBehaviour, IPointerClickHandler
{
    public Image icon;

    public ItemObject itemObject;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI level;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 1)
        {
            OnItemClick();
        }
        else if (eventData.clickCount == 2)
        {
            OnItemDoubleClick();
        }
    }

    public void AssignItemObject(ItemObject iObject)
    {
        if (iObject == null)
        {
            itemObject = null;
            icon.enabled = false;
            quantity.text = "";
            level.text = "";
        }
        else
        {
            icon.enabled = true;
            itemObject = iObject;
            icon.sprite = iObject.item.itemIcon;
            if (iObject.quantity == 1)
                quantity.text = "";
            else
                quantity.text = iObject.quantity.ToString();
            level.text = iObject.item.level.ToString();
        }
    }
    public void OnItemClick()
    {
        GlobalInput.Instance.SetSelectedItemObject(itemObject);
    }

    public void OnItemDoubleClick()
    {
        if(GlobalInput.Instance.CheckIfSelectedCharacter())
        {
            GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().EquipItem(itemObject);
        }
    }
}
