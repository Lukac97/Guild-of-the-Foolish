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

    private void Start()
    {
        if (Instance == null)
            _instance = this;
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
