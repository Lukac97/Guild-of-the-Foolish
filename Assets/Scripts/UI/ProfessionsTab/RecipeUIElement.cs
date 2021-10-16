using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeUIElement : MonoBehaviour
{
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI canCreateAmount;

    private Recipe linkedRecipe;

    public void InitRecipeUIElement(Recipe recipe)
    {
        if (recipe == null)
        {
            GetComponentInParent<ProfessionTabMain>().RemoveExistingRecipeElement(this);
        }
        linkedRecipe = recipe;

        recipeName.text = recipe.recipeName;
        UpdateCanMake();
    }

    public void UpdateCanMake()
    {
        List<int> availablePerIngredient = new List<int>();
        foreach (Recipe.IngredientNeeded ingrNeed in linkedRecipe.neededIngredients)
        {
            ItemObject itemObject = GuildInventory.Instance.FindItem(ingrNeed.ingredientPredef.ingredientItem);
            if (itemObject == null)
            {
                availablePerIngredient.Add(0);
                continue;
            }
            availablePerIngredient.Add(itemObject.quantity / ingrNeed.quantity);
        }

        int minAmount = Mathf.Min(availablePerIngredient.ToArray());

        canCreateAmount.text = minAmount == 0 ? "" : minAmount.ToString();
    }
}
