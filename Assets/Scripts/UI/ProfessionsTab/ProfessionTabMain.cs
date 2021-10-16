using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfessionTabMain : MonoBehaviour
{
    public GameObject recipeUIPrefab;
    public GameObject recipesPanel;

    [Header("Dropdown")]
    public TMP_Dropdown professionDropdown;

    public ArtisanProfession currentProfession;
    private List<ArtisanProfession> availableProfessions = new List<ArtisanProfession>();
    private List<RecipeUIElement> displayedRecipes = new List<RecipeUIElement>();

    private void Start()
    {
        GuildInventory.Instance.InventoryChanged += UpdateExistingRecipes;
        currentProfession = null;
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
            RedrawRecipes();
        }
    }
    public void OnDropdownChoice(int professionNr)
    {
        SetCurrentProfession(availableProfessions[professionNr]);
    }

    public void SetCurrentProfession(ArtisanProfession newProfession)
    {
        currentProfession = newProfession;
        RedrawRecipes();
    }

    private void RedrawRecipes()
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
}
