using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellDescriptionController : MonoBehaviour
{
    [Header("Item info")]
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellLevel;
    public TextMeshProUGUI spellCost;
    public TextMeshProUGUI spellDescription;
    public Image icon;

    private CombatSpell currentSpell = null;

    private void Start()
    {
        UpdateSelectedSpell();
    }

    public void ShowSpellDescription(CombatSpell spell)
    {
        currentSpell = spell;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        UpdateSelectedSpell();
    }

    public void HideSpellDescription()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void UpdateSelectedSpell()
    {
        if (currentSpell == null)
        {
            HideSpellDescription();
            return;
        }
        spellName.text = currentSpell.name;
        spellLevel.text = currentSpell.spellLevel.ToString();
        spellCost.text = currentSpell.spellCost.ToString();
        spellDescription.text = currentSpell.spellDescription;
        icon.sprite = currentSpell.spellIcon;
    }
}
