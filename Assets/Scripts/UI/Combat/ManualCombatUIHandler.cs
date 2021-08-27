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
    public TextMeshProUGUI characterLevel;
    public TextMeshProUGUI enemyLevel;

    [Header("Sliders")]
    [SerializeField] private Slider charHpSlider;
    [SerializeField] private Slider charSrSlider;
    [Space(3)]
    [SerializeField] private TextMeshProUGUI charCurrentHp;
    [SerializeField] private TextMeshProUGUI charMaxHp;
    [SerializeField] private TextMeshProUGUI charCurrentSr;
    [SerializeField] private TextMeshProUGUI charMaxSr;
    [Space(10)]
    [SerializeField] private Slider enemyHpSlider;
    [SerializeField] private Slider enemySrSlider;
    [Space(3)]
    [SerializeField] private TextMeshProUGUI enemyCurrentHp;
    [SerializeField] private TextMeshProUGUI enemyMaxHp;
    [SerializeField] private TextMeshProUGUI enemyCurrentSr;
    [SerializeField] private TextMeshProUGUI enemyMaxSr;

    //[Header("Logger")]
    //public ScrollRect loggerScrollRect;
    //public GameObject loggerPrefab;
    //public GameObject loggerPanel;

    [Header("Spells")]
    public GameObject spellsPanel;
    public GameObject usableSpellContainerPrefab;

    private List<UsableSpellDisplay> usableSpellDisplays;
    private void Awake()
    {
        CharactersController.CharactersResourcesUpdated += UpdateCharacterResources;
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
        characterLevel.text = charStats.level.ToString();

        EnemyStats enemyStats = cbEncounter.enemy.combatHandler.GetComponent<EnemyStats>();
        enemyImage.sprite = enemyStats.enemyMould.enemyIcon;
        enemyName.text = enemyStats.enemyMould.name;
        enemyLevel.text = enemyStats.level.ToString();
        InitEquippedCombatSpells();
        UpdateCharacterResources();
        UpdateEnemyResources();
        UIScreenController.Instance.ActivateCombatScreen();
    }

    public void RefreshDisplay()
    {
        UpdateUsableSpells();
    }

    private void UpdateCharacterResources()
    {
        if (combatEncounter == null)
            return;

        // Number setting
        if (charCurrentHp != null)
        {
            charCurrentHp.text = combatEncounter.character.combatHandler.combatStats.currentHealth.ToString("N0");
        }
        if (charMaxHp != null)
        {
            charMaxHp.text = combatEncounter.character.combatHandler.combatStats.totalStats.maxHealth.ToString("N0");
        }
        if (charCurrentSr != null)
        {
            charCurrentSr.text = combatEncounter.character.combatHandler.combatStats.currentSpellResource.ToString("N0");
        }
        if (charMaxSr != null)
        {
            charMaxSr.text = combatEncounter.character.combatHandler.combatStats.totalStats.maxSpellResource.ToString("N0");
        }

        if (enemyCurrentHp != null)
        {
            enemyCurrentHp.text = combatEncounter.enemy.combatHandler.combatStats.currentHealth.ToString("N0");
        }
        if (enemyMaxHp != null)
        {
            enemyMaxHp.text = combatEncounter.enemy.combatHandler.combatStats.totalStats.maxHealth.ToString("N0");
        }
        if (enemyCurrentSr != null)
        {
            enemyCurrentSr.text = combatEncounter.enemy.combatHandler.combatStats.currentSpellResource.ToString("N0");
        }
        if (enemyMaxSr != null)
        {
            enemyMaxSr.text = combatEncounter.enemy.combatHandler.combatStats.totalStats.maxSpellResource.ToString("N0");
        }

        // Slider setting
        if (charHpSlider != null)
        {
            if (combatEncounter.character.combatHandler.combatStats.totalStats.maxHealth == 0)
                charHpSlider.value = 0;
            else
                charHpSlider.value = combatEncounter.character.combatHandler.combatStats.currentHealth
                    / combatEncounter.character.combatHandler.combatStats.totalStats.maxHealth;
        }

        if (charSrSlider != null)
        {
            if (combatEncounter.character.combatHandler.combatStats.totalStats.maxSpellResource == 0)
                charSrSlider.value = 0;
            else
                charSrSlider.value = combatEncounter.character.combatHandler.combatStats.currentSpellResource
                    / combatEncounter.character.combatHandler.combatStats.totalStats.maxSpellResource;
        }
    }

    public void UpdateEnemyResources()
    {
        if (combatEncounter == null)
            return;
        if (enemyHpSlider != null)
        {
            if (combatEncounter.enemy.combatHandler.combatStats.totalStats.maxHealth == 0)
                enemyHpSlider.value = 0;
            else
                enemyHpSlider.value = combatEncounter.enemy.combatHandler.combatStats.currentHealth
                    / combatEncounter.enemy.combatHandler.combatStats.totalStats.maxHealth;
        }
        if (enemySrSlider != null)
        {
            if (combatEncounter.enemy.combatHandler.combatStats.totalStats.maxSpellResource == 0)
                enemySrSlider.value = 0;
            else
                enemySrSlider.value = combatEncounter.enemy.combatHandler.combatStats.currentSpellResource
                    / combatEncounter.enemy.combatHandler.combatStats.totalStats.maxSpellResource;
        }
    }

    //private void RefreshCombatLog()
    //{
    //    ClearCombatLog();
    //    CombatLoggerDisplay();
    //}

    //private void ClearCombatLog()
    //{
    //    foreach (Transform child in loggerPanel.transform)
    //        Destroy(child.gameObject);
    //}

    //private void CombatLoggerDisplay()
    //{
    //    foreach (CombatLogger.SingleLog singleLog in combatEncounter.combatLogger.listOfLogs)
    //    {
    //        GameObject displayedLog = Instantiate(loggerPrefab, loggerPanel.transform);
    //        displayedLog.GetComponentInChildren<TextMeshProUGUI>().text = singleLog.text;
    //        if (singleLog.enemyTurn)
    //            CbLoggerFcts.ColorLogBackground(displayedLog, true);
    //        else if (singleLog.charTurn)
    //            CbLoggerFcts.ColorLogBackground(displayedLog, false);
    //        else if (singleLog.turnNumber)
    //            CbLoggerFcts.ColorLogDefault(displayedLog);
    //        else if (singleLog.outcome > -1)
    //            CbLoggerFcts.ColorLogFinishedBackground(displayedLog, singleLog.outcome);
    //    }
    //}


    private void InitEquippedCombatSpells()
    {
        usableSpellDisplays.Clear();
        foreach(Transform child in spellsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(CombatHandler.EquippedCombatSpell eqSpell in combatEncounter.character.combatHandler.combatSpells)
        {
            if (eqSpell.combatSpell != null)
            {
                GameObject gO = Instantiate(usableSpellContainerPrefab, spellsPanel.transform);
                UsableSpellDisplay usd = gO.GetComponent<UsableSpellDisplay>();
                usableSpellDisplays.Add(usd);
                usd.InitUsableSpellDisplay(eqSpell);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(spellsPanel.GetComponent<RectTransform>());
    }

    public void OnClickPassTurn()
    {
        combatEncounter.ManualChooseAction(null);
    }

    public void OnClickRun()
    {
        combatEncounter.ManualRunFromCombat();
    }

    private void UpdateUsableSpells()
    {
        foreach (UsableSpellDisplay usableSpell in usableSpellDisplays)
        {
            usableSpell.UpdateSpell();
        }
    }

    public void DisplayMessage()
    {
        //Implement message system

    }
}
