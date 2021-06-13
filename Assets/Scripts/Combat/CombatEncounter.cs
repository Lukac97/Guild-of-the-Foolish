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

    [HideInInspector]
    public CombatWinReward combatReward;
    [HideInInspector]
    public CombatLogger combatLogger;
    public int finalOutcome = -1;
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

        //Global action
        GlobalTime.CreateCombatEncounterAction(this);
    }

    public int SimulateCombat()
    {
        int outcome = 2; // 0 - defeat, 1 - victory, 2 - tie

        UsedSpellResult intensity = null;
        combatLogger = new CombatLogger();
        combatLogger.InitCombatLogger();

        List<AppliedStatusEffect> startSE = new List<AppliedStatusEffect>();

        for (int turnNumber = 1; turnNumber <= GlobalRules.maxCombatTurns; turnNumber++)
        {
            combatLogger.AddTurnNumberLog(turnNumber);
            //TODO: Apply status effects to character
            startSE = character.combatHandler.TurnStart();
            //Check if character dead
            if (character.combatHandler.isInjured)
            {
                outcome = 0;
                break;
            }

            combatLogger.AddStatusEffectsLog(character.participantName, startSE, false);
            if (!character.combatHandler.isStunned)
            {
                intensity = character.combatHandler.ChooseSpell(enemy.combatHandler);
                combatLogger.AddLog(character.participantName, enemy.participantName, intensity, false);
            }
            if (character.combatHandler.isInjured)
            {
                outcome = 0;
                break;
            }

            //--------------------------Enemy turn--------------------------------------
            //TODO: Apply status effects to character
            startSE = enemy.combatHandler.TurnStart();
            //Check if character dead
            if (enemy.combatHandler.isInjured)
            {
                outcome = 1;
                break;
            }

            combatLogger.AddStatusEffectsLog(enemy.participantName, startSE, true);
            if (!enemy.combatHandler.isStunned)
            {
                intensity = enemy.combatHandler.ChooseSpell(character.combatHandler);
                combatLogger.AddLog(enemy.participantName, character.participantName, intensity, true);
            }

            if (enemy.combatHandler.isInjured)
            {
                outcome = 1;
                break;
            }
        }
        combatLogger.AddFinishLog(character.participantName, enemy.participantName, outcome);

        combatReward = new CombatWinReward();
        combatReward.GenerateYield(enemy.combatHandler, outcome);
        //TODO: Log to file when finished calculating combat, then read when necessary...
        finalOutcome = outcome;
        UninitiateCombat();
        return outcome;
    }

    public void UninitiateCombat()
    {
        CombatWindowController.Instance.ActivateThisPopUp(this);
    }

    public void DestroyThisCombatEncounter()
    {
        GetComponentInParent<CombatEncounterController>().DestroyEncounter(this);
    }

}
