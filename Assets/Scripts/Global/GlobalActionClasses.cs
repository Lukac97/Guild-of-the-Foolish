using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalAction
{
    public int nrOfHalfCycles;

    public virtual void OnActionFinished()
    {

    }

    public bool OnHalfCycleChanged()
    {
        nrOfHalfCycles -= 1;
        return CheckIfActionFinished();
    }

    public bool CheckIfActionFinished()
    {
        if (nrOfHalfCycles <= 0)
        {
            OnActionFinished();
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class CombatEncounterAction : GlobalAction
{
    public CombatEncounter combatEncounter;

    public override void OnActionFinished()
    {
        if(combatEncounter != null)
        {
            combatEncounter.SimulateCombat();
        }
    }

    public CombatEncounterAction(CombatEncounter cbEnc, int _nrOfHalfCycles)
    {
        combatEncounter = cbEnc;
        nrOfHalfCycles = _nrOfHalfCycles;
    }
}

[System.Serializable]
public class CharacterMoveAction : GlobalAction
{
    public CharStats charStats;
    public Location loc;

    public override void OnActionFinished()
    {
        if (charStats != null && loc != null)
        {
            charStats.ChangeLocation(loc);
        }
    }

    public CharacterMoveAction(CharStats _charStats, Location _loc, int _nrOfHalfCycles)
    {
        charStats = _charStats;
        loc = _loc;
        nrOfHalfCycles = _nrOfHalfCycles;
    }
}

