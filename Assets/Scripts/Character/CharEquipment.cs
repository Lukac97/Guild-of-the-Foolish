using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEquipment : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSlotItem
    {
        public WeaponSlot slot;
        public WeaponItem item;

        public WeaponSlotItem(WeaponSlot _slot)
        {
            slot = _slot;
            item = null;
        }
    }

    [System.Serializable]
    public class ArmorSlotItem
    {
        public ArmorSlot slot;
        public ArmorItem item;

        public ArmorSlotItem(ArmorSlot _slot)
        {
            slot = _slot;
            item = null;
        }
    }

    [Header("Weapons")]
    public List<WeaponSlotItem> weaponSlots;
    [Header("Armor")]
    public List<ArmorSlotItem> armorSlots;

    public Attributes equipmentAttributes;

    private CharStats charStats;

    private void Awake()
    {
        weaponSlots = new List<WeaponSlotItem>();
        armorSlots = new List<ArmorSlotItem>();
        AddDefaultWeaponSlots();
        AddDefaultArmorSlots();
        UpdateEquipmentAttributes();
    }

    private void Start()
    {
        charStats = GetComponent<CharStats>();
    }

    public void AddDefaultWeaponSlots()
    {
        weaponSlots.Add(new WeaponSlotItem(WeaponSlot.MAIN_HAND));
        weaponSlots.Add(new WeaponSlotItem(WeaponSlot.OFF_HAND));
    }

    public void AddDefaultArmorSlots()
    {
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.HEAD));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.SHOULDERS));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.HANDS));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.CHEST));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.LEGS));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.FEET));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.NECK));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.FINGER));
        armorSlots.Add(new ArmorSlotItem(ArmorSlot.FINGER));
    }

    public void UpdateEquipmentAttributes()
    {
        Attributes eqAttr = new Attributes();
        foreach(WeaponSlotItem weapon in weaponSlots)
        {
            if(weapon.item != null)
                eqAttr += weapon.item.attributes;
        }
        foreach (ArmorSlotItem armor in armorSlots)
        {
            if(armor.item != null)
                eqAttr += armor.item.attributes;
        }
    }

    public void EquipItem(WeaponItem item)
    {

    }

    public void EquipItem(ArmorItem item)
    {

    }

    public bool CanWear(WeaponItem item)
    {
        foreach(WeaponType wType in charStats.characterClass.viableWeaponTypes)
        {
            if(item.weaponType == wType)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanWear(ArmorItem item)
    {
        foreach (ArmorType aType in charStats.characterClass.viableArmorTypes)
        {
            if (item.armorType == aType)
            {
                return true;
            }
        }
        return false;
    }

}
