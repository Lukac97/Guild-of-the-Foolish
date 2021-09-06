using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorItem : Item
{
    public ArmorSlot itemSlot;
    [Header("Attributes")]
    public Attributes attributes;
    public float armorValue;
    [Tooltip("Skip if its jewelry.")]
    public ArmorType armorType;

    public ArmorItem(ArmorItem newArmorItem)
    {
        itemName = newArmorItem.itemName;
        itemDescription = newArmorItem.itemDescription;
        itemIcon = newArmorItem.itemIcon;
        level = newArmorItem.level;
        armorType = newArmorItem.armorType;
        itemSlot = newArmorItem.itemSlot;
        attributes = newArmorItem.attributes;
        armorValue = newArmorItem.armorValue;
    }

    public ArmorItem()
    {

    }
}
