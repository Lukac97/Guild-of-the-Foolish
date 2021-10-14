using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCombat : CombatHandler
{
    private CharStats charStats;
    private CharEquipment charEquipment;

    private void Awake()
    {
        if (combatSpells.Count <= GlobalRules.maxCombatSpells)
        {
            for (int i = combatSpells.Count; i < GlobalRules.maxCombatSpells; i++)
            {
                combatSpells.Add(new EquippedCombatSpell());
            }
        }
        else
        {
            combatSpells = combatSpells.GetRange(0, GlobalRules.maxCombatSpells);
        }
    }

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

        foreach(EquippedCombatSpell eqSpell in combatSpells)
        {
            if(eqSpell.combatSpell == null)
            {
                eqSpell.AssignSpell(newCombatSpell);
                CharactersController.ChangedChosenSpells.Invoke();
                return true;
            }
        }
        return false;
    }

    public bool AddCombatSpellToSlot(CombatSpell newCombatSpell, int slotNr)
    {
        if (slotNr >= GlobalRules.maxCombatSpells)
            return false;
        if (IndexOfEquippedCombatSpell(newCombatSpell) >= 0)
            return false;
        if (slotNr < combatSpells.Count)
        {
            combatSpells[slotNr].AssignSpell(newCombatSpell);
        }
        else
        {
            for(int i = combatSpells.Count; i < slotNr; i++)
            {
                combatSpells.Add(new EquippedCombatSpell());
            }
            combatSpells.Add(new EquippedCombatSpell(newCombatSpell));
        }
        CharactersController.ChangedChosenSpells.Invoke();
        return true;
    }

    public bool RemoveCombatSpellFromSlot(int slotNr)
    {
        if (slotNr < 0)
            return false;
        combatSpells[slotNr].AssignSpell(null);
        CharactersController.ChangedChosenSpells.Invoke();
        return true;
    }

    public bool RemoveCombatSpell(CombatSpell remCombatSpell)
    {
        int idxCS = IndexOfEquippedCombatSpell(remCombatSpell);
        if (idxCS < 0)
            return false;
        combatSpells[idxCS].AssignSpell(null);
        CharactersController.ChangedChosenSpells.Invoke();
        return true;
    }

    public bool SwapCombatSpellSlots(int firstSlot, int secondSlot)
    {
        if (firstSlot >= combatSpells.Count | secondSlot >= combatSpells.Count)
            return false;
        if (combatSpells[firstSlot] == null | combatSpells[secondSlot] == null)
            return false;
        if (firstSlot == secondSlot)
            return true;
        CombatSpell tmpFirstSpell = combatSpells[firstSlot].combatSpell;
        CombatSpell tmpSecondSpell = combatSpells[secondSlot].combatSpell;
        combatSpells[firstSlot].AssignSpell(null);
        combatSpells[secondSlot].AssignSpell(tmpFirstSpell);
        combatSpells[firstSlot].AssignSpell(tmpSecondSpell);
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
