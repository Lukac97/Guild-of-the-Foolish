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

    public ArtisanProfession(ArtisanProfessionMould apMould)
    {
        professionName = apMould.artisanProfessionName;
        level = 1;
        availableRecipes = new List<Recipe>(apMould.listOfRecipes);
        artisanProfessionMould = apMould;
    }
}
