using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentWeaponSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public WeaponSlot weaponSlot;
    public TextMeshProUGUI slotName;
    public ItemIconHandler itemIcon;
    public CharEquipment.WeaponSlotItem weaponSlotItem = null;

    private EquipmentSlotController parentController;
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
