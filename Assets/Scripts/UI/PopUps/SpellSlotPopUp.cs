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
    [SerializeField] private GameObject deselectButton;
    [SerializeField] private TextMeshProUGUI equipButtonText;


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

    public void OnClickOnAvailableSpell(CombatSpell cmbSpell)
    {
        toChooseCombatSpell = cmbSpell;
        InitToEquipSpellDetails();
    }

    public void OnClickDeselectSpellToEquip()
    {
        toChooseCombatSpell = null;
        InitToEquipSpellDetails();
    }

    public void OnClickEquipYes()
    {
        CombatSpell temp = currentCombatSpell;
        if (toChooseCombatSpell == null)
        {
            spellsDisplay.selectedCharacterCombat.RemoveCombatSpellFromSlot(spellSlotNumber);
        }
        else
        {
            spellsDisplay.selectedCharacterCombat.AddCombatSpellToSlot(toChooseCombatSpell, spellSlotNumber);
        }
        toChooseCombatSpell = currentCombatSpell;
        InitContent();
    }

    private void InitToEquipSpellDetails()
    {
        if (toChooseCombatSpell == null)
        {
            deselectButton.SetActive(false);
            equipButtonText.text = "UNEQUIP";
        }
        else
        {
            deselectButton.SetActive(true);
            equipButtonText.text = "EQUIP";
        }
        DisplaySpellsDetails(toChooseCombatSpell, toChooseSpellIcon, toChooseSpellDescription, toChooseSpellName);
    }

    private void InitContent()
    {
        currentCombatSpell = spellSlotNumber < currentCharCombat.combatSpells.Count ?
            currentCharCombat.combatSpells[spellSlotNumber].combatSpell : null;
        titleText.text = "Spell slot " + (spellSlotNumber + 1).ToString();
        DisplaySpellsDetails(currentCombatSpell, spellIcon, spellDescription, spellName);
        InitToEquipSpellDetails();
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
        toChooseCombatSpell = null;
        popUpPanel.DeactivatePopUp(activatable);
    }

}
