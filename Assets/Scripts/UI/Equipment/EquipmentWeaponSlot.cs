using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentWeaponSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public WeaponSlot weaponSlot;
    public TextMeshProUGUI slotName;
    public ItemIconHandler itemIcon;
    public CharEquipment.WeaponSlotItem weaponSlotItem = null;

    private EquipmentSlotController parentController;
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
        if (!ItemExistsCheck())
            return;

        itemIcon.SetIconTransparency(1f);
        DraggingIconHandler.Instance.StopObjectDrag();
    }

    private bool ItemExistsCheck()
    {
        if (weaponSlotItem == null)
            return false;
        if (weaponSlotItem.item == null)
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
                EquipmentWeaponSlot wSlot = eventData.pointerDrag.GetComponent<EquipmentWeaponSlot>();
                if (wSlot == null | wSlot.weaponSlotItem == null | wSlot.weaponSlotItem.item == null)
                    return;
                if (!GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().
                    IsCorrectItemSlot(wSlot.weaponSlotItem.item, weaponSlotItem))
                    return;
                else if(wSlot.weaponSlotItem.item != null)
                {
                    ItemObject newItemObject = GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().UnequipSlot(wSlot.weaponSlotItem);
                    GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().EquipItem(newItemObject, weaponSlotItem);
                }
            }
            else if (iLEl.itemObject != null & iLEl.itemObject.item != null)
            {

                GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().EquipItem(iLEl.itemObject, weaponSlotItem);
            }
        }
    }

    public void OnItemClick()
    {
        //if (weaponSlotItem != null)
        //{
        //    EquipmentSlotPopUp.Instance.OpenEquipmentSlot(CharTabMain.Instance.currentChar, weaponSlotItem);
        //}
    }

    public void OnItemDoubleClick()
    {
        CharEquipment charEq = CharTabMain.Instance.currentChar.GetComponent<CharEquipment>();
        if (charEq != null)
            charEq.UnequipSlot(weaponSlotItem);
    }

    public void SetItemSlot(CharEquipment.WeaponSlotItem slotItem)
    {
        WeaponItem item;
        if (slotItem == null)
        {
            item = null;
            weaponSlotItem = null;
        }
        else
        {
            item = slotItem.item;
            weaponSlotItem = slotItem;
        }

        if (item == null)
        {
            slotName.gameObject.SetActive(true);
            slotName.text = weaponSlot.ToString().Replace("_", " ");
            itemIcon.IconSetActive(false);
        }
        else
        {
            slotName.gameObject.SetActive(false);
            itemIcon.IconSetActive(true);
            itemIcon.InitItemIconHandler(item);
            if (slotItem.isFakeEquipped)
            {
                itemIcon.SetIconTransparency(0.4f);
            }
            else
            {
                itemIcon.SetIconTransparency(1.0f);
            }
        }
    }
}
