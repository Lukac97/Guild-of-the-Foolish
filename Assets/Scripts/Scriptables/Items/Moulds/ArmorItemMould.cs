using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Mould", menuName = "Items/Armor Mould")]
public class ArmorItemMould : ItemMould
{
    public ArmorSlot itemSlot;
    [Header("Attributes")]
    [Tooltip("Skip if its jewelry.")]
    public ArmorType armorType;
}
