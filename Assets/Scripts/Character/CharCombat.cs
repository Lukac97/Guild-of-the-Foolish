using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCombat : CombatHandler
{
    private CharStats charStats;
    private CharEquipment charEquipment;

    private void Start()
    {
        charEquipment = GetComponent<CharEquipment>();
        charStats = GetComponent<CharStats>();
        UpdateCharCombat();
    }

    public void UpdateCharCombat()
    {
        if (charStats == null)
            return;
        combatStats.CalculateCombatStats(charStats.totalAttributes);
        NotifyResourcesUpdated();
    }

    protected override void NotifyResourcesUpdated()
    {
        CharactersController.CharactersUpdated.Invoke();
        CharactersController.CharactersResourcesUpdated.Invoke();
    }

    public override int GetLevel()
    {
        return charStats.level;
    }

    public bool AddCombatSpell(CombatSpell newCombatSpell)
    {
        if (IndexOfEquippedCombatSpell(newCombatSpell) >= 0)
            return false;
        if (combatSpells.Count >= GlobalRules.maxCombatSpells)
        {
            combatSpells.RemoveAt(0);
        }

        combatSpells.Add(new EquippedCombatSpell(newCombatSpell));
        CharactersController.ChangedChosenSpells.Invoke();
        return true;
    }

    public bool RemoveCombatSpell(CombatSpell remCombatSpell)
    {
        int idxCS = IndexOfEquippedCombatSpell(remCombatSpell);
        if (idxCS < 0)
            return false;
        combatSpells.RemoveAt(idxCS);
        CharactersController.ChangedChosenSpells.Invoke();
        return true;
    }

    public int IndexOfEquippedCombatSpell(CombatSpell combatSpell)
    {
        int counter = 0;
        foreach(EquippedCombatSpell eqCombatSpell in combatSpells)
        {
            if (eqCombatSpell.combatSpell == combatSpell)
                return counter;
            counter++;
        }
        return -1;
    }

}
