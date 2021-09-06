using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Mould", menuName = "Items/Weapon Mould")]
public class WeaponItemMould : ItemMould
{
    public WeaponWielding weaponWielding;
    public WeaponRange weaponRange;
    public WeaponType weaponType;
}
