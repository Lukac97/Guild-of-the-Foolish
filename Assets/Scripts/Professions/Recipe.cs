using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Recipe", menuName = "Professions/Recipe")]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public class IngredientNeeded
    {
        public IngredientItemPredefined ingredientPredef;
        public int quantity;
    }

    [System.Serializable]
    public class ResultingItem
    {
        public PredefinedItemsInterface predefinedItem;
        public int quantity;
    }

    [SerializeField] private List<IngredientNeeded> neededIngredients;
    [SerializeField] private int recipeLevel;
    [SerializeField] private List<ResultingItem> resultingItems;
}
