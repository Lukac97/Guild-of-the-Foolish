using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentArmorSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public ArmorSlot armorSlot;
    public TextMeshProUGUI slotName;
    public ItemIconHandler itemIcon;
    public CharEquipment.ArmorSlotItem armorSlotItem = null;

    private EquipmentSlotController parentController;
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

    public void OnDrop(PointerEventData eventData)
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
            return;
        if (eventData.pointerDrag != null)
        {
            InventoryListElement iLEl = eventData.pointerDrag.GetComponent<InventoryListElement>();
            if ((iLEl != null) & (iLEl.itemObject.item != null))
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
        CharEquipment charEq = CharTabMain.Instance.currentChar.GetComponent<CharEquipment>();
        if (charEq != null)
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
