using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artisan Profession", menuName = "Professions/Artisan Profession")]
public class ArtisanProfessionMould : ScriptableObject
{
    public string artisanProfessionName;
    public List<Recipe> listOfRecipes;
}