using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryListElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemIconHandler icon;

    public ItemObject itemObject;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI level;
    [Space(6)]
    public UnityEvent<ItemObject> SingleClickEvent;
    public UnityEvent<ItemObject> DoubleClickEvent;

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

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.ShowItemDetailsDisplay(this, false);
    }

    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.HideItemDetailsDisplay();
    }

    public void AssignItemObject(ItemObject iObject)
    {
        if (iObject == null)
        {
            itemObject = null;
            icon.IconSetActive(false);
            quantity.text = "";
            level.text = "";
        }
        else
        {
            icon.IconSetActive(true);
            itemObject = iObject;
            icon.InitItemIconHandler(iObject.item);
            if (iObject.quantity == 1)
                quantity.text = "";
            else
                quantity.text = iObject.quantity.ToString();
            level.text = iObject.item.level.ToString();
        }
    }

    public void AssignEvents(UnityEvent<ItemObject> _single, UnityEvent<ItemObject> _double)
    {
        SingleClickEvent = _single;
        DoubleClickEvent = _double;
    }

    public void SetHighlight(bool doHighlight, Color color = default(Color))
    {
        //TODO: Implement Highlight
        //if(doHighlight)
        //{
        //    if (color != null)
        //    {
        //        icon.color = color;
        //        return;
        //    }
        //}
        //icon.color = Color.white;
    }

    public void OnItemClick()
    {
        if (SingleClickEvent != null)
        {
            SingleClickEvent.Invoke((ItemObject)itemObject);
            HoveringInfoDisplay.Instance.ShowItemDetailsDisplay(this, false);
        }
    }

    public void OnItemDoubleClick()
    {
        if (DoubleClickEvent != null)
        {
            DoubleClickEvent.Invoke((ItemObject)itemObject);
            HoveringInfoDisplay.Instance.ShowItemDetailsDisplay(this, false);
        }
    }
}
