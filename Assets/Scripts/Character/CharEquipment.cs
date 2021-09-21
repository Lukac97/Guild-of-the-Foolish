using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEquipment : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSlotItem
    {
        public WeaponSlot slot;
        private WeaponItem _item = null;
        public WeaponItem item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
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
        private ArmorItem _item = null;
        public ArmorItem item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }

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
    private CharCombat charCombat;

    private void Awake()
    {
        AddAdditionalSlots();
        UpdateEquipmentAttributes();
    }

    private void Start()
    {
        charStats = GetComponent<CharStats>();
        charCombat = GetComponent<CharCombat>();
    }

    public void AddAdditionalSlots()
    {
        //TODO: Implement character class specific slots
    }

    public void NotifyAfterEquipping(bool invoke)
    {
        if (invoke)
        {
            UpdateEquipmentAttributes();
            charStats.UpdateTotalAttributes();
        }
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
        equipmentAttributes = eqAttr;
    }

    #region BestSlot
    public List<WeaponSlotItem> FindBestSlotToEquipItem(WeaponItem weaponItem)
    {
        List<WeaponSlotItem> bestSlots = new List<WeaponSlotItem>();
        List<WeaponSlot> viableSlots = new List<WeaponSlot>();
        if(weaponItem.weaponWielding == WeaponWielding.MAIN_HAND)
        {
            viableSlots.Add(WeaponSlot.MAIN_HAND);
        }
        else if (weaponItem.weaponWielding == WeaponWielding.OFF_HAND)
        {
            viableSlots.Add(WeaponSlot.OFF_HAND);
        }
        else if (weaponItem.weaponWielding == WeaponWielding.ONE_HANDED)
        {
            viableSlots.Add(WeaponSlot.OFF_HAND);
            viableSlots.Add(WeaponSlot.MAIN_HAND);
        }
        else if (weaponItem.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            //FIRST ELEMENT WILL BE MAIN HAND
            bestSlots.Add(weaponSlots[0]);
            if(weaponSlots[1].slot == WeaponSlot.MAIN_HAND)
            {
                bestSlots.Insert(0, weaponSlots[1]);
            }
            else
            {
                bestSlots.Add(weaponSlots[1]);
            }
            return bestSlots;
        }

        foreach (WeaponSlotItem weaponSlotItem in weaponSlots)
        {
            if (viableSlots.Contains(weaponSlotItem.slot))
                bestSlots.Add(weaponSlotItem);
        }

        while(bestSlots.Count > 1)
        {
            if(GlobalFuncs.CheckIfWorse(bestSlots[0].item, bestSlots[1].item))
            {
                bestSlots.RemoveAt(1);
            }
            else
            {
                bestSlots.RemoveAt(0);
            }
        }

        return bestSlots;
    }

    public List<ArmorSlotItem> FindBestSlotToEquipItem(ArmorItem armorItem)
    {
        List<ArmorSlotItem> bestSlots = new List<ArmorSlotItem>();
        foreach(ArmorSlotItem armorSlotItem in armorSlots)
        {
            if (GlobalFuncs.CheckIfWorse(armorSlotItem.item, armorItem))
            {
                bestSlots.Add(armorSlotItem);
            }
        }

        while(bestSlots.Count > 1)
        {
            if(GlobalFuncs.CheckIfWorse(bestSlots[0].item, bestSlots[1].item))
            {
                bestSlots.RemoveAt(1);
            }
            else
            {
                bestSlots.RemoveAt(0);
            }
        }

        return bestSlots;
    }
    #endregion BestSlot

    public bool EquipItem(ItemObject itemObject)
    {
        if(itemObject.item.GetType() == typeof(WeaponItem))
        {
            return EquipItem(itemObject, FindBestSlotToEquipItem((WeaponItem)itemObject.item)[0]);
        }
        else if (itemObject.item.GetType() == typeof(ArmorItem))
        {
            return EquipItem(itemObject, FindBestSlotToEquipItem((ArmorItem)itemObject.item)[0]);
        }
        return false;
    }

    public bool EquipItem(ItemObject itemObject, ArmorSlotItem armorSlotItem)
    {
        return EquipItem(itemObject, armorSlotItem, null);
    }

    public bool EquipItem(ItemObject itemObject, WeaponSlotItem weaponSlotItem)
    {
        return EquipItem(itemObject, null, weaponSlotItem);
    }

    private bool EquipItem(ItemObject itemObject, ArmorSlotItem armorSlotItem, WeaponSlotItem weaponSlotItem)
    {
        //Perform checks
        Item equipmentItem = itemObject.item;
        if (!CanEquipItem(equipmentItem))
            return false;
        //
        GuildInventory.Instance.RemoveItemFromInventory(equipmentItem);
        if (armorSlotItem != null)
            EquipItem((ArmorItem)equipmentItem, armorSlotItem);
        else if (weaponSlotItem != null)
            EquipItem((WeaponItem)equipmentItem, weaponSlotItem);
        else
            return false;
        NotifyAfterEquipping(true);
        return true;
    }

    public bool CanEquipItem(Item itemToCheck)
    {
        if (charStats.level < itemToCheck.level)
            return false;
        if(itemToCheck.GetType() == typeof(WeaponItem))
        {
            if (charStats.characterClass.viableWeaponTypes.Contains(((WeaponItem)itemToCheck).weaponType))
                return true;
            else
                return false;
        }
        else if (itemToCheck.GetType() == typeof(ArmorItem))
        {
            if (((ArmorItem)itemToCheck).itemSlot == ArmorSlot.FINGER)
                return true;
            else if (charStats.characterClass.viableArmorTypes.Contains(((ArmorItem)itemToCheck).armorType))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    #region Specific Equips
    private bool EquipItem(ArmorItem armor, ArmorSlotItem slotItem)
    {
        if (slotItem == null)
            return false;
        if (!UnequipSlot(slotItem, true))
            return false;
        slotItem.item = armor;
        return true;
    }

    private bool EquipItem(WeaponItem weapon, WeaponSlotItem slotItem)
    {
        if (slotItem == null)
            return false;
        if (!UnequipSlot(slotItem, true))
            return false;
        if (weapon.weaponWielding == WeaponWielding.MAIN_HAND)
        {
            if (slotItem.slot != WeaponSlot.MAIN_HAND)
                return false;
            UnequipSlot(slotItem, true);
            slotItem.isFakeEquipped = false;
            slotItem.item = weapon;
        }
        else if (weapon.weaponWielding == WeaponWielding.OFF_HAND)
        {
            if (slotItem.slot != WeaponSlot.OFF_HAND)
                return false;
            UnequipSlot(slotItem, true);
            slotItem.isFakeEquipped = false;
            slotItem.item = weapon;
        }
        else if (weapon.weaponWielding == WeaponWielding.ONE_HANDED)
        {
            UnequipSlot(slotItem, true);
            slotItem.isFakeEquipped = false;
            slotItem.item = weapon;
        }
        else if (weapon.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            UnequipSlot(slotItem, true);
            slotItem.item = weapon;
            foreach(WeaponSlotItem wSlotItem in weaponSlots)
            {
                if(wSlotItem.slot != slotItem.slot)
                {
                    wSlotItem.item = weapon;
                    wSlotItem.isFakeEquipped = true;
                    break;
                }
            }
        }
        
        slotItem.item = weapon;
        return true;
    }
    
    private bool EquipItemAuto(ArmorItem armor)
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
                else if (GlobalFuncs.CheckIfWorse(occSlot.item, finalSlot.item))
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

    private bool EquipItemAuto(WeaponItem weapon)
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
                UnequipSlot(mainHand, true);
            mainHand.item = weapon;
            mainHand.isFakeEquipped = false;
        }
        else if (weapon.weaponWielding == WeaponWielding.OFF_HAND)
        {
            if (offHand.item != null)
                UnequipSlot(offHand, true);
            offHand.item = weapon;
            offHand.isFakeEquipped = false;
        }
        else if (weapon.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            if (mainHand.item != null)
                UnequipSlot(mainHand, true);
            mainHand.item = weapon;
            mainHand.isFakeEquipped = false;

            if (offHand.item != null)
                UnequipSlot(offHand, true);
            offHand.item = weapon;
            offHand.isFakeEquipped = true;
        }
        else if (weapon.weaponWielding == WeaponWielding.ONE_HANDED)
        {
            if(mainHand.item != null)
            {
                if(offHand.item != null)
                {
                    if (GlobalFuncs.CheckIfWorse(mainHand.item, offHand.item))
                    {
                        UnequipSlot(mainHand, true);
                        mainHand.item = weapon;
                        mainHand.isFakeEquipped = false;
                    }
                    else
                    {
                        UnequipSlot(offHand, true);
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
    #endregion Specific Equips

    public bool UnequipSlot(ArmorSlotItem armorSlot, bool equip=false)
    {
        if (armorSlot.item == null)
            return true;
        GuildInventory.Instance.AddItemToInventory(armorSlot.item);
        armorSlot.item = null;
        NotifyAfterEquipping(!equip);
        return true;
    }

    public bool UnequipSlot(WeaponSlotItem weaponSlot, bool equip = false)
    {
        if (weaponSlot.item == null)
            return true;
        GuildInventory.Instance.AddItemToInventory(weaponSlot.item);
        if(weaponSlot.item.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            //clear both slots when two handed
            foreach(WeaponSlotItem si in weaponSlots)
            {
                si.item = null;
            }
        }
        else
            weaponSlot.item = null;
        NotifyAfterEquipping(!equip);
        return true;
    }
}
