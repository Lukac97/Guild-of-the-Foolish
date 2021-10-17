using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RecipeUIElement : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI canCreateAmount;

    public List<Recipe.IngredientAvailability> availablePerIngredient;

    public Recipe linkedRecipe;

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
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        ProfessionTabMain.Instance.SelectRecipe(this);
    }

    public void UpdateCanMake()
    {
        int recipeCount = GetRecipeAvailableCount();
        canCreateAmount.text = (recipeCount == 0 ? "" : recipeCount.ToString());
    }

    public int GetRecipeAvailableCount()
    {
        return linkedRecipe.GetRecipeAvailableCount(out availablePerIngredient);
    }

    public Recipe.IngredientAvailability GetIngredientAvailability(Item item)
    {
        foreach(Recipe.IngredientAvailability ingA in availablePerIngredient)
        {
            if(ingA.itemNeeded == item)
            {
                return ingA;
            }
        }

        return null;
    }
}
