using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipItemComparison : MonoBehaviour
{
    [HideInInspector]
    public CharEquipment charEquipment;
    [HideInInspector]
    public ItemObject itemObject;
    [HideInInspector]
    public List<Item> itemsToRemove;

    public GameObject mainPanel;
    public GameObject bodyPanel;
    public GameObject statComparisonPrefab;

    [Space(3)]
    public GameObject toRemoveOne;
    public GameObject toRemoveTwo;
    public TextMeshProUGUI nameToEquip;
    public TextMeshProUGUI nameToRemove;
    public TextMeshProUGUI nameToRemoveFirst;
    public TextMeshProUGUI nameToRemoveSecond;

    [Space(6)]
    public List<WeaponSlot> targetSlotsWeapon;
    public WeaponWielding currentWielding;

    public void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += UpdateComparison;
        GlobalInput.Instance.changeSelectedItemObject += UpdateComparison;
        UpdateComparison();
    }

    [System.Serializable]
    public class WeaponComparison
    {
        public int attackDamagePercent;
        public int armorValue;
        public Attributes attributes;

        public WeaponComparison()
        {
            attackDamagePercent = 0;
            armorValue = 0;
            attributes = new Attributes();
        }

        public WeaponComparison(WeaponItem weapon)
        {
            attackDamagePercent = weapon.attackDamageMultiplier <= 0 ? 0 : Mathf.FloorToInt(weapon.attackDamageMultiplier*100);
            armorValue = weapon.armorValue <= 0 ? 0 : Mathf.FloorToInt(weapon.armorValue);
            attributes = new Attributes(weapon.attributes);
        }

        public static WeaponComparison operator+(WeaponComparison a, WeaponComparison b)
        {
            WeaponComparison c = new WeaponComparison();
            c.attackDamagePercent = a.attackDamagePercent + b.attackDamagePercent;
            c.armorValue = a.armorValue + b.armorValue;
            c.attributes = a.attributes + b.attributes;
            return c;
        }
        public static WeaponComparison operator -(WeaponComparison a, WeaponComparison b)
        {
            WeaponComparison c = new WeaponComparison();
            c.attackDamagePercent = a.attackDamagePercent - b.attackDamagePercent;
            c.armorValue = a.armorValue - b.armorValue;
            c.attributes = a.attributes - b.attributes;
            return c;
        }
        public float CalculateRating()
        {
            return attributes.SumOfAllAttributes() + armorValue / 8 + attackDamagePercent/10;
        }
    }

    [System.Serializable]
    public class ArmorComparison
    {
        public int armorValue;
        public Attributes attributes;

        public ArmorComparison()
        {
            armorValue = 0;
            attributes = new Attributes();
        }

        public ArmorComparison(ArmorItem armor)
        {
            armorValue = armor.armorValue <= 0 ? 0 : Mathf.FloorToInt(armor.armorValue);
            attributes = new Attributes(armor.attributes);
        }

        public static ArmorComparison operator +(ArmorComparison a, ArmorComparison b)
        {
            ArmorComparison c = new ArmorComparison();
            c.armorValue = a.armorValue + b.armorValue;
            c.attributes = a.attributes + b.attributes;
            return c;
        }

        public static ArmorComparison operator -(ArmorComparison a, ArmorComparison b)
        {
            ArmorComparison c = new ArmorComparison();
            c.armorValue = a.armorValue - b.armorValue;
            c.attributes = a.attributes - b.attributes;
            return c;
        }

        public float CalculateRating()
        {
            return attributes.SumOfAllAttributes() + armorValue/8;
        }
    }

    public ArmorComparison CompareEquipment(ArmorItem armor)
    {
        if (charEquipment == null | itemObject == null)
            return null;
        ArmorComparison toEquipArmor = new ArmorComparison(armor);
        ArmorComparison toRemoveArmor = new ArmorComparison();

        CharEquipment.ArmorSlotItem suitableSlot = null;
        itemsToRemove = new List<Item>();

        foreach(CharEquipment.ArmorSlotItem slot in charEquipment.armorSlots)
        {
            if (slot.slot == armor.itemSlot)
            {
                if (suitableSlot == null)
                    suitableSlot = slot;
                else if (GlobalFuncs.CheckIfWorse(slot.item, suitableSlot.item))
                    suitableSlot = slot;
            }
        }
        if (suitableSlot == null)
            return null;
        if (suitableSlot.item != null)
        {
            toRemoveArmor += new ArmorComparison(suitableSlot.item);
            itemsToRemove.Add(suitableSlot.item);
        }
        return toEquipArmor - toRemoveArmor;
    }

    public WeaponComparison CompareEquipment(WeaponItem weapon)
    {
        if (charEquipment == null | itemObject == null)
            return null;
        WeaponComparison toEquipWeapon = new WeaponComparison(weapon);
        WeaponComparison toRemoveWeapon = new WeaponComparison();
        CharEquipment.WeaponSlotItem offHand = null;
        CharEquipment.WeaponSlotItem mainHand = null;
        foreach (CharEquipment.WeaponSlotItem slot in charEquipment.weaponSlots)
        {
            if (slot.slot == WeaponSlot.MAIN_HAND)
                mainHand = slot;
            if (slot.slot == WeaponSlot.OFF_HAND)
                offHand = slot;
        }
        if (offHand == null | mainHand == null)
            return null;
        currentWielding = weapon.weaponWielding;
        targetSlotsWeapon = new List<WeaponSlot>();
        itemsToRemove = new List<Item>();
        if(weapon.weaponWielding == WeaponWielding.TWO_HANDED)
        {
            if (mainHand.item != null)
            {
                toRemoveWeapon += new WeaponComparison(mainHand.item);
                itemsToRemove.Add(mainHand.item);
            }
            if (offHand.item != null)
            {
                if (!offHand.isFakeEquipped)
                {
                    toRemoveWeapon += new WeaponComparison(offHand.item);
                    itemsToRemove.Add(offHand.item);
                }
            }
            targetSlotsWeapon.Add(WeaponSlot.MAIN_HAND);
            targetSlotsWeapon.Add(WeaponSlot.OFF_HAND);
        }
        if (weapon.weaponWielding == WeaponWielding.ONE_HANDED)
        {
            if(mainHand.item == null)
            {
                targetSlotsWeapon.Add(WeaponSlot.MAIN_HAND);
            }
            else
            {
                if(offHand.item == null)
                {
                    targetSlotsWeapon.Add(WeaponSlot.OFF_HAND);
                }
                else
                {
                    if (GlobalFuncs.CheckIfWorse(mainHand.item, offHand.item))
                    {
                        toRemoveWeapon += new WeaponComparison(mainHand.item);
                        itemsToRemove.Add(mainHand.item);
                        targetSlotsWeapon.Add(WeaponSlot.MAIN_HAND);
                    }
                    else
                    {
                        toRemoveWeapon += new WeaponComparison(offHand.item);
                        itemsToRemove.Add(offHand.item);
                        targetSlotsWeapon.Add(WeaponSlot.OFF_HAND);
                    }
                }
            }
        }
        if(weapon.weaponWielding == WeaponWielding.MAIN_HAND)
        {
            if(mainHand.item != null)
            {
                toRemoveWeapon += new WeaponComparison(mainHand.item);
                itemsToRemove.Add(mainHand.item);
                targetSlotsWeapon.Add(WeaponSlot.MAIN_HAND);
            }
        }
        if (weapon.weaponWielding == WeaponWielding.OFF_HAND)
        {
            if (mainHand.item != null)
            {
                toRemoveWeapon += new WeaponComparison(offHand.item);
                itemsToRemove.Add(offHand.item);
                targetSlotsWeapon.Add(WeaponSlot.OFF_HAND);
            }
        }

        return toEquipWeapon - toRemoveWeapon;
    }

    public void UpdateComparison()
    {
        if (GlobalInput.Instance.CheckIfSelectedCharacter() & GlobalInput.Instance.selectedItemObject != null)
        {
            
            charEquipment = GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>();
            itemObject = GlobalInput.Instance.selectedItemObject;
            WeaponComparison weaponComparison = null;
            ArmorComparison armorComparison = null;
            if (itemObject.item.GetType() == typeof(WeaponItem))
                weaponComparison = CompareEquipment((WeaponItem)itemObject.item);
            else if (itemObject.item.GetType() == typeof(ArmorItem))
                armorComparison = CompareEquipment((ArmorItem)itemObject.item);
            if (weaponComparison == null & armorComparison == null)
            {
                mainPanel.SetActive(false);
                return;
            }
            else
                mainPanel.SetActive(true);

            ClearPreviousDisplay();
            if (weaponComparison != null)
                DisplayComparison(weaponComparison);
            else if (armorComparison != null)
                DisplayComparison(armorComparison);
        }
        else
        {
            itemObject = null;
            charEquipment = null;
            mainPanel.SetActive(false);
        }
    }

    public void ClearPreviousDisplay()
    {
        foreach(Transform child in bodyPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayComparison(WeaponComparison weaponComparison)
    {
        nameToEquip.text = itemObject.item.name;
        if(itemsToRemove.Count <= 1)
        {
            toRemoveOne.SetActive(true);
            toRemoveTwo.SetActive(false);
            if (itemsToRemove.Count == 0)
                nameToRemove.text = "(Empty slot)";
            else
                nameToRemove.text = itemsToRemove[0].name;
        }
        else
        {
            toRemoveOne.SetActive(false);
            toRemoveTwo.SetActive(true);
            nameToRemoveFirst.text = itemsToRemove[0].name;
            nameToRemoveSecond.text = itemsToRemove[1].name;
        }
        if(weaponComparison.attackDamagePercent != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("ATT DMG", weaponComparison.attackDamagePercent, "%");
        }
        if (weaponComparison.armorValue != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("ARMOR", weaponComparison.armorValue);
        }
        if (weaponComparison.attributes.strength != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("STR", weaponComparison.attributes.strength);
        }
        if (weaponComparison.attributes.agility != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("AGI", weaponComparison.attributes.agility);
        }
        if (weaponComparison.attributes.intellect != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("INT", weaponComparison.attributes.intellect);
        }
        if (weaponComparison.attributes.luck != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("LUK", weaponComparison.attributes.luck);
        }
    }
    public void DisplayComparison(ArmorComparison armorComparison)
    {
        nameToEquip.text = itemObject.item.name;
        toRemoveOne.SetActive(true);
        toRemoveTwo.SetActive(false);
        if (itemsToRemove.Count == 0)
            nameToRemove.text = "(Empty slot)";
        else
            nameToRemove.text = itemsToRemove[0].name;

        if (armorComparison.armorValue != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("ARMOR", armorComparison.armorValue);
        }
        if (armorComparison.attributes.strength != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("STR", armorComparison.attributes.strength);
        }
        if (armorComparison.attributes.agility != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("AGI", armorComparison.attributes.agility);
        }
        if (armorComparison.attributes.intellect != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("INT", armorComparison.attributes.intellect);
        }
        if (armorComparison.attributes.luck != 0)
        {
            GameObject gO = Instantiate(statComparisonPrefab, bodyPanel.transform);
            gO.GetComponent<CmpStat>().SetText("LUK", armorComparison.attributes.luck);
        }
    }
}
