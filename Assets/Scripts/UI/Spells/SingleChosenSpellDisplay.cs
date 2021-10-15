using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SingleChosenSpellDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public TextMeshProUGUI level;
    public TextMeshProUGUI spellCost;
    public Image spellIcon;

    private CharCombat chosenCharCombat;
    private int chosenSpellIdx;
    private Vector2 localStartDragPos;

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

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.ShowSpellDetailsDisplay(this, true);
    }

    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        HoveringInfoDisplay.Instance.HideSpellDetailsDisplay();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!SpellSlotOccupied())
            return;
        SetIconTransparency(0.4f);
        localStartDragPos = GetComponent<RectTransform>().position;
        DraggingIconHandler.Instance.StartDraggingObject(this);
        DraggingIconHandler.Instance.UpdateObjectDrag(localStartDragPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!SpellSlotOccupied())
            return;
        localStartDragPos += eventData.delta / DraggingIconHandler.Instance.canvas.scaleFactor;
        DraggingIconHandler.Instance.UpdateObjectDrag(transform.TransformPoint(localStartDragPos));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (SpellSlotOccupied())
            SetIconTransparency(1f);
        DraggingIconHandler.Instance.StopObjectDrag();
    }

    private bool SpellSlotOccupied()
    {
        return GlobalInput.CheckIfSelectedCharacter() ?
            GlobalInput.Instance.selectedEntity.GetComponent<CharCombat>().combatSpells[chosenSpellIdx].combatSpell != null : false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
            return;
        if (eventData.pointerDrag != null)
        {
            SingleSpellDisplay ssDisplay = eventData.pointerDrag.GetComponent<SingleSpellDisplay>();
            if (ssDisplay == null)
            {
                SingleChosenSpellDisplay sChosenDisplay = eventData.pointerDrag.GetComponent<SingleChosenSpellDisplay>();
                if (sChosenDisplay != null)
                {
                    if (!sChosenDisplay.SpellSlotOccupied())
                        return;
                    GlobalInput.Instance.selectedEntity.GetComponent<CharCombat>().SwapCombatSpellSlots(
                        sChosenDisplay.chosenSpellIdx, chosenSpellIdx);
                }
            }
            else if (ssDisplay.linkedSpell != null)
            {
                GlobalInput.Instance.selectedEntity.GetComponent<CharCombat>().AddCombatSpellToSlot(ssDisplay.linkedSpell, chosenSpellIdx);
            }
        }
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
        //SpellSlotPopUp.Instance.OpenSpellSlot(chosenCharCombat, chosenSpellIdx);
    }

    public void OnSpellDoubleClick()
    {
        chosenCharCombat.RemoveCombatSpellFromSlot(chosenSpellIdx);
    }

    public void ShowSpellSlot(bool doShow)
    {
        level.enabled = doShow;
        spellCost.enabled = doShow;
        spellIcon.color = new Color(spellIcon.color.r, spellIcon.color.g, spellIcon.color.b, doShow ? 1 : 0);
    }

    private void SetIconTransparency(float tVal)
    {
        Color clr = spellIcon.color;
        spellIcon.color = new Color(clr.r, clr.g, clr.b, tVal);
    }

    public CombatSpell GetCombatSpell()
    {
        return chosenCharCombat != null ? chosenCharCombat.combatSpells[chosenSpellIdx].combatSpell : null;
    }
}
