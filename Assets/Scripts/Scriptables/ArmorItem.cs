using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorItem : Item
{
    public List<ArmorSlot> itemSlots;
    [Header("Attributes")]
    public Attributes attributes;
    public float armorValue;
    public int level;
    [Tooltip("Skip if its jewelry.")]
    public ArmorType armorType;
}
