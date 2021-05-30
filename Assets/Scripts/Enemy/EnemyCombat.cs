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
        combatSpells = new List<EquippedCombatSpell>();
        foreach (CombatSpell combatSpell in enemyMould.combatSpells)
        {
            if (combatSpell.spellLevel <= level)
            {
                combatSpells.Add(new EquippedCombatSpell(combatSpell));
            }
        }
        isCreatedForBattle = true;
        combatEncounter = originEncounter;
    }

    public override int GetLevel()
    {
        return enemyStats.level;
    }

    //public override EquippedCombatSpell ChooseSpell()
    //{
    //    CombatSpellType optimalType = CombatSpellType.HARMFUL;
    //    if (combatStats.currentHealth < combatStats.maxHealth / 2)
    //        optimalType = CombatSpellType.BENEFICIAL;

    //    List<EquippedCombatSpell> availableSpells = new List<EquippedCombatSpell>();
    //    foreach(EquippedCombatSpell spell in combatSpells)
    //    {
    //        if (!spell.IsOnCooldown())
    //            availableSpells.Add(spell);
    //    }

    //    List<EquippedCombatSpell> optimalSpells = new List<EquippedCombatSpell>();
    //    List<EquippedCombatSpell> nonOptimalSpells = new List<EquippedCombatSpell>();
    //    foreach (EquippedCombatSpell spell in availableSpells)
    //    {
    //        if (spell.combatSpell.combatSpellType == optimalType)
    //            optimalSpells.Add(spell);
    //        else
    //            nonOptimalSpells.Add(spell);
    //    }

    //    //TODO: More advanced AI for choosing spells
    //    if (optimalSpells != null)
    //        return optimalSpells[0];
    //    if (nonOptimalSpells != null)
    //        return nonOptimalSpells[0];
    //    return null;
    //}
}
