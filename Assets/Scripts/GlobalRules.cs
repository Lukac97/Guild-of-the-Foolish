using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRules : MonoBehaviour
{
    private static GlobalRules _instance;
    public static GlobalRules Instance { get { return _instance; } }

    public int attributePointsPerLevel;
    public int maxCombatTurns;

    private void Start()
    {
        if (Instance == null)
            _instance = this;
    }
}
