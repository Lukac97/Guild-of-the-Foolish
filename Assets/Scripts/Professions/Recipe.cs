using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Recipe", menuName = "Professions/Recipe")]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public class IngredientNeeded
    {
        public ScriptableObject ingredientPredef;
        public int quantity;
    }

    [System.Serializable]
    public class ResultingItem
    {
        public ScriptableObject predefinedItem;
        public int quantity;
    }

    [System.Serializable]
    public class IngredientAvailability
    {
        public Item itemNeeded;
        public int amountNeeded;
        public int amountAvailable;
    }

    public string recipeName;
    public List<IngredientNeeded> neededIngredients;
    public int recipeLevel;
    public List<ResultingItem> resultingItems;

    public ResultingItem GetResultingItem(Item item)
    {
        foreach(ResultingItem resItem in resultingItems)
        {
            if(GlobalFuncs.GetItemFromScriptableObject(resItem.predefinedItem) == item)
            {
                return resItem;
            }
        }
        return null;
    }

    public int GetRecipeAvailableCount(out List<IngredientAvailability> availabilityList)
    {
        List<IngredientAvailability> availablePerIngredient = new List<IngredientAvailability>();

        foreach (Recipe.IngredientNeeded ingrNeed in neededIngredients)
        {
            Item itemNeeded = GlobalFuncs.GetItemFromScriptableObject(ingrNeed.ingredientPredef);
            if (itemNeeded == null)
            {
                continue;
            }
            ItemObject itemObject = GuildInventory.Instance.FindItemByOrigin(itemNeeded);
            Recipe.IngredientAvailability ingA = new Recipe.IngredientAvailability();
            ingA.itemNeeded = itemNeeded;
            ingA.amountNeeded = ingrNeed.quantity;
            ingA.amountAvailable = itemObject == null ? 0 : itemObject.quantity;
            availablePerIngredient.Add(ingA);
        }

        availabilityList = availablePerIngredient;

        int possibleCreations = -1;
        foreach (Recipe.IngredientAvailability ingA in availablePerIngredient)
        {
            int tmp = ingA.amountAvailable / ingA.amountNeeded;
            if (possibleCreations < 0 | tmp < possibleCreations)
                possibleCreations = tmp;
        }
        return Mathf.Max(possibleCreations, 0);
    }

    public bool CraftThisRecipe()
    {
        if(GetRecipeAvailableCount(out _) > 0)
        {
            foreach(Recipe.IngredientNeeded ingrNeed in neededIngredients)
            {
                ItemObject itemObject = GuildInventory.Instance.FindItemByOrigin(
                    GlobalFuncs.GetItemFromScriptableObject(ingrNeed.ingredientPredef));
                GuildInventory.Instance.RemoveItemFromInventory(itemObject.item, ingrNeed.quantity);
            }

            foreach(ResultingItem resItem in resultingItems)
            {
                if (resItem.predefinedItem.GetType() == typeof(ArmorItemPredefined))
                {
                    GuildInventory.Instance.AddItemToInventory(GlobalFuncs.GenerateItemFromPredefined(
                        (ArmorItemPredefined)resItem.predefinedItem), resItem.quantity);
                }
                else if (resItem.predefinedItem.GetType() == typeof(WeaponItemPredefined))
                {
                    GuildInventory.Instance.AddItemToInventory(GlobalFuncs.GenerateItemFromPredefined(
                        (WeaponItemPredefined)resItem.predefinedItem), resItem.quantity);
                }
                else if (resItem.predefinedItem.GetType() == typeof(ConsumableItemPredefined))
                {
                    GuildInventory.Instance.AddItemToInventory(GlobalFuncs.GenerateItemFromPredefined(
                        (ConsumableItemPredefined)resItem.predefinedItem), resItem.quantity);
                }
                else if (resItem.predefinedItem.GetType() == typeof(IngredientItemPredefined))
                {
                    GuildInventory.Instance.AddItemToInventory(GlobalFuncs.GenerateItemFromPredefined(
                        (IngredientItemPredefined)resItem.predefinedItem), resItem.quantity);
                }
            }

            return true;
        }
        return false;
    }
}
