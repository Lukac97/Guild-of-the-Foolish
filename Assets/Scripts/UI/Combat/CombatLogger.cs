using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatLogger
{
    [System.Serializable]
    public class SingleLog
    {
        public bool turnNumber;
        public bool enemyTurn;
        public bool charTurn;
        public int outcome; // -1 equivalent of false, 0 win, 1 lose, 2 tie
        public string text;

        public SingleLog()
        {
            turnNumber = false;
            enemyTurn = false;
            charTurn = false;
            outcome = -1;
            text = "";
        }
    }

    public List<SingleLog> listOfLogs;


    public void InitCombatLogger()
    {
        listOfLogs = new List<SingleLog>();
    }

    public SingleLog InstantiateCombatLog()
    {
        SingleLog newLog = new SingleLog();
        listOfLogs.Add(newLog);
        return newLog;
    }

    public void AddTurnNumberLog(int turnNumber)
    {
        SingleLog newLog = InstantiateCombatLog();
        newLog.text = turnNumber + ". turn:\n";
    }

    public void AddStatusEffectsLog(string casterName, List<AppliedStatusEffect> appliedSE, bool isEnemyTurn)
    {
        string newCasterName = isEnemyTurn ? ColorText(casterName, GlobalInput.Instance.enemyNameColor) : ColorText(casterName, GlobalInput.Instance.charNameColor);
        //TODO: Implement better logging for status effects
        SingleLog newLog = InstantiateCombatLog();
        foreach (AppliedStatusEffect appliedStatusEffect in appliedSE)
        {
            newLog.text += newCasterName
                + GetStatusEffectSpecificString(appliedStatusEffect) +
                " ++++.";
        }
        if (isEnemyTurn)
            newLog.enemyTurn = true;
        else
            newLog.charTurn = true;
    }

    public void AddLog(string casterName, string targetName, UsedSpellResult spellResult, bool isEnemyTurn)
    {
        string newCasterName = isEnemyTurn ? ColorText(casterName, GlobalInput.Instance.enemyNameColor) : ColorText(casterName, GlobalInput.Instance.charNameColor);
        string newTargetName = isEnemyTurn ? ColorText(targetName, GlobalInput.Instance.charNameColor) : ColorText(targetName, GlobalInput.Instance.enemyNameColor);
        SingleLog newLog = InstantiateCombatLog();
        if (spellResult == null)
        {
            newLog.text = ColorText(casterName, GlobalInput.Instance.charNameColor) + " passed the turn.";
        }
        else
        {
            newLog.text = newCasterName
                + " used " + ColorText(spellResult.spellUsed.name, GlobalInput.Instance.spellColor)
                + " on " + newTargetName;

            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.harmfulStatusEffectsToTarget)
            {
                newLog.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + newTargetName
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }
            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.harmfulStatusEffectsToSelf)
            {
                newLog.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + newCasterName
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }
            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.beneficialStatusEffectsToTarget)
            {
                newLog.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + newTargetName
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }
            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.beneficialStatusEffectsToSelf)
            {
                newLog.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + newCasterName
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }

            foreach(AppliedIntensityInstance appliedInstance in spellResult.appliedIntensityInstances)
            {
                if(appliedInstance.intensityPurpose == IntensityPurpose.HEAL)
                {
                    if(appliedInstance.onSelf)
                    {
                        newLog.text += ", did "
                            + ColorText(appliedInstance.intensity.ToString("0.00"), GlobalInput.Instance.healColor)
                            + " healing to " + newCasterName;
                    }
                    else
                    {
                        newLog.text += ", did "
                            + ColorText(appliedInstance.intensity.ToString("0.00"), GlobalInput.Instance.healColor)
                            + " healing to " + newTargetName;
                    }
                }
                else if(appliedInstance.intensityPurpose == IntensityPurpose.DAMAGE)
                {
                    if (appliedInstance.onSelf)
                    {
                        newLog.text += ", dealt "
                            + ColorText(appliedInstance.intensity.ToString("0.00"), GlobalInput.Instance.damageColor)
                            + " damage to " + newCasterName;
                    }
                    else
                    {
                        newLog.text += ", dealt "
                            + ColorText(appliedInstance.intensity.ToString("0.00"), GlobalInput.Instance.damageColor)
                            + " damage to " + newTargetName;
                    }
                }
            }
        }

        if (isEnemyTurn)
            newLog.enemyTurn = true;
        else
            newLog.charTurn = true;
    }

    private string GetStatusEffectSpecificString(AppliedStatusEffect appliedStatusEffect)
    {
        if(appliedStatusEffect.statusEffect.GetType() == typeof(HarmfulStatusEffect))
        {
            HarmfulStatusEffect newStatusEffect = (HarmfulStatusEffect)appliedStatusEffect.statusEffect;
            switch(newStatusEffect.statusEffectType)
            {
                case HarmfulStatusEffectType.DAMAGE_OVER_TIME:
                    return " inflicted damage over time ("
                        + ColorText(appliedStatusEffect.intensityToReceive.intensity.ToString("0.00"), GlobalInput.Instance.damageColor)
                        + ") to ";
                case HarmfulStatusEffectType.STUN:
                    return " stunned ";
            }
        }
        else if (appliedStatusEffect.statusEffect.GetType() == typeof(BeneficialStatusEffect))
        {
            BeneficialStatusEffect newStatusEffect = (BeneficialStatusEffect)appliedStatusEffect.statusEffect;
            switch (newStatusEffect.statusEffectType)
            {
                case BeneficialStatusEffectType.ANTI_CC:
                    return " put anti-cc blessing on ";
                case BeneficialStatusEffectType.HEALING_OVER_TIME:
                    return " put healing over time ("
                        + ColorText(appliedStatusEffect.intensityToReceive.intensity.ToString("0.00"), GlobalInput.Instance.healColor)
                        + ") on ";
            }
        }

        return "";
    }

    public void AddFinishLog(string casterName, string targetName, int outcome)
    {
        SingleLog newLog = InstantiateCombatLog();
        if (outcome == 1)
            newLog.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " won the battle against " + ColorText(targetName, GlobalInput.Instance.enemyNameColor) + ".";
        else if (outcome == 0)
            newLog.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " lost the battle against " + ColorText(targetName, GlobalInput.Instance.enemyNameColor) + ".";
        else
            newLog.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " tied in battle against " + ColorText(targetName, GlobalInput.Instance.enemyNameColor) + ".";

        newLog.outcome = outcome;
    }

    private string ColorText(string text, Color clr)
    {
        string newText = "<color=#" + ColorUtility.ToHtmlStringRGB(clr).ToString() + ">" + text + "</color>";
        return newText;
    }
}
