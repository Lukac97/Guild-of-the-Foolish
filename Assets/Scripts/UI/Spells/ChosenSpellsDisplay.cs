using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenSpellsDisplay : MonoBehaviour
{
    public GameObject spellPanel;

    [HideInInspector]
    public CharCombat selectedCharacterCombat;
    [HideInInspector]
    public CharStats selectedCharacterStats;

    private List<SingleChosenSpellDisplay> singleChosenSpellDisplays;

    private void Start()
    {
        CharTabMain.CharTabChangedChar += UpdateSpellDisplay;
        CharactersController.ChangedChosenSpells += UpdateSpellDisplay;
        singleChosenSpellDisplays = new List<SingleChosenSpellDisplay>();
        foreach (Transform child in spellPanel.transform)
            singleChosenSpellDisplays.Add(child.GetComponent<SingleChosenSpellDisplay>());
        UpdateSpellDisplay();
        GlobalFuncs.PackGridLayoutSquare(spellPanel, spellPanel.transform.childCount);
    }

    public void UpdateSpellDisplay()
    {
        if (CharTabMain.Instance.currentChar == null)
        {
            HideAllSpellSlots();
            return;
        }

        selectedCharacterCombat = CharTabMain.Instance.currentChar.GetComponent<CharCombat>();
        selectedCharacterStats = selectedCharacterCombat.GetComponent<CharStats>();
        AssignSingleSpells();
    }

    public void HideAllSpellSlots()
    {
        foreach (SingleChosenSpellDisplay ssd in singleChosenSpellDisplays)
        {
            ssd.ShowSpellSlot(false);
        }
    }

    public void AssignSingleSpells()
    {
        for(int i = 0; i < GlobalRules.maxCombatSpells; i++)
        {
            singleChosenSpellDisplays[i].InitSingleSpellDisplay(selectedCharacterCombat, i);
        }
        for(int i = GlobalRules.maxCombatSpells; i < singleChosenSpellDisplays.Count; i++)
        {
            singleChosenSpellDisplays[i].ShowSpellSlot(false);
        }
    }
}
