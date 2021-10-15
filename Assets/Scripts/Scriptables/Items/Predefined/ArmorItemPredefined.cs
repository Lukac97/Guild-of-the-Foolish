using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorItemPredefined : ScriptableObject, PredefinedItemsInterface
{
    public ArmorItem armorItem;
}
