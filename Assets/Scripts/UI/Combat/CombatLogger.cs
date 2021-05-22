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

    public void AddLog(string casterName, string targetName, CombatSpell spell, float intensity, bool isEnemyTurn)
    {
        GameObject newLog = InstantiateCombatLog();
        TextMeshProUGUI tmp = newLog.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = ColorText(casterName, GlobalInput.Instance.charNameColor)
            + " used " + ColorText(spell.name, GlobalInput.Instance.spellColor)
            + " on " + ColorText(targetName, GlobalInput.Instance.enemyNameColor);
        if (spell.combatSpellType == CombatSpellType.HARMFUL)
            tmp.text += ", dealt " + ColorText(intensity.ToString(), GlobalInput.Instance.damageColor) + " damage.";
        else if (spell.combatSpellType == CombatSpellType.BENEFICIAL)
            tmp.text += ", healed " + ColorText(intensity.ToString(), GlobalInput.Instance.healColor) + " health.";

        ColorLogBackground(newLog, isEnemyTurn);
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

    private string ColorText(string text, Color clr)
    {
        string newText = "<color=#" + ColorUtility.ToHtmlStringRGB(clr).ToString() + ">" + text + "</color>";
        return newText;
    }
}
