using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientItem : Item
{
    public IngredientType ingredientType;

    public IngredientItem(IngredientItem _ingredientItem)
    {
        itemName = _ingredientItem.itemName;
        itemDescription = _ingredientItem.itemDescription;
        itemIcon = _ingredientItem.itemIcon;
        level = _ingredientItem.level;
        ingredientType = _ingredientItem.ingredientType;
    }
}
