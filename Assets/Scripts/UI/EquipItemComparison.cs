using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipItemComparison
{
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
            attackDamagePercent = weapon.attackDamageMultiplier <= 0 ? 0 : Mathf.FloorToInt(weapon.attackDamageMultiplier * 100);
            armorValue = weapon.armorValue <= 0 ? 0 : Mathf.FloorToInt(weapon.armorValue);
            attributes = new Attributes(weapon.attributes);
        }

        public static WeaponComparison operator +(WeaponComparison a, WeaponComparison b)
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
            return attributes.SumOfAllAttributes() + armorValue / 8 + attackDamagePercent / 10;
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
            return attributes.SumOfAllAttributes() + armorValue / 8;
        }
    }
}
