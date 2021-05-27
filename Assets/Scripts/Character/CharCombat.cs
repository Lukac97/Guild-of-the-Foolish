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
}
