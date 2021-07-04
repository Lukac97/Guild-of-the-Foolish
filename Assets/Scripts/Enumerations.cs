using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumToString
{
    public static string ToNiceString(WeaponSlot enumer)
    {
        return ToNiceString(enumer.ToString());
    }

    public static string ToNiceString(ArmorSlot enumer)
    {
        return ToNiceString(enumer.ToString());
    }

    public static string ToNiceString(string str)
    {
        str = str.Replace('_', ' ').ToUpper();
        return str;
    }
}

[System.Serializable]
public enum AttributesEnum
{
    STRENGTH,
    AGILITY,
    INTELLECT,
    LUCK
}

#region Equipment and Inventory
public enum InventorySortBy
{
    NAME,
    QUANTITY,
    LEVEL
}

public enum InventorySortOrder
{
    ASCENDING,
    DESCENDING
}

public enum WeaponSlot
{
    MAIN_HAND,
    OFF_HAND
}

public enum WeaponWielding
{
    ONE_HANDED,
    TWO_HANDED,
    MAIN_HAND,
    OFF_HAND
}

public enum WeaponRange
{
    MELEE,
    RANGED
}

public enum WeaponType
{
    WAND,
    STAFF,
    DAGGER,
    SWORD,
    AXE,
    MACE,
    POLEARM,
    JAVELIN,
    BOW,
    CROSSBOW,
    TOME,
    SHIELD
}

public enum ArmorSlot
{
    HEAD,
    SHOULDERS,
    CHEST,
    LEGS,
    FEET,
    HANDS,
    NECK,
    FINGER
}

public enum ArmorType
{
    CLOTH,
    PADDED_CLOTH,
    LEATHER,
    SCALE,
    MAIL,
    PLATE
}
#endregion
public enum CreatureType
{
    HUMANOID,
    BEAST,
    VOID
}

#region Spells
public enum HarmfulStatusEffectType
{
    STUN,
    ROOT,
    DAZED,
    BLIND,
    DAMAGE_OVER_TIME,
    DECREASE_AVOID_POTENCY,
    DECREASE_HIT_POTENCY,
    DECREASE_CRITICAL_POTENCY,
    DECREASE_PHYSICAL_RESISTANCE,
    DECREASE_MAGIC_RESISTANCE,
    DECREASE_MAX_HEALTH,
    DECREASE_MAX_SPELL_RESOURCE,
    DECREASE_MAGIC_INTENSITY,
    DECREASE_PHYSICAL_INTENSITY,
    INCREASE_DAMAGE_RECEIVED,
    DECREASE_HEALING_RECEIVED
}

public enum BeneficialStatusEffectType
{
    ANTI_CC,
    PRECISE,
    TRUESTRIKE,
    EVASIVE,
    HEALING_OVER_TIME,
    INCREASE_AVOID_POTENCY,
    INCREASE_HIT_POTENCY,
    INCREASE_CRITICAL_POTENCY,
    INCREASE_PHYSICAL_RESISTANCE,
    INCREASE_MAGIC_RESISTANCE,
    INCREASE_MAX_HEALTH,
    INCREASE_MAX_SPELL_RESOURCE,
    INCREASE_MAGIC_INTENSITY,
    INCREASE_PHYSICAL_INTENSITY,
    DECREASE_DAMAGE_RECEIVED,
    INCREASE_HEALING_RECEIVED
}

public enum IntensityPurpose
{
    DAMAGE,
    HEAL
}

public enum PrimaryIntensityType
{
    PHYSICAL,
    MAGICAL
}

public enum SecondaryIntensityType
{
    BLUNT,
    PIERCING,
    SLASHING,
    FIRE,
    WATER,
    LIGHTNING,
    EARTH,
    LIGHT,
    DARK
}
#endregion Spells
