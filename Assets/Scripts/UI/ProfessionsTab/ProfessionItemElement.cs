using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ProfessionItemElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemIconHandler icon;

    public TextMeshProUGUI quantity;
    public TextMeshProUGUI level;

    public bool isIngredient;
    public Item item;

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.ShowItemDetailsDisplay(this, false);
    }

    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.HideItemDetailsDisplay();
    }

    public void AssignPredefinedItem(Item initItem)
    {
        item = initItem;
        icon.InitItemIconHandler(initItem);
        UpdateProfessionItemDetails();
    }

    public void UpdateProfessionItemDetails()
    {

        if (quantity != null)
        {
            if (isIngredient)
            {
                Recipe.IngredientAvailability ingA =
                    ProfessionTabMain.Instance.currentRecipeUISelected.GetIngredientAvailability(item);
                quantity.text = ingA.amountAvailable.ToString() + " / " + ingA.amountNeeded.ToString();
            }
            else
            {
                Recipe.ResultingItem resultingItem = ProfessionTabMain.Instance.currentRecipeUISelected.
                    linkedRecipe.GetResultingItem(item);
                if (resultingItem != null)
                {
                    quantity.text = resultingItem.quantity.ToString();
                }
            }

            if (level != null)
            {
                level.text = item.level.ToString();
            }
        }
    }

}
