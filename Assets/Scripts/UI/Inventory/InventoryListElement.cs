using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryListElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ItemIconHandler icon;

    public ItemObject itemObject;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI level;
    [Space(6)]
    public UnityEvent<ItemObject> SingleClickEvent;
    public UnityEvent<ItemObject> DoubleClickEvent;

    private Vector2 localStartDragPos;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ItemExistsCheck())
            return;

        icon.SetIconTransparency(0.4f);
        localStartDragPos = GetComponent<RectTransform>().position;
        DraggingIconHandler.Instance.StartDraggingObject(this);
        DraggingIconHandler.Instance.UpdateObjectDrag(localStartDragPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!ItemExistsCheck())
            return;

        localStartDragPos += eventData.delta / DraggingIconHandler.Instance.canvas.scaleFactor;
        DraggingIconHandler.Instance.UpdateObjectDrag(transform.TransformPoint(localStartDragPos));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!ItemExistsCheck())
            return;

        icon.SetIconTransparency(1f);
        DraggingIconHandler.Instance.StopObjectDrag();
    }

    private bool ItemExistsCheck()
    {
        if (itemObject == null)
            return false;
        if (itemObject.item == null)
            return false;

        return true;
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
