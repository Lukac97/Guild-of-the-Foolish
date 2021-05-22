using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    public GameObject enemyPrefab;

    public List<GameObject> activeEnemies;
    public CombatHandler character;
    public CombatHandler enemy;

    private string characterName;
    private string enemyName;

    public void InitiateCombat(CharStats charStats, Location.PossibleEnemy enemyFromLoc, Location loc)
    {
        GameObject gO = Instantiate(enemyPrefab, transform);
        activeEnemies.Add(gO);
        int selectedLvl = Random.Range(enemyFromLoc.minLevel, enemyFromLoc.maxLevel);

        EnemyStats eStats = gO.GetComponent<EnemyStats>();
        EnemyCombat eCombat = gO.GetComponent<EnemyCombat>();
        eStats.InitEnemyStats(enemyFromLoc.enemyMould, selectedLvl, loc);
        eCombat.InitEnemyCombat(enemyFromLoc.enemyMould, selectedLvl, this);
        character = charStats.GetComponent<CombatHandler>();
        enemy = gO.GetComponent<CombatHandler>();
        characterName = charStats.characterName;
        enemyName = eStats.enemyName;
        ScreensController.Instance.ActivateCombatScreen(true);
    }

    public int SimulateCombat()
    {
        bool keepFighting = true;
        int turnNumber = 1;
        int outcome = 2; // 0 - defeat, 1 - victory, 2 - tie

        CombatSpell chosenSpell;
        float intensity;

        CombatLogger.Instance.ClearLog();

        while (keepFighting)
        {
            //TODO: Apply status effects to character

            //Check if character dead
            if(character.isInjured)
            {
                outcome = 0;
                break;
            }
            chosenSpell = character.combatSpells[0];
            intensity = chosenSpell.UseSpell(character, enemy);
            CombatLogger.Instance.AddLog(characterName, enemyName, chosenSpell, intensity, false);
            if(character.isInjured)
            {
                outcome = 0;
                break;
            }
            //--------------------------Enemy turn--------------------------------------
            //TODO: Apply status effects to character

            //Check if character dead
            if (enemy.isInjured)
            {
                outcome = 1;
                break;
            }
            chosenSpell = enemy.combatSpells[0];
            intensity = chosenSpell.UseSpell(enemy, character);
            CombatLogger.Instance.AddLog(enemyName, characterName, chosenSpell, intensity, true);
            if (enemy.isInjured)
            {
                outcome = 1;
                break;
            }

            turnNumber += 1;
            if (turnNumber == 40)
                keepFighting = false;
        }
        CombatLogger.Instance.AddFinishLog(characterName, enemyName, outcome);

        //TODO: Log to file when finished calculating combat, then read when necessary...
        UninitiateCombat();
        return outcome;
    }

    public void UninitiateCombat()
    {
        GetComponentInParent<CombatEncounterController>().DestroyEncounter(this);
    }
}
