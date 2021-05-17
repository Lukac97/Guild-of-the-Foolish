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
        public bool isFakeEquipped;

        public WeaponSlotItem(WeaponSlot _slot)
        {
            slot = _slot;
            item = null;
            isFakeEquipped = false;
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
        AddAdditionalSlots();
        UpdateEquipmentAttributes();
    }

    private void Start()
    {
        charStats = GetComponent<CharStats>();
    }

    public void AddAdditionalSlots()
    {
        //TODO: Implement character class specific slots
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

    public bool EquipItem(ItemObject itemObject)
    {
        CharStats charStats = GetComponent<CharStats>();
        if (charStats.level < itemObject.item.level)
            return false;
        Item equipmentItem = itemObject.item;
        if (equipmentItem.GetType() != typeof(WeaponItem) & equipmentItem.GetType() != typeof(ArmorItem))
            return false;
        GuildInventory.Instance.RemoveItemFromInventory(equipmentItem);
        if(equipmentItem.GetType() == typeof(ArmorItem))
            EquipItem((ArmorItem)equipmentItem);
        else
            EquipItem((WeaponItem)equipmentItem);
        CharactersController.Instance.CharactersUpdated.Invoke();
        return true;
    }

    public bool EquipItem(ArmorItem armor)
    {
        List<ArmorSlotItem> occupiedSlots = new List<ArmorSlotItem>();
        foreach(ArmorSlotItem armorSlot in armorSlots)
        {
            if (armorSlot.slot == armor.itemSlot)
            {
                if(armorSlot.item == null)
                {
                    armorSlot.item = armor;
                    return true;
                }
                else
                {
                    occupiedSlots.Add(armorSlot);
                }
            }
        }
        if(occupiedSlots.Count > 0)
        {
            ArmorSlotItem finalSlot = null;
            foreach(ArmorSlotItem occSlot in occupiedSlots)
            {
                if (finalSlot == null)
                    finalSlot = occSlot;
                else if (GlobalFuncs.Instance.CheckIfWorse(occSlot.item, finalSlot.item))
                    finalSlot = occSlot;
            }

            if (!UnequipSlot(finalSlot, true))
                return false;
            finalSlot.item = armor;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EquipItem(WeaponItem weapon)
    {
        List<WeaponSlotItem> occupiedSlots = new List<WeaponSlotItem>();
        List<WeaponSlot> requiredSlots = new List<WeaponSlot>();
        WeaponSlotItem mainHand = null;
        WeaponSlotItem offHand = null;
        foreach (WeaponSlotItem weaponSlot in weaponSlots)
        {
            if (weaponSlot.slot == WeaponSlot.MAIN_HAND)
                mainHand = weaponSlot;
            else if (weaponSlot.slot == WeaponSlot.OFF_HAND)
                offHand = weaponSlot;
        }

        if (mainHand == null | offHand == null)
            return false;

        if (weapon.weaponWielding == WeaponWielding.MAIN_HAND)
        {
            if (mainHand.item != null)
                UnequipSlot(mainHand);
            mainHand.item = weapon;
            mainHand.isFakeEquipped = false;
        }
        else if (weapon.weaponWielding == WeaponWielding.OFF_HAND)
        {
            if (offHand.item != null)
                UnequipSlot(offHand);
            offHand.item = weapon;
            offHand.isFakeEquipped = false;
        }
        else if (weapon.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            if (mainHand.item != null)
                UnequipSlot(mainHand);
            mainHand.item = weapon;
            mainHand.isFakeEquipped = false;

            if (offHand.item != null)
                UnequipSlot(offHand);
            offHand.item = weapon;
            offHand.isFakeEquipped = true;
        }
        else if (weapon.weaponWielding == WeaponWielding.ONE_HANDED)
        {
            if(mainHand.item != null)
            {
                if(offHand.item != null)
                {
                    if (GlobalFuncs.Instance.CheckIfWorse(mainHand.item, offHand.item))
                    {
                        UnequipSlot(mainHand);
                        mainHand.item = weapon;
                        mainHand.isFakeEquipped = false;
                    }
                    else
                    {
                        UnequipSlot(offHand);
                        offHand.item = weapon;
                        offHand.isFakeEquipped = false;
                    }
                }
                else
                {
                    offHand.item = weapon;
                    offHand.isFakeEquipped = false;
                }
            }
            else
            {
                mainHand.item = weapon;
                mainHand.isFakeEquipped = false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public bool UnequipSlot(ArmorSlotItem armorSlot, bool equip=false)
    {
        if (armorSlot.item == null)
            return true;
        GuildInventory.Instance.AddItemToInventory(armorSlot.item);
        armorSlot.item = null;
        if (!equip)
            CharactersController.Instance.CharactersUpdated.Invoke();
        return true;
    }

    public bool UnequipSlot(WeaponSlotItem weaponSlot, bool equip = false)
    {
        if (weaponSlot.item == null)
            return true;
        GuildInventory.Instance.AddItemToInventory(weaponSlot.item);
        if(weaponSlot.item.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            foreach(WeaponSlotItem si in weaponSlots)
            {
                si.item = null;
            }
        }
        else
            weaponSlot.item = null;
        if (!equip)
            CharactersController.Instance.CharactersUpdated.Invoke();
        return true;
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
