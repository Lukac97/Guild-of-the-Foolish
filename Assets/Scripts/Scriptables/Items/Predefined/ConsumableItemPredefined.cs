using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumable")]
public class ConsumableItemPredefined : ScriptableObject, PredefinedItemsInterface
{
    public ConsumableItem consumableItem;
}
