using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorItem : Item
{
    public ArmorSlot itemSlot;
    [Header("Attributes")]
    public Attributes attributes;
    public float armorValue;
    [Tooltip("Skip if its jewelry.")]
    public ArmorType armorType;
}
