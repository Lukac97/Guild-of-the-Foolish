using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

//UI class for displaying spells
public class SpellsDisplay : MonoBehaviour
{
    public GameObject spellPanel;
    private int itemsPerPage;

    [Header("Pages")]
    public Button nextPage;
    public TextMeshProUGUI pageNumberMesh;
    public Button previousPage;

    [Header("On spell click events")]
    [Space(3)]
    public UnityEvent<CombatSpell> SingleClickEvent;
    public UnityEvent<CombatSpell> DoubleClickEvent;

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
        CharTabMain.CharTabChangedChar += UpdateSpellDisplay;
        CharactersController.ChangedChosenSpells += UpdateSpellDisplay;
        singleSpellDisplays = new List<SingleSpellDisplay>();
        foreach (Transform child in spellPanel.transform)
            singleSpellDisplays.Add(child.GetComponent<SingleSpellDisplay>());
        UpdateSpellDisplay();
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
        allCombatSpells = new List<CombatSpell>();

        allCombatSpells.AddRange(selectedCharacterStats.characterClass.combatSpells);
        foreach(CombatHandler.EquippedCombatSpell eqCbSpell in selectedCharacterCombat.combatSpells)
            allCombatSpells.Remove(eqCbSpell.combatSpell);

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
                ssd.AssignEvents(SingleClickEvent, DoubleClickEvent);
            }
            else
            {
                ssd.InitSingleSpellDisplay(null);
                ssd.AssignEvents(SingleClickEvent, DoubleClickEvent);
            }
            startCnt += 1;
        }
    }

    public void UpdatePagePanel()
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
