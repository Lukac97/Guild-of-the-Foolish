using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public WeaponWielding weaponWielding;
    public WeaponRange weaponRange;
    public WeaponType weaponType;
    [Header("Attributes")]
    public Attributes attributes;
    public int level;
    public float attackDamageMultiplier;
    public float armorValue;

}
