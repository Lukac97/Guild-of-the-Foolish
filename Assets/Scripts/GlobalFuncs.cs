using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFuncs : MonoBehaviour
{
    private static GlobalFuncs _instance;
    public static GlobalFuncs Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    public bool CheckIfWorse(ArmorItem armor1, ArmorItem armor2)
    {
        if (armor1 == null)
            return true;
        if (armor2 == null)
            return false;
        EquipItemComparison.ArmorComparison armorCmp1 = new EquipItemComparison.ArmorComparison(armor1);
        EquipItemComparison.ArmorComparison armorCmp2 = new EquipItemComparison.ArmorComparison(armor2);
        if (armorCmp1.CalculateRating() < armorCmp2.CalculateRating())
            return true;
        else
            return false;
    }
    public bool CheckIfWorse(WeaponItem weapon1, WeaponItem weapon2)
    {
        if (weapon1 == null)
            return true;
        if (weapon2 == null)
            return false;
        EquipItemComparison.WeaponComparison weaponCmp1 = new EquipItemComparison.WeaponComparison(weapon1);
        EquipItemComparison.WeaponComparison weaponCmp2 = new EquipItemComparison.WeaponComparison(weapon2);
        if (weaponCmp1.CalculateRating() < weaponCmp2.CalculateRating())
            return true;
        else
            return false;
    }
}
