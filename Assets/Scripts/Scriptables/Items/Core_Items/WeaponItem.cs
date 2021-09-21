using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponItem : Item
{
    public WeaponWielding weaponWielding;
    public WeaponRange weaponRange;
    public WeaponType weaponType;
    [Header("Attributes")]
    public Attributes attributes;
    public float attackDamageMultiplier;
    public float armorValue;

    public WeaponItem(WeaponItem newWeaponItem)
    {
        itemName = newWeaponItem.itemName;
        itemDescription = newWeaponItem.itemDescription;
        itemIcon = newWeaponItem.itemIcon;
        level = newWeaponItem.level;
        weaponWielding = newWeaponItem.weaponWielding;
        weaponRange = newWeaponItem.weaponRange;
        weaponType = newWeaponItem.weaponType;
        attributes = newWeaponItem.attributes;
        attackDamageMultiplier = newWeaponItem.attackDamageMultiplier;
        armorValue = newWeaponItem.armorValue;
    }

    public WeaponItem()
    {

    }

}
