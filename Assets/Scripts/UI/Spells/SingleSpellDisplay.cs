using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SingleSpellDisplay : MonoBehaviour, IPointerClickHandler
{
    public CanvasGroup canvasGroup;

    public TextMeshProUGUI level;
    public TextMeshProUGUI spellCost;
    public Image spellIcon;
    public CombatSpell linkedSpell;

    [Space(6)]
    public UnityEvent<CombatSpell> SingleClickEvent;
    public UnityEvent<CombatSpell> DoubleClickEvent;

    private SpellsDisplay spellsDisplay;

    private void Start()
    {
        spellsDisplay = GetComponentInParent<SpellsDisplay>();
    }

    public void InitSingleSpellDisplay(CombatSpell combatSpell)
    {
        if (combatSpell == null)
        {
            ShowSpellSlot(false);
            return;
        }
        ShowSpellSlot(true);
        linkedSpell = combatSpell;
        level.text = combatSpell.spellLevel.ToString();
        spellCost.text = combatSpell.spellCost.ToString();
        spellIcon.sprite = combatSpell.spellIcon;
    }

    public void AssignEvents(UnityEvent<CombatSpell> _single, UnityEvent<CombatSpell> _double)
    {
        SingleClickEvent = _single;
        DoubleClickEvent = _double;
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
        if (SingleClickEvent != null)
        {
            SingleClickEvent.Invoke((CombatSpell)linkedSpell);
        }
    }

    public void OnSpellDoubleClick()
    {
        if (DoubleClickEvent != null)
        {
            DoubleClickEvent.Invoke((CombatSpell)linkedSpell);
        }
    }

    public void ShowSpellSlot(bool doShow)
    {
        canvasGroup.alpha = doShow ? 1 : 0;
        canvasGroup.blocksRaycasts = doShow;
        canvasGroup.interactable = doShow;
    }
}
