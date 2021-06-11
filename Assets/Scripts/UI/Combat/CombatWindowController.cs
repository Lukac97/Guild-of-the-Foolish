using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatWindowController : MonoBehaviour
{
    private static CombatWindowController _instance;
    public static CombatWindowController Instance
    {
        get
        {
            return _instance;
        }
    }

    [Header("Pop up settings")]
    public GameObject activatable;
    [Space(6)]
    [Header("Content settings")]
    public CanvasGroup loggerCanvasGroup;
    public GameObject loggerPrefab;
    public GameObject loggerPanel;

    [HideInInspector]
    public CombatEncounter currentEncounter;
    private PopUpController popUpPanel;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    private void Start()
    {
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void ActivateThisPopUp(CombatEncounter combatEncounter)
    {
        currentEncounter = combatEncounter;
        ShowEndOfCombatScreen();
        popUpPanel.ActivatePopUp(activatable);
    }
    public void ClosePopUp()
    {
        popUpPanel.DeactivatePopUp(activatable);
        currentEncounter.DestroyThisCombatEncounter();
    }

    public void ShowEndOfCombatScreen()
    {
        //This is gonna be on-click function instead of just being called immediately
        ShowCombatLog();
    }

    //On click claim
    public void ClaimRewards()
    {
        GuildMain.Instance.AddGold(currentEncounter.combatReward.goldYield);
        currentEncounter.character.combatHandler.GetComponent<CharStats>()
            .AddExperience(currentEncounter.combatReward.experienceYield);
        foreach (Item itemToAdd in currentEncounter.combatReward.items)
        {
            GuildInventory.Instance.AddItemToInventory(itemToAdd);
        }
        ClosePopUp();
    }

    public void ShowCombatLog()
    {
        ClearCombatLog();
        loggerCanvasGroup.alpha = 1;
        loggerCanvasGroup.blocksRaycasts = true;
        loggerCanvasGroup.interactable = true;
        CombatLoggerDisplay();
    }

    public void CloseCombatLog()
    {
        loggerCanvasGroup.alpha = 0;
        loggerCanvasGroup.blocksRaycasts = false;
        loggerCanvasGroup.interactable = false;
        ClearCombatLog();
        //TODO: FIX FOR NOW!
        ClaimRewards();
    }

    public void ClearCombatLog()
    {
        foreach (Transform child in loggerPanel.transform)
            Destroy(child.gameObject);
    }

    public void CombatLoggerDisplay()
    {
        foreach(CombatLogger.SingleLog singleLog in currentEncounter.combatLogger.listOfLogs)
        {
            GameObject displayedLog = Instantiate(loggerPrefab, loggerPanel.transform);
            displayedLog.GetComponentInChildren<TextMeshProUGUI>().text = singleLog.text;
            if (singleLog.enemyTurn)
                ColorLogBackground(displayedLog, true);
            else if (singleLog.charTurn)
                ColorLogBackground(displayedLog, false);
            else if (singleLog.turnNumber)
                ColorLogDefault(displayedLog);
            else if (singleLog.outcome > -1)
                ColorLogFinishedBackground(displayedLog, singleLog.outcome);
        }
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
}
