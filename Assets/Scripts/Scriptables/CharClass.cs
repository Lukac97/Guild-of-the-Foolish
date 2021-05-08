using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Class", menuName = "Classes/Class")]
public class CharClass : ScriptableObject
{
    public string className;
    public Sprite classIcon;
    public Attributes startingAttributes;

    [Header("Usable Weapons/Armor")]
    public List<WeaponType> viableWeaponTypes;
    public List<ArmorType> viableArmorTypes;

}
