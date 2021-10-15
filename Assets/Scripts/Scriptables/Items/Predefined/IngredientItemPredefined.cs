using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Items/Ingredient")]
public class IngredientItemPredefined : ScriptableObject, PredefinedItemsInterface
{
    public IngredientItem ingredientItem;
}
