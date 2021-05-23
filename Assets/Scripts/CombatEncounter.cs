using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    public class CombatParticipant
    {
        public CombatHandler combatHandler;
        public string participantName;
        public bool isFriendly;

        public CombatParticipant(CombatHandler cbh, string partName, bool friendly)
        {
            combatHandler = cbh;
            participantName = partName;
            isFriendly = friendly;
        }
    }

    public GameObject enemyPrefab;

    public List<GameObject> activeEnemies;
    public CombatParticipant character;
    public CombatParticipant enemy;

    public void InitiateCombat(CharStats charStats, Location.PossibleEnemy enemyFromLoc, Location loc)
    {
        GameObject gO = Instantiate(enemyPrefab, transform);
        activeEnemies.Add(gO);
        int selectedLvl = Random.Range(enemyFromLoc.minLevel, enemyFromLoc.maxLevel);

        EnemyStats eStats = gO.GetComponent<EnemyStats>();
        EnemyCombat eCombat = gO.GetComponent<EnemyCombat>();
        eStats.InitEnemyStats(enemyFromLoc.enemyMould, selectedLvl, loc);
        eCombat.InitEnemyCombat(enemyFromLoc.enemyMould, selectedLvl, this);

        character = new CombatParticipant(charStats.GetComponent<CombatHandler>(), charStats.characterName, true);
        enemy = new CombatParticipant(gO.GetComponent<CombatHandler>(), eStats.enemyName, false);

        ScreensController.Instance.ActivateCombatScreen(true);
    }

    public int SimulateCombat()
    {
        int outcome = 2; // 0 - defeat, 1 - victory, 2 - tie

        UsedSpellResult intensity = null;

        CombatLogger.Instance.ClearLog();

        for (int turnNumber = 1; turnNumber <= GlobalRules.Instance.maxCombatTurns; turnNumber++)
        {
            //TODO: Apply status effects to character
            character.combatHandler.TurnStart();
            //Check if character dead
            if (character.combatHandler.isInjured)
            {
                outcome = 0;
                break;
            }

            if (character.combatHandler.isStunned)
                CombatLogger.Instance.AddCasterStateLog(character.participantName, "stunned");
            else
            {
                intensity = character.combatHandler.ChooseSpell(enemy.combatHandler);
                CombatLogger.Instance.AddLog(character.participantName, enemy.participantName, intensity, false);
            }
            if (character.combatHandler.isInjured)
            {
                outcome = 0;
                break;
            }

            //--------------------------Enemy turn--------------------------------------
            //TODO: Apply status effects to character
            enemy.combatHandler.TurnStart();
            //Check if character dead
            if (enemy.combatHandler.isInjured)
            {
                outcome = 1;
                break;
            }

            if (enemy.combatHandler.isStunned)
                CombatLogger.Instance.AddCasterStateLog(enemy.participantName, "stunned");
            else
            {
                intensity = enemy.combatHandler.ChooseSpell(character.combatHandler);
                CombatLogger.Instance.AddLog(enemy.participantName, character.participantName, intensity, true);
            }
            if (enemy.combatHandler.isInjured)
            {
                outcome = 1;
                break;
            }
        }
        CombatLogger.Instance.AddFinishLog(character.participantName, enemy.participantName, outcome);

        //TODO: Log to file when finished calculating combat, then read when necessary...
        UninitiateCombat();
        return outcome;
    }

    public void UninitiateCombat()
    {
        GetComponentInParent<CombatEncounterController>().DestroyEncounter(this);
    }

}
