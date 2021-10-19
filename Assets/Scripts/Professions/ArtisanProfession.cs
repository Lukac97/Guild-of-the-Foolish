using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArtisanProfession
{
    public string professionName;
    public int level;
    public List<Recipe> availableRecipes;

    private ArtisanProfessionMould artisanProfessionMould;

    public float currentExperience;
    public float neededExperienceForNextLevel;

    public ArtisanProfession(ArtisanProfessionMould apMould)
    {
        professionName = apMould.artisanProfessionName;
        level = 1;
        currentExperience = 0;
        availableRecipes = new List<Recipe>(apMould.listOfRecipes);
        artisanProfessionMould = apMould;
        neededExperienceForNextLevel = GlobalRules.MaxExperienceForProfessionLevel(level);
    }

    public bool CraftRecipe(Recipe recipeToCraft)
    {
        bool isSuccessful = recipeToCraft.CraftThisRecipe();
        if (isSuccessful)
            AddExperience(recipeToCraft);
        return isSuccessful;
    }

    public void AddExperience(Recipe recipeCrafted)
    {
        float experienceToGain = GlobalRules.GetRecipeExp(recipeCrafted.recipeLevel);
        if(currentExperience + experienceToGain >= neededExperienceForNextLevel)
        {
            float leftoverExperience = currentExperience + experienceToGain - neededExperienceForNextLevel;
            LevelUpProfession(leftoverExp: leftoverExperience);
        }
        else
        {
            currentExperience += experienceToGain;
        }

        ProfessionTabMain.Instance.UpdateProfessionDetails();
    }

    public void LevelUpProfession(float leftoverExp = 0)
    {
        float newLeftoverExp = leftoverExp;
        level += 1;
        neededExperienceForNextLevel = GlobalRules.MaxExperienceForProfessionLevel(level);
        while (newLeftoverExp > neededExperienceForNextLevel)
        {
            newLeftoverExp -= neededExperienceForNextLevel;
            level += 1;
            neededExperienceForNextLevel = GlobalRules.MaxExperienceForProfessionLevel(level);
        }
        currentExperience = newLeftoverExp;
    }

}
