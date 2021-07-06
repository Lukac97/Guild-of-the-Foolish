using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentArmorSlot : MonoBehaviour, IPointerClickHandler
{
    public ArmorSlot armorSlot;
    public TextMeshProUGUI slotName;
    public Image itemIcon;
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

    public void OnItemClick()
    {
        if (armorSlotItem != null)
        {
            EquipmentSlotPopUp.Instance.OpenEquipmentSlot(CharTabMain.Instance.currentChar, armorSlotItem);
        }
    }

    public void OnItemDoubleClick()
    {
        //CharEquipment charEq = CharTabMain.Instance.currentChar.GetComponent<CharEquipment>();
        //if(charEq != null)
        //    charEq.UnequipSlot(armorSlotItem);
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
            itemIcon.enabled = false;
        }
        else
        {
            slotName.gameObject.SetActive(false);
            itemIcon.enabled = true;
            itemIcon.sprite = item.itemIcon;
        }
    }
}
