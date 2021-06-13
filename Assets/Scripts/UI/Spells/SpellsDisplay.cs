using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellsDisplay : MonoBehaviour
{
    public GameObject spellPanel;
    public bool forChosen = false;
    private int itemsPerPage;

    [Header("Pages")]
    public Button nextPage;
    public TextMeshProUGUI pageNumberMesh;
    public Button previousPage;

    [HideInInspector]
    public CharCombat selectedCharacterCombat;
    [HideInInspector]
    public CharStats selectedCharacterStats;

    private List<SingleSpellDisplay> singleSpellDisplays;

    private int pageNumber;
    private int maxPageNumber;
    private int totalSpellCount;
    private List<CombatSpell> allCombatSpells;
    private void Awake()
    {
        itemsPerPage = spellPanel.transform.childCount;
    }

    private void Start()
    {
        GlobalInput.Instance.onChangedSelectedEntity += UpdateSpellDisplay;
        CharactersController.ChangedChosenSpells += UpdateSpellDisplay;
        singleSpellDisplays = new List<SingleSpellDisplay>();
        foreach (Transform child in spellPanel.transform)
            singleSpellDisplays.Add(child.GetComponent<SingleSpellDisplay>());
        UpdateSpellDisplay();
    }

    public void UpdateSpellDisplay()
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
        {
            HideAllSpellSlots();
            return;
        }

        selectedCharacterCombat = GlobalInput.Instance.selectedEntity.GetComponent<CharCombat>();
        selectedCharacterStats = selectedCharacterCombat.GetComponent<CharStats>();
        allCombatSpells = new List<CombatSpell>();

        if (forChosen)
        {
            foreach(CombatHandler.EquippedCombatSpell eqCbSpell in selectedCharacterCombat.combatSpells)
                allCombatSpells.Add(eqCbSpell.combatSpell);
        }
        else
        {
            allCombatSpells.AddRange(selectedCharacterStats.characterClass.combatSpells);
            foreach(CombatHandler.EquippedCombatSpell eqCbSpell in selectedCharacterCombat.combatSpells)
                allCombatSpells.Remove(eqCbSpell.combatSpell);
        }

        totalSpellCount = allCombatSpells.Count;
        maxPageNumber = 1 + totalSpellCount / itemsPerPage;
        pageNumber = 1;
        UpdatePagePanel();
    }

    public void AssignSingleSpells()
    {
        int startCnt = (pageNumber - 1) * itemsPerPage;
        foreach (SingleSpellDisplay ssd in singleSpellDisplays)
        {
            if (startCnt < totalSpellCount)
            {
                ssd.InitSingleSpellDisplay(allCombatSpells[startCnt]);
            }
            else
            {
                ssd.InitSingleSpellDisplay(null);
            }
            startCnt += 1;
        }
    }

    public void UpdatePagePanel()
    {
        if (!forChosen)
        {
            pageNumberMesh.text = pageNumber.ToString();
            if (pageNumber == 1)
                previousPage.interactable = false;
            else
                previousPage.interactable = true;
            if (pageNumber == maxPageNumber)
                nextPage.interactable = false;
            else
                nextPage.interactable = true;
        }

        AssignSingleSpells();
    }

    public void NextPage()
    {
        pageNumber += 1;
        UpdatePagePanel();
    }
    public void PreviousPage()
    {
        pageNumber -= 1;
        UpdatePagePanel();
    }

    public void HideAllSpellSlots()
    {
        foreach(SingleSpellDisplay ssd in singleSpellDisplays)
        {
            ssd.ShowSpellSlot(false);
        }
    }
}