using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UsableSpellDisplay : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI spellCost;
    public Image spellIcon;

    [Space(6)]
    public GameObject cooldownContainer;
    public Image cooldownMask;
    public TextMeshProUGUI cooldownLeft;

    [Space(6)]
    public CombatHandler.EquippedCombatSpell linkedSpell;
    private ManualCombatUIHandler manualCombatHandler;
    public void InitUsableSpellDisplay(CombatHandler.EquippedCombatSpell equippedCombatSpell)
    {
        manualCombatHandler = GetComponentInParent<ManualCombatUIHandler>();
        if (equippedCombatSpell == null)
        {
            Destroy(gameObject);
            return;
        }
        linkedSpell = equippedCombatSpell;
        level.text = equippedCombatSpell.combatSpell.spellLevel.ToString();
        spellCost.text = equippedCombatSpell.combatSpell.spellCost.ToString();
        spellIcon.sprite = equippedCombatSpell.combatSpell.spellIcon;

        UpdateSpell();
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClickUsableSpell();
    }

    private void OnClickUsableSpell()
    {
        if(linkedSpell.cooldownLeft <= 0)
        {
            manualCombatHandler.combatEncounter.ManualChooseAction(linkedSpell);
        }
    }

    public void UpdateSpell()
    {
        cooldownLeft.text = linkedSpell.cooldownLeft.ToString();
        if (linkedSpell.cooldownLeft <= 0)
        {
            cooldownContainer.SetActive(false);
        }
        else
        {
            cooldownContainer.SetActive(true);
        }
    }
}
