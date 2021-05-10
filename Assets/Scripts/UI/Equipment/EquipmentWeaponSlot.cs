using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentWeaponSlot : MonoBehaviour
{
    public WeaponSlot weaponSlot;
    public TextMeshProUGUI slotName;
    public Image itemIcon;

    public void SetItemSlot(WeaponItem item)
    {
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
