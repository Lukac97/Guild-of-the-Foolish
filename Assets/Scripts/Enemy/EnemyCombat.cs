using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CombatHandler
{
    private EnemyStats enemyStats;

    private bool isCreatedForBattle = false;
    private CombatEncounter combatEncounter;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        UpdateEnemyCombat();
        FillResources();
        if (isCreatedForBattle)
            combatEncounter.SimulateCombat();
    }
    public void UpdateEnemyCombat()
    {
        combatStats.CalculateCombatStats(enemyStats.enemyAttributes);
    }

    public void InitEnemyCombat(EnemyMould enemyMould, int level, CombatEncounter originEncounter)
    {
        combatSpells = new List<CombatSpell>();
        foreach (CombatSpell combatSpell in enemyMould.combatSpells)
        {
            if (combatSpell.spellLevel <= level)
            {
                combatSpells.Add(combatSpell);
            }
        }
        isCreatedForBattle = true;
        combatEncounter = originEncounter;
    }
}
