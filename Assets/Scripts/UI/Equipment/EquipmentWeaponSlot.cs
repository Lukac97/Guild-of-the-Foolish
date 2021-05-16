using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentWeaponSlot : MonoBehaviour, IPointerClickHandler
{
    public WeaponSlot weaponSlot;
    public TextMeshProUGUI slotName;
    public Image itemIcon;
    public CharEquipment.WeaponSlotItem weaponSlotItem = null;
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
    }

    public void OnItemDoubleClick()
    {
        if (!GlobalInput.Instance.CheckIfSelectedCharacter())
            return;

        GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().UnequipSlot(weaponSlotItem);
    }

    public void SetItemSlot(CharEquipment.WeaponSlotItem slotItem)
    {
        WeaponItem item;
        if (slotItem == null)
        {
            item = null;
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
