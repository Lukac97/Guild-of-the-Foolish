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
            linkedSpell = null;
            level.text = "";
            spellCost.text = "";
            spellIcon.enabled = false;
            spellIcon.sprite = null;
            cooldownContainer.SetActive(false);
        }
        else
        {
            linkedSpell = equippedCombatSpell;
            level.text = equippedCombatSpell.combatSpell.spellLevel.ToString();
            spellCost.text = equippedCombatSpell.combatSpell.spellCost.ToString();
            spellIcon.enabled = true;
            spellIcon.sprite = equippedCombatSpell.combatSpell.spellIcon;
        }

        UpdateSpell();
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClickUsableSpell();
    }

    private void OnClickUsableSpell()
    {
        if (linkedSpell == null)
            return;
        if(linkedSpell.cooldownLeft <= 0)
        {
            int outcome = manualCombatHandler.combatEncounter.ManualChooseAction(linkedSpell);
            if(outcome == 1)
                manualCombatHandler.mCombatAnimationHandler.NotEnoughResourceAlert();
        }
        else
        {
            manualCombatHandler.mCombatAnimationHandler.OnCooldownAlert();
        }
    }

    public void UpdateSpell()
    {
        if (linkedSpell == null)
            return;
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
