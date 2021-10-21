using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRules : MonoBehaviour
{

    private static GlobalRules _instance;
    public static GlobalRules Instance { get { return _instance; } }

    public static int attributePointsPerLevel = 5;
    public static int maxCombatTurns = 20;
    public static int maxCombatSpells = 6;
    public static float equipmentAttackMultiplierRangeMin = 1.0f;
    public static float equipmentAttackMultiplierRangeMax = 1.25f;
    private static float levelOneExp = 100.0f;
    private static float levelOneProfessionExp = 50.0f;
    private static float levelOneRecipeExp = 5.0f;

    private static Dictionary<string, List<int>> inUseMouldedItemIDs = new Dictionary<string, List<int>>();

    private void Start()
    {
        if (Instance == null)
            _instance = this;
    }

    public static string CreateIDForMouldedItem(ItemMould itemMould)
    {
        string strNewID = itemMould.itemID.Replace(" ", "_").ToLower();
        if(inUseMouldedItemIDs.TryGetValue(strNewID, out List<int> existingIDs))
        {
            for(int i = 0; i < existingIDs.Count; i++)
            {
                if( existingIDs[i] != i)
                {
                    existingIDs.Insert(i, i);
                    return $"{strNewID}_%_{i}";
                }
            }
            return null;
        }
        inUseMouldedItemIDs[strNewID] = new List<int> { 0 };
        return $"{strNewID}_%_0";
    }

    public static void RemoveIDFromMouldedItem(Item mouldedItem)
    {
        string[] splitBy = { "_%_" };
        string[] listOfIDs = mouldedItem.itemID.Split(splitBy, StringSplitOptions.RemoveEmptyEntries);
        string baseID = listOfIDs[0];
        int numberID = Int32.Parse(listOfIDs[1]);

        if(inUseMouldedItemIDs.ContainsKey(baseID))
        {
            inUseMouldedItemIDs[baseID].Remove(numberID);
        }
    }

    public static float maxExperienceForLevel(int level)
    {
        return levelOneExp * Mathf.Pow(1.25f, level - 1);
    }

    public static float MaxExperienceForProfessionLevel(int level)
    {
        return levelOneProfessionExp * Mathf.Pow(1.5f, level - 1);
    }

    public static float GetRecipeExp(int recipeLevel)
    {
        return levelOneRecipeExp * Mathf.Pow(1.25f, recipeLevel - 1);
    }

    public static int GetEquipmentAttributePointsForLevel(int level)
    {
        return Mathf.RoundToInt(2 + Mathf.Pow(level, 2) / 5);
    }

    public static int GetEquipmentArmorPointsForLevel(int level)
    {
        return Mathf.RoundToInt(2 + Mathf.Pow(level, 2) / 5);
    }

    //Get percentage of how much damage should be resisted
    public static float GetMaxResistanceBasedOnAttackLevel(int level)
    {
        return 100 + (level - 1) * 20;
    }
}
