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
    [Header("Logger")]
    public CanvasGroup loggerCanvasGroup;
    public GameObject loggerPrefab;
    public GameObject loggerPanel;
    [Space(6)]
    [Header("Content settings")]
    public TextMeshProUGUI outcomeTitle;
    [Space(3)]
    public GameObject itemRewardPrefab;
    [SerializeField] private GameObject GEPanel;
    [SerializeField] private GameObject GEPanelScroll;
    [SerializeField] private GameObject ItemPanel;
    [SerializeField] private GameObject ItemPanelScroll;
    [SerializeField] private float spacingPercentage;
    [Space(3)]
    public GameObject goldExpPrizeSlot;
    public GameObject itemPrizeSlot;


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
        GlobalFuncs.PackGridLayoutWithScroll(GEPanelScroll, GEPanel, 1, spacingPercentage);
        GlobalFuncs.PackGridLayoutWithScroll(ItemPanelScroll, ItemPanel, 1, spacingPercentage);
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
        UIScreenController.Instance.ActivateMainScreen();
        currentEncounter.DestroyThisCombatEncounter();
    }

    public void ShowEndOfCombatScreen()
    {
        CloseCombatLog();
        if(currentEncounter.finalOutcome == 0)
            outcomeTitle.text = "Defeat...";
        else if(currentEncounter.finalOutcome == 1)
            outcomeTitle.text = "Victory!";
        else if(currentEncounter.finalOutcome == 2)
            outcomeTitle.text = "Tied.";
        else
            outcomeTitle.text = "--Error--";
        //Clear previous rewards
        foreach (Transform child in GEPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in ItemPanel.transform)
        {
            Destroy(child.gameObject);
        }

        //SetActive slots
        if(currentEncounter.combatReward.goldYield <= 0 & currentEncounter.combatReward.experienceYield <= 0)
        {
            goldExpPrizeSlot.SetActive(false);
        }
        else
        {
            goldExpPrizeSlot.SetActive(true);
        }

        if(currentEncounter.combatReward.items.Count == 0)
        {
            itemPrizeSlot.SetActive(false);
        }
        else
        {
            itemPrizeSlot.SetActive(true);
        }
        //Show current rewards
        foreach(CombatWinReward.ItemReward itemReward in currentEncounter.combatReward.items)
        {
            GameObject newItemReward = Instantiate(itemRewardPrefab, ItemPanel.transform);
            RewardItemSlot rewItemSlot = newItemReward.GetComponent<RewardItemSlot>();
            rewItemSlot.AssignItemReward(itemReward);
        }

        if(currentEncounter.combatReward.goldYield > 0)
        {
            GameObject newItemReward = Instantiate(itemRewardPrefab, GEPanel.transform);
            RewardItemSlot rewItemSlot = newItemReward.GetComponent<RewardItemSlot>();
            rewItemSlot.AssignGoldReward(currentEncounter.combatReward.goldYield);
        }

        if(currentEncounter.combatReward.experienceYield > 0)
        {
            GameObject newItemReward = Instantiate(itemRewardPrefab, GEPanel.transform);
            RewardItemSlot rewItemSlot = newItemReward.GetComponent<RewardItemSlot>();
            rewItemSlot.AssignGoldReward(currentEncounter.combatReward.experienceYield);
        }


    }

    //On click claim
    public void ClaimRewards()
    {
        GuildMain.Instance.AddGold(currentEncounter.combatReward.goldYield);
        currentEncounter.character.combatHandler.GetComponent<CharStats>()
            .AddExperience(currentEncounter.combatReward.experienceYield);
        foreach (CombatWinReward.ItemReward itemToAdd in currentEncounter.combatReward.items)
        {
            GuildInventory.Instance.AddItemToInventory(itemToAdd.item, itemToAdd.quantity);
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
                CbLoggerFcts.ColorLogBackground(displayedLog, true);
            else if (singleLog.charTurn)
                CbLoggerFcts.ColorLogBackground(displayedLog, false);
            else if (singleLog.turnNumber)
                CbLoggerFcts.ColorLogDefault(displayedLog);
            else if (singleLog.outcome > -1)
                CbLoggerFcts.ColorLogFinishedBackground(displayedLog, singleLog.outcome);
        }
    }
}
