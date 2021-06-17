using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManualCombatUIHandler : MonoBehaviour
{
    private static ManualCombatUIHandler _instance;
    public static ManualCombatUIHandler Instance
    {
        get
        {
            return _instance;
        }
    }

    public CombatEncounter combatEncounter;

    public Image characterImage;
    public Image enemyImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI enemyName;

    [Header("Logger")]
    public ScrollRect loggerScrollRect;
    public GameObject loggerPrefab;
    public GameObject loggerPanel;

    [Header("Spells")]
    public GameObject spellsPanel;
    public GameObject usableSpellContainerPrefab;

    private List<UsableSpellDisplay> usableSpellDisplays;
    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        usableSpellDisplays = new List<UsableSpellDisplay>();
    }

    public void InitManualCombatUIHandler(CombatEncounter cbEncounter)
    {
        combatEncounter = cbEncounter;

        CharStats charStats = cbEncounter.character.combatHandler.GetComponent<CharStats>();
        characterImage.sprite = charStats.characterClass.classIcon;
        characterName.text = charStats.name;

        EnemyStats enemyStats = cbEncounter.enemy.combatHandler.GetComponent<EnemyStats>();
        enemyImage.sprite = enemyStats.enemyMould.enemyIcon;
        enemyName.text = enemyStats.name;
        InitEquippedCombatSpells();
        RefreshCombatLog();
        UIScreenController.Instance.ActivateCombatScreen();
    }

    public void TurnEnd()
    {
        RefreshCombatLog();
        UpdateUsableSpells();
    }

    private void RefreshCombatLog()
    {
        ClearCombatLog();
        CombatLoggerDisplay();
    }

    private void ClearCombatLog()
    {
        foreach (Transform child in loggerPanel.transform)
            Destroy(child.gameObject);
    }

    private void CombatLoggerDisplay()
    {
        foreach (CombatLogger.SingleLog singleLog in combatEncounter.combatLogger.listOfLogs)
        {
            GameObject displayedLog = Instantiate(loggerPrefab, loggerPanel.transform);
            displayedLog.GetComponentInChildren<TextMeshProUGUI>().text = singleLog.text;
            if (singleLog.enemyTurn)
                CbLoggerFcts.ColorLogBackground(displayedLog, true);
            else if (singleLog.charTurn)
                CbLoggerFcts.ColorLogBackground(displayedLog, false);
            else if (singleLog.turnNumber)
                CbLoggerFcts.ColorLogDefault(displayedLog);
            else if (singleLog.outcome > -1)
                CbLoggerFcts.ColorLogFinishedBackground(displayedLog, singleLog.outcome);
        }
    }


    private void InitEquippedCombatSpells()
    {
        usableSpellDisplays.Clear();
        foreach(Transform child in spellsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(CombatHandler.EquippedCombatSpell eqSpell in combatEncounter.character.combatHandler.combatSpells)
        {
            GameObject gO = Instantiate(usableSpellContainerPrefab, spellsPanel.transform);
            UsableSpellDisplay usd = gO.GetComponent<UsableSpellDisplay>();
            usableSpellDisplays.Add(usd);
            usd.InitUsableSpellDisplay(eqSpell);
        }
    }

    private void UpdateUsableSpells()
    {
        foreach (UsableSpellDisplay usableSpell in usableSpellDisplays)
        {
            usableSpell.UpdateSpell();
        }
    }
}
