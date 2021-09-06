using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRules : MonoBehaviour
{
    private static GlobalRules _instance;
    public static GlobalRules Instance { get { return _instance; } }

    public static int attributePointsPerLevel = 5;
    public static int maxCombatTurns = 20;
    public static int maxCombatSpells = 6;
    public static Vector2 equipmentAttackMultiplierRange = new Vector2(1, 1.25f);
    private static float levelOneExp = 100.0f;

    private void Start()
    {
        if (Instance == null)
            _instance = this;
    }

    public static float maxExperienceForLevel(int level)
    {
        return levelOneExp * Mathf.Pow(1.25f, level - 1);
    }

    public static int GetEquipmentAttributePointsForLevel(int level)
    {
        return Mathf.RoundToInt(2 + Mathf.Pow(level, 2) / 5);
    }

    public static int GetEquipmentArmorPointsForLevel(int level)
    {
        return Mathf.RoundToInt(2 + Mathf.Pow(level, 2) / 5);
    }
}
