using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponItemPredefined : ScriptableObject, PredefinedItemsInterface
{
    public WeaponItem weaponItem;
}
