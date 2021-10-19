using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentArmorSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ArmorSlot armorSlot;
    public TextMeshProUGUI slotName;
    public ItemIconHandler itemIcon;
    public CharEquipment.ArmorSlotItem armorSlotItem = null;

    private EquipmentSlotController parentController;
    private Vector2 localStartDragPos;
    void Start()
    {
        parentController = GetComponentInParent<EquipmentSlotController>();
    }

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
        HoveringInfoDisplay.Instance.ShowItemDetailsDisplay(this, true);
    }

    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.HideItemDetailsDisplay();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ItemExistsCheck())
            return;

        itemIcon.SetIconTransparency(0.4f);
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
        itemIcon.SetIconTransparency(1f);
        DraggingIconHandler.Instance.StopObjectDrag();
    }

    private bool ItemExistsCheck()
    {
        if (armorSlotItem == null)
            return false;
        if (armorSlotItem.item == null)
            return false;

        return true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
            return;
        if (eventData.pointerDrag != null)
        {
            InventoryListElement iLEl = eventData.pointerDrag.GetComponent<InventoryListElement>();
            if (iLEl == null)
            {
                EquipmentArmorSlot aSlot = eventData.pointerDrag.GetComponent<EquipmentArmorSlot>();
                if (aSlot == null | aSlot.armorSlotItem == null | aSlot.armorSlotItem.item == null)
                    return;
                if (!GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().
                    IsCorrectItemSlot(aSlot.armorSlotItem.item, armorSlotItem))
                    return;
                else if (aSlot.armorSlotItem.item != null)
                {
                    ItemObject newItemObject = GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().UnequipSlot(aSlot.armorSlotItem);
                    GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().EquipItem(newItemObject, armorSlotItem);
                }
            }
            else if (iLEl.itemObject != null & iLEl.itemObject.item != null)
            {
                GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().EquipItem(iLEl.itemObject, armorSlotItem);
            }
        }
    }

    public void OnItemClick()
    {
        //if (armorSlotItem != null)
        //{
        //    EquipmentSlotPopUp.Instance.OpenEquipmentSlot(CharTabMain.Instance.currentChar, armorSlotItem);
        //}
    }

    public void OnItemDoubleClick()
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
            return;
        CharEquipment charEq = GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>();
        charEq.UnequipSlot(armorSlotItem);
    }


    public void SetItemSlot(CharEquipment.ArmorSlotItem slotItem)
    {
        ArmorItem item;
        if (slotItem == null)
        {
            item = null;
            armorSlotItem = null;
        }
        else
        {
            item = slotItem.item;
            armorSlotItem = slotItem;
        }
        if(item == null)
        {
            slotName.gameObject.SetActive(true);
            slotName.text = armorSlot.ToString().Replace("_", " ");
            itemIcon.IconSetActive(false);
        }
        else
        {
            slotName.gameObject.SetActive(false);
            itemIcon.IconSetActive(true);
            itemIcon.InitItemIconHandler(item);
        }
    }
}
