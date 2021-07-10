using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SingleChosenSpellDisplay : MonoBehaviour, IPointerClickHandler
{

    public TextMeshProUGUI level;
    public TextMeshProUGUI spellCost;
    public Image spellIcon;

    private CharCombat chosenCharCombat;
    private int chosenSpellIdx;

    public void InitSingleSpellDisplay(CharCombat chosenCombat, int spellIdx)
    {
        if (chosenCombat == null)
            return;
        chosenCharCombat = chosenCombat;
        chosenSpellIdx = spellIdx;
        if (spellIdx >= chosenCombat.combatSpells.Count)
        {
            ShowSpellSlot(false);
            return;
        }
        else if (chosenCombat.combatSpells[spellIdx] == null | chosenCombat.combatSpells[spellIdx].combatSpell == null)
        {
            ShowSpellSlot(false);
            return;
        }
        ShowSpellSlot(true);
        level.text = chosenCharCombat.combatSpells[chosenSpellIdx].combatSpell.spellLevel.ToString();
        spellCost.text = chosenCharCombat.combatSpells[chosenSpellIdx].combatSpell.spellCost.ToString();
        spellIcon.sprite = chosenCharCombat.combatSpells[chosenSpellIdx].combatSpell.spellIcon;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 1)
        {
            OnSpellClick();
        }
        else if (eventData.clickCount == 2)
        {
            OnSpellDoubleClick();
        }
    }

    public void OnSpellClick()
    {
        SpellSlotPopUp.Instance.OpenSpellSlot(chosenCharCombat, chosenSpellIdx);
    }

    public void OnSpellDoubleClick()
    {
        //if (spellsDisplay.forChosen)
        //    spellsDisplay.selectedCharacterCombat.RemoveCombatSpell(linkedSpell);
        //else
        //    spellsDisplay.selectedCharacterCombat.AddCombatSpell(linkedSpell);
    }

    public void ShowSpellSlot(bool doShow)
    {
        level.enabled = doShow;
        spellCost.enabled = doShow;
        spellIcon.color = new Color(spellIcon.color.r, spellIcon.color.g, spellIcon.color.b, doShow ? 1 : 0);
    }
}
