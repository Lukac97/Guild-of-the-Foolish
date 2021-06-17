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

    public bool isManualEncounter;
    
    [Header("Encounter Options")]
    [Space(6)]
    public GameObject enemyPrefab;

    public List<GameObject> activeEnemies;
    public CombatParticipant character;
    public CombatParticipant enemy;

    [HideInInspector]
    public CombatWinReward combatReward;
    [HideInInspector]
    public CombatLogger combatLogger;

    [Header("Manual combat stuff")]
    public int finalOutcome = -1;
    public int manualTurnNumber = 1;
    public bool manualIsPlayersTurn;
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

        //PLACEHOLDER
        isManualEncounter = true;

        //Global action
        if (isManualEncounter)
        {
            StartManualCombat();
        }
        else
        {
            GlobalTime.CreateCombatEncounterAction(this);
        }
    }

    #region Manual combat
    public void StartManualCombat()
    {
        //Prepare for manual combat
        combatLogger = new CombatLogger();
        combatLogger.InitCombatLogger();

        manualTurnNumber = 1;
        manualIsPlayersTurn = true;

        ManualCombatUIHandler.Instance.InitManualCombatUIHandler(this);
    }

    public void ManualChooseAction(CombatHandler.EquippedCombatSpell eqSpell)
    {
        if (!manualIsPlayersTurn)
            return;

        combatLogger.AddTurnNumberLog(manualTurnNumber);

        List<AppliedStatusEffect> startSE = new List<AppliedStatusEffect>();
        UsedSpellResult intensity = null;

        if (character.combatHandler.isInjured)
        {
            EndOfCombat(0); //char died - defeat
        }

        startSE = character.combatHandler.TurnStart();
        //Check if character dead
        if (character.combatHandler.isInjured)
        {
            EndOfCombat(0); //char died - defeat
        }

        combatLogger.AddStatusEffectsLog(character.participantName, startSE, false);
        if (!character.combatHandler.isStunned)
        {
            intensity = character.combatHandler.CastASpell(eqSpell, enemy.combatHandler);
            combatLogger.AddLog(character.participantName, enemy.participantName, intensity, false);
        }

        if (enemy.combatHandler.isInjured)
        {
            EndOfCombat(1);  //enemy died - victory
        }

        manualIsPlayersTurn = false;
        //Enemy turn
        ManualSimulateCombat();
    }

    public void ManualSimulateCombat()
    {
        int turnOutcome = 2;
        if(!manualIsPlayersTurn)
        {
            turnOutcome = SingleCharTurn(enemy, character, true);
            if (turnOutcome == 0)
            {
                EndOfCombat(1);  //enemy died - victory
            }
            else if (turnOutcome == 1)
            {
                EndOfCombat(0); //character died - defeat
            }
            manualTurnNumber += 1;
            if (manualTurnNumber > GlobalRules.maxCombatTurns)
                EndOfCombat(2); //its a tie
            manualIsPlayersTurn = true;
            ManualCombatUIHandler.Instance.TurnEnd();
        }
    }

    public void EndOfCombat(int outcome)
    {
        combatLogger.AddFinishLog(character.participantName, enemy.participantName, outcome);
        enemy.combatHandler.EndOfCombat();
        character.combatHandler.EndOfCombat();

        combatReward = new CombatWinReward();
        combatReward.GenerateYield(enemy.combatHandler, outcome);
        //TODO: Log to file when finished calculating combat, then read when necessary...
        finalOutcome = outcome;
        UninitiateCombat();
    }

    #endregion Manual combat

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
        EndOfCombat(outcome);
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
