using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRules : MonoBehaviour
{
    private static GlobalRules _instance;
    public static GlobalRules Instance { get { return _instance; } }

    public static int attributePointsPerLevel = 5;
    public static int maxCombatTurns = 20;
    public static int maxCombatSpells = 3;

    private void Start()
    {
        if (Instance == null)
            _instance = this;
    }
}
