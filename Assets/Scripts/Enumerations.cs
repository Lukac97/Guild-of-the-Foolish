using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    DAMAGE_OVER_TIME
}

public enum BeneficialStatusEffectType
{
    HEALING_OVER_TIME,
    ANTI_CC
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
