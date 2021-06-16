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

        combatLogger = new CombatLogger();
        combatLogger.InitCombatLogger();

        List<AppliedStatusEffect> startSE = new List<AppliedStatusEffect>();
        int turnOutcome = 2;

        for (int turnNumber = 1; turnNumber <= GlobalRules.maxCombatTurns; turnNumber++)
        {
            combatLogger.AddTurnNumberLog(turnNumber);
            //TODO: Apply status effects to character

            turnOutcome = SingleCharTurn(character, enemy, false);

            if (turnOutcome == 0)
            {
                outcome = 0;  //character died - defeat
                break;
            }
            else if (turnOutcome == 1)
            {
                outcome = 1; //enemy died - victory
                break;
            }

            //--------------------------Enemy turn--------------------------------------
            //TODO: Apply status effects to character

            turnOutcome = SingleCharTurn(enemy, character, true);
            if(turnOutcome == 0)
            {
                outcome = 1;  //enemy died - victory
                break;
            }
            else if(turnOutcome == 1)
            {
                outcome = 0; //character died - defeat
                break;
            }
        }
        combatLogger.AddFinishLog(character.participantName, enemy.participantName, outcome);
        enemy.combatHandler.EndOfCombat();
        character.combatHandler.EndOfCombat();

        combatReward = new CombatWinReward();
        combatReward.GenerateYield(enemy.combatHandler, outcome);
        //TODO: Log to file when finished calculating combat, then read when necessary...
        finalOutcome = outcome;
        UninitiateCombat();
        return outcome;
    }

    private int SingleCharTurn(CombatParticipant attackingChar, CombatParticipant defendingChar, bool isEnemyTurn)
    {
        //RETURNS:  0 - attackingChar died, 1 - defendingChar died, 2 - nothing happened
        List<AppliedStatusEffect> startSE = new List<AppliedStatusEffect>();
        UsedSpellResult intensity = null;

        //Check if character dead
        if (attackingChar.combatHandler.isInjured)
        {
            return 0;
        }

        startSE = attackingChar.combatHandler.TurnStart();
        //Check if character dead
        if (attackingChar.combatHandler.isInjured)
        {
            return 0;
        }

        combatLogger.AddStatusEffectsLog(attackingChar.participantName, startSE, isEnemyTurn);
        if (!attackingChar.combatHandler.isStunned)
        {
            intensity = attackingChar.combatHandler.ChooseSpell(defendingChar.combatHandler);
            combatLogger.AddLog(attackingChar.participantName, defendingChar.participantName, intensity, isEnemyTurn);
        }

        if (defendingChar.combatHandler.isInjured)
        {
            return 1;
        }

        return 2;
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
