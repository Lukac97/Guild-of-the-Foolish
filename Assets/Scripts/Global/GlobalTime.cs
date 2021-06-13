using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTime : MonoBehaviour
{
    private static GlobalTime _instance;
    public static GlobalTime Instance
    {
        get
        {
            return _instance;
        }
    }

    public static List<GlobalAction> globalActions;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        globalActions = new List<GlobalAction>();
    }

    private void FixedUpdate()
    {
        List<GlobalAction> newGlobalActions = new List<GlobalAction>(globalActions);
        foreach(GlobalAction gA in globalActions)
        {
            if(gA.TimeChanged(Time.deltaTime))
            {
                newGlobalActions.Remove(gA);
            }
        }
        globalActions = new List<GlobalAction>(newGlobalActions);
    }

    public static void CreateCombatEncounterAction(CombatEncounter cbE)
    {
        globalActions.Add(new CombatEncounterAction(cbE, 1));
    }

    public static void CreateCharacterMoveAction(CharStats charStats, Location loc)
    {
        globalActions.Add(new CharacterMoveAction(charStats, loc, 1));
    }

}
