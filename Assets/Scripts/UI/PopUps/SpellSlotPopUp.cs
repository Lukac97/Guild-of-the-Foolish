using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpellSlotPopUp : MonoBehaviour
{
    private static SpellSlotPopUp _instance;
    public static SpellSlotPopUp Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject activatable;
    [SerializeField]
    private SpellsDisplay spellsDisplay;

    [Space(5)]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI spellDescription;
    public TextMeshProUGUI spellName;
    public Image spellIcon;
    
    [Header("To choose spell")]
    [Space(5)]
    public TextMeshProUGUI toChooseSpellDescription;
    public TextMeshProUGUI toChooseSpellName;
    public Image toChooseSpellIcon;

    [Header("Canvas groups")]
    [Space(6)]
    [SerializeField]
    private CanvasGroup mainSlotPanel;
    [SerializeField]
    private CanvasGroup changeEquippedSlotPanel;

    [Space(10)]
    [SerializeField]
    private CanvasGroup spellDetailsPanel;
    [SerializeField]
    private CanvasGroup spellDetailsButtonsPanel;

    [SerializeField]
    private CanvasGroup availableSpellsPanel;
    [SerializeField]
    private CanvasGroup availableSpellsButtonsPanel;


    private CharCombat currentCharCombat;
    private int spellSlotNumber;
    private CombatSpell currentCombatSpell;
    private CombatSpell toChooseCombatSpell;

    private PopUpController popUpPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void OpenSpellSlot(CharCombat selectedChar, int slotNumber)
    {
        if (selectedChar == null)
            return;
        if (slotNumber < 0)
            return;
        currentCharCombat = selectedChar;
        spellSlotNumber = slotNumber;
        popUpPanel.ActivatePopUp(activatable);
        InitContent();
    }

    public void OnClickChange()
    {
        InitAvailableSpellsList();
        ActivateChangeChosenPanel();
    }

    public void OnClickOnAvailableSpell(CombatSpell cmbSpell)
    {
        toChooseCombatSpell = cmbSpell;
        InitToEquipSpellDetails();
        ActivateSpellDetailsPanel();
    }

    public void OnClickCancelChange()
    {
        ActivateMainSlotPanel();
    }

    public void OnClickEquipYes()
    {
        spellsDisplay.selectedCharacterCombat.AddCombatSpellToSlot(toChooseCombatSpell, spellSlotNumber);
        InitContent();
    }

    public void OnClickEquipNo()
    {
        InitAvailableSpellsList();
    }

    private void InitToEquipSpellDetails()
    {
        DisplaySpellsDetails(toChooseCombatSpell, toChooseSpellIcon, toChooseSpellDescription, toChooseSpellName);
    }

    private void InitAvailableSpellsList()
    {
        spellsDisplay.UpdateSpellDisplay();
        ActivateAvailableSpellsPanel();
    }

    private void InitContent()
    {
        currentCombatSpell = spellSlotNumber < currentCharCombat.combatSpells.Count ?
            currentCharCombat.combatSpells[spellSlotNumber].combatSpell : null;
        titleText.text = "Spell slot " + (spellSlotNumber + 1).ToString();
        DisplaySpellsDetails(currentCombatSpell, spellIcon, spellDescription, spellName);
        ActivateMainSlotPanel();
    }

    private void DisplaySpellsDetails(CombatSpell spell, Image iconToSet,
        TextMeshProUGUI descToSet, TextMeshProUGUI nameToSet)
    {
        if(spell == null)
        {
            if (iconToSet != null)
                iconToSet.enabled = false;
            if (descToSet != null)
                descToSet.text = "";
            if (nameToSet != null)
                nameToSet.text = "";
            return;
        }

        if (iconToSet != null)
            iconToSet.enabled = true;
        if (descToSet != null)
            descToSet.text = spell.spellDescription;
        if (nameToSet != null)
            nameToSet.text = spell.name;
    }

    public void CloseSpellSlot()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }

    private void ActivateMainSlotPanel()
    {
        ActivateCanvasGroup(mainSlotPanel, true);
        ActivateCanvasGroup(changeEquippedSlotPanel, false);
    }

    private void ActivateChangeChosenPanel()
    {
        ActivateCanvasGroup(changeEquippedSlotPanel, true);
        ActivateCanvasGroup(mainSlotPanel, false);

        ActivateAvailableSpellsPanel();
    }

    private void ActivateAvailableSpellsPanel()
    {
        ActivateCanvasGroup(availableSpellsPanel, true);
        ActivateCanvasGroup(availableSpellsButtonsPanel, true);
        ActivateCanvasGroup(spellDetailsPanel, false);
        ActivateCanvasGroup(spellDetailsButtonsPanel, false);
    }

    private void ActivateSpellDetailsPanel()
    {
        ActivateCanvasGroup(spellDetailsPanel, true);
        ActivateCanvasGroup(spellDetailsButtonsPanel, true);
        ActivateCanvasGroup(availableSpellsPanel, false);
        ActivateCanvasGroup(availableSpellsButtonsPanel, false);
    }

    public void ActivateCanvasGroup(CanvasGroup cg, bool doActivate)
    {
        cg.alpha = doActivate ? 1 : 0;
        cg.interactable = doActivate;
        cg.blocksRaycasts = doActivate;
    }
}
