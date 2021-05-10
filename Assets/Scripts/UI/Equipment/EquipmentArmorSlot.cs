using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentArmorSlot : MonoBehaviour
{
    public ArmorSlot armorSlot;
    public TextMeshProUGUI slotName;
    public Image itemIcon;

    public void SetItemSlot(ArmorItem item)
    {
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
