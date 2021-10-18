using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfessionTabMain : MonoBehaviour
{

    private static ProfessionTabMain _instance;
    public static ProfessionTabMain Instance { get { return _instance; } }

    public GameObject recipeUIPrefab;
    public GameObject recipesPanel;
    public CanvasGroup recipeDetailsCG;
    [SerializeField] private Button craftButton;

    [Header("Recipe details")]
    [SerializeField] private GameObject recipeResultScrollGO;
    [SerializeField] private GameObject recipeResultPanelGO;
    [SerializeField] private GameObject recipeIngredientsScrollGO;
    [SerializeField] private GameObject recipeIngredientsPanelGO;

    [SerializeField] private GameObject recipeResultItemPrefab;
    [SerializeField] private GameObject recipeIngredientItemPrefab;

    [Header("Dropdown")]
    public TMP_Dropdown professionDropdown;

    public ArtisanProfession currentProfession;
    public RecipeUIElement currentRecipeUISelected;
    private List<ArtisanProfession> availableProfessions = new List<ArtisanProfession>();
    private List<RecipeUIElement> displayedRecipes = new List<RecipeUIElement>();

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    private void Start()
    {
        GuildInventory.Instance.InventoryChanged += UpdateExistingRecipes;
        currentProfession = null;
        foreach(Transform child in recipeIngredientsPanelGO.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in recipeResultPanelGO.transform)
        {
            Destroy(child.gameObject);
        }
        GlobalFuncs.PackGridLayoutWithScroll(recipeResultScrollGO, recipeResultPanelGO, 1, 0.1f);
        GlobalFuncs.PackGridLayoutWithScroll(recipeIngredientsScrollGO, recipeIngredientsPanelGO, 1, 0.1f);
        UpdateProfessionDropdown();
    }

    private void UpdateProfessionDropdown()
    {
        availableProfessions = GuildProfessions.Instance.artisanProfessions;
        professionDropdown.ClearOptions();
        List<string> professionNames = new List<string>();
        foreach (ArtisanProfession profession in availableProfessions)
        {
            professionNames.Add(profession.professionName);
        }
        professionDropdown.AddOptions(professionNames);
        if (currentProfession == null)
        {
            professionDropdown.value = 0;
            OnDropdownChoice(0);
        }
        else
        {
            RedrawRecipesList();
        }
    }
    public void OnDropdownChoice(int professionNr)
    {
        SetCurrentProfession(availableProfessions[professionNr]);
    }

    public void SetCurrentProfession(ArtisanProfession newProfession)
    {
        currentProfession = newProfession;
        RedrawRecipesList();
        SelectRecipe(null);
    }

    public void SelectRecipe(RecipeUIElement recipeUIElement)
    {
        bool sameRecipeSelected = false;
        if (currentRecipeUISelected == recipeUIElement & recipeUIElement != null)
            sameRecipeSelected = true;
        currentRecipeUISelected = recipeUIElement;
        //Logic for displaying/undisplaying details about recipe
        if (recipeUIElement == null)
        {
            GlobalFuncs.SetActiveCanvasGroup(recipeDetailsCG, false);
        }
        else
        {
            GlobalFuncs.SetActiveCanvasGroup(recipeDetailsCG, true);
            if (!sameRecipeSelected)
            {
                DisplaySelectedRecipeDetails();
                if(recipeUIElement.availableForCrafting)
                {
                    craftButton.interactable = true;
                }
                else
                {
                    craftButton.interactable = false;
                }
            }
        }
    }

    private void DisplaySelectedRecipeDetails()
    {
        foreach(Transform child in recipeResultPanelGO.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in recipeIngredientsPanelGO.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Recipe.ResultingItem resItem in currentRecipeUISelected.linkedRecipe.resultingItems)
        {
            GameObject gO = Instantiate(recipeResultItemPrefab, recipeResultPanelGO.transform);
            ProfessionItemElement profItem = gO.GetComponent<ProfessionItemElement>();
            profItem.AssignPredefinedItem(GlobalFuncs.GetItemFromScriptableObject(resItem.predefinedItem));
        }

        foreach(Recipe.IngredientNeeded ingNeed in currentRecipeUISelected.linkedRecipe.neededIngredients)
        {
            GameObject gO = Instantiate(recipeIngredientItemPrefab, recipeIngredientsPanelGO.transform);
            ProfessionItemElement profItem = gO.GetComponent<ProfessionItemElement>();
            profItem.AssignPredefinedItem(GlobalFuncs.GetItemFromScriptableObject(ingNeed.ingredientPredef));
        }

    }

    private void RedrawRecipesList()
    {
        CleanAllRecipes();
        foreach(Recipe recipe in currentProfession.availableRecipes)
        {
            GameObject newGO = Instantiate(recipeUIPrefab, recipesPanel.transform);
            RecipeUIElement recipeUIElement = newGO.GetComponent<RecipeUIElement>();
            recipeUIElement.InitRecipeUIElement(recipe);
            displayedRecipes.Add(recipeUIElement);
        }
    }

    public void UpdateExistingRecipes()
    {
        foreach(RecipeUIElement recipeEl in displayedRecipes)
        {
            recipeEl.UpdateCanMake();
        }

        foreach(ProfessionItemElement iEl in recipeIngredientsPanelGO.GetComponentsInChildren<ProfessionItemElement>())
        {
            iEl.UpdateProfessionItemDetails();
        }

        if (currentRecipeUISelected != null)
        {
            if (currentRecipeUISelected.availableForCrafting)
            {
                craftButton.interactable = true;
            }
            else
            {
                craftButton.interactable = false;
            }
        }
    }

    public void RemoveExistingRecipeElement(RecipeUIElement recipeUIElement)
    {
        displayedRecipes.Remove(recipeUIElement);
        Destroy(recipeUIElement.gameObject);
    }

    private void CleanAllRecipes()
    {
        displayedRecipes.Clear();
        foreach(Transform child in recipesPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClickCraft()
    {
        if (currentRecipeUISelected == null)
            return;

        bool isSuccessful = currentRecipeUISelected.linkedRecipe.CraftThisRecipe();
    }
}
