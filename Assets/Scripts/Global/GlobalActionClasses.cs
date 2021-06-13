using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalAction
{
    public float duration;

    public virtual void OnTimeRunOut()
    {

    }

    public bool TimeChanged(float deltaTime)
    {
        duration -= deltaTime;
        if(duration <= 0)
        {
            OnTimeRunOut();
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class CombatEncounterAction : GlobalAction
{
    public CombatEncounter combatEncounter;

    public override void OnTimeRunOut()
    {
        if(combatEncounter != null)
        {
            combatEncounter.SimulateCombat();
        }
    }

    public CombatEncounterAction(CombatEncounter cbEnc, float _duration)
    {
        combatEncounter = cbEnc;
        duration = _duration;
    }
}

[System.Serializable]
public class CharacterMoveAction : GlobalAction
{
    public CharStats charStats;
    public Location loc;

    public override void OnTimeRunOut()
    {
        if (charStats != null & loc != null)
        {
            charStats.ChangeLocation(loc);
        }
    }

    public CharacterMoveAction(CharStats _charStats, Location _loc, float _duration)
    {
        charStats = _charStats;
        loc = _loc;
        duration = _duration;
    }
}

