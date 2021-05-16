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
        GlobalInput.Instance.SetSelectedItemObject(itemObject);
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

            if (!UnequipSlot(occupiedSlots[0], true))
                return false;
            occupiedSlots[0].item = armor;
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
        }
        else if (weapon.weaponWielding == WeaponWielding.OFF_HAND)
        {
            if (offHand.item != null)
                UnequipSlot(offHand);
            offHand.item = weapon;
        }
        else if (weapon.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            if (mainHand.item != null)
                UnequipSlot(mainHand);
            mainHand.item = weapon;

            if (offHand.item != null)
                UnequipSlot(offHand);
            offHand.item = weapon;
        }
        else if (weapon.weaponWielding == WeaponWielding.ONE_HANDED)
        {
            if(mainHand.item != null)
            {
                if(offHand.item != null)
                {
                    UnequipSlot(mainHand);
                    mainHand.item = weapon;
                }
                else
                {
                    offHand.item = weapon;
                }
            }
            else
            {
                mainHand.item = weapon;
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
