using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatLogger : MonoBehaviour
{
    private static CombatLogger _instance;
    public static CombatLogger Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject parentPanel;
    public GameObject logPrefab;

    private List<GameObject> listOfLogs;


    private void Start()
    {
        if (Instance == null)
            _instance = this;
        listOfLogs = new List<GameObject>();
    }


    public void ClearLog()
    {
        foreach (Transform log in parentPanel.transform)
        {
            Destroy(log.gameObject);
        }
        listOfLogs.Clear();
    }

    public GameObject InstantiateCombatLog()
    {
        GameObject newLog = Instantiate(logPrefab, parentPanel.transform);
        listOfLogs.Add(newLog);
        return newLog;
    }

    public void AddTurnNumberLog(int turnNumber)
    {
        GameObject newLog = InstantiateCombatLog();
        TextMeshProUGUI tmp = newLog.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = turnNumber + ". turn:\n";
    }

    public void AddStatusEffectsLog(string casterName, List<AppliedStatusEffect> appliedSE, bool isEnemyTurn)
    {
        //TODO: Implement better logging for status effects
        GameObject newLog = InstantiateCombatLog();
        TextMeshProUGUI tmp = newLog.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = "";
        foreach (AppliedStatusEffect appliedStatusEffect in appliedSE)
        {
            tmp.text += ColorText(casterName, GlobalInput.Instance.charNameColor)
                + GetStatusEffectSpecificString(appliedStatusEffect) +
                " ++++.";
        }

        ColorLogBackground(newLog, isEnemyTurn);
    }

    public void AddLog(string casterName, string targetName, UsedSpellResult spellResult, bool isEnemyTurn)
    {
        GameObject newLog = InstantiateCombatLog();
        TextMeshProUGUI tmp = newLog.GetComponentInChildren<TextMeshProUGUI>();
        if (spellResult == null)
        {
            tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor) + " passed the turn.";
        }
        else
        {
            tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " used " + ColorText(spellResult.spellUsed.name, GlobalInput.Instance.spellColor)
                + " on " + ColorText(targetName, GlobalInput.Instance.enemyNameColor);

            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.harmfulStatusEffectsToTarget)
            {
                tmp.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + ColorText(targetName, GlobalInput.Instance.enemyNameColor)
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }
            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.harmfulStatusEffectsToSelf)
            {
                tmp.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + ColorText(casterName, GlobalInput.Instance.charNameColor)
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }
            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.beneficialStatusEffectsToTarget)
            {
                tmp.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + ColorText(targetName, GlobalInput.Instance.enemyNameColor)
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }
            foreach(AppliedStatusEffect appliedStatusEffect in spellResult.beneficialStatusEffectsToSelf)
            {
                tmp.text += "," + GetStatusEffectSpecificString(appliedStatusEffect)
                    + ColorText(casterName, GlobalInput.Instance.charNameColor)
                    + " for " + appliedStatusEffect.statusEffect.turnDuration.ToString() + " turns";
            }

            if(spellResult.damageToTarget > 0)
                tmp.text += ", dealt "
                    + ColorText(spellResult.damageToTarget.ToString(), GlobalInput.Instance.damageColor)
                    + " damage to " + ColorText(targetName, GlobalInput.Instance.enemyNameColor);
            if(spellResult.damageToSelf > 0)
                tmp.text += ", dealt "
                    + ColorText(spellResult.damageToSelf.ToString(), GlobalInput.Instance.damageColor)
                    + " damage to " + ColorText(casterName, GlobalInput.Instance.charNameColor);
            if(spellResult.healingToTarget > 0)
                tmp.text += ", did "
                    + ColorText(spellResult.healingToTarget.ToString(), GlobalInput.Instance.healColor)
                    + " healing to " + ColorText(targetName, GlobalInput.Instance.charNameColor);
            if(spellResult.healingToSelf > 0)
                tmp.text += ", did "
                    + ColorText(spellResult.healingToSelf.ToString(), GlobalInput.Instance.healColor)
                    + " healing to " + ColorText(targetName, GlobalInput.Instance.charNameColor);
            tmp.text += ".";
        }

        ColorLogBackground(newLog, isEnemyTurn);
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
                        + ColorText(appliedStatusEffect.intensityToReceive.ToString(), GlobalInput.Instance.damageColor)
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
                        + ColorText(appliedStatusEffect.intensityToReceive.ToString(), GlobalInput.Instance.healColor)
                        + ") on ";
            }
        }

        return "";
    }

    public void AddCasterStateLog(string casterName, string stateName)
    {
        GameObject newLog = InstantiateCombatLog();
        TextMeshProUGUI tmp = newLog.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
            + " is " + stateName + ".";
    }

    public void AddFinishLog(string casterName, string targetName, int outcome)
    {
        GameObject newLog = InstantiateCombatLog();
        TextMeshProUGUI tmp = newLog.GetComponentInChildren<TextMeshProUGUI>();
        if (outcome == 1)
            tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " won the battle against " + ColorText(targetName, GlobalInput.Instance.enemyNameColor) + ".";
        else if (outcome == 0)
            tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " lost the battle against " + ColorText(targetName, GlobalInput.Instance.enemyNameColor) + ".";
        else
            tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
                + " tied in battle against " + ColorText(targetName, GlobalInput.Instance.enemyNameColor) + ".";

        ColorLogFinishedBackground(newLog, outcome);
    }

    private void ColorLogBackground(GameObject logObj, bool isEnemyTurn)
    {
        Image image = logObj.GetComponent<Image>();
        if (isEnemyTurn)
            image.color = GlobalInput.Instance.enemyTurnColor;
        else
            image.color = GlobalInput.Instance.characterTurnColor;
    }

    private void ColorLogFinishedBackground(GameObject logObj, int outcome)
    {
        Image image = logObj.GetComponent<Image>();
        if (outcome == 1)
            image.color = GlobalInput.Instance.battleWonColor;
        else if (outcome == 0)
            image.color = GlobalInput.Instance.battleLostColor;
        else
            image.color = GlobalInput.Instance.battleTiedColor;
    }

    private void ColorLogDefault(GameObject logObj)
    {
        Image image = logObj.GetComponent<Image>();
        image.color = GlobalInput.Instance.defaultLogColor;
    }

    private string ColorText(string text, Color clr)
    {
        string newText = "<color=#" + ColorUtility.ToHtmlStringRGB(clr).ToString() + ">" + text + "</color>";
        return newText;
    }
}
