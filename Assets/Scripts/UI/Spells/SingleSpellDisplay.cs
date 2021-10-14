using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SingleSpellDisplay : MonoBehaviour, IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public CanvasGroup canvasGroup;

    public TextMeshProUGUI level;
    public TextMeshProUGUI spellCost;
    public Image spellIcon;
    public CombatSpell linkedSpell;

    private SpellsDisplay spellsDisplay;
    private Vector2 localStartDragPos;

    private void Start()
    {
        spellsDisplay = GetComponentInParent<SpellsDisplay>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetIconTransparency(0.4f);
        localStartDragPos = GetComponent<RectTransform>().position;
        DraggingIconHandler.Instance.StartDraggingObject(this);
        DraggingIconHandler.Instance.UpdateObjectDrag(localStartDragPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        localStartDragPos += eventData.delta / DraggingIconHandler.Instance.canvas.scaleFactor;
        DraggingIconHandler.Instance.UpdateObjectDrag(transform.TransformPoint(localStartDragPos));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetIconTransparency(1f);
        DraggingIconHandler.Instance.StopObjectDrag();
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
    }

    public void OnSpellDoubleClick()
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
            return;
        GlobalInput.Instance.selectedEntity.GetComponent<CharCombat>().AddCombatSpell(linkedSpell);
    }

    public void ShowSpellSlot(bool doShow)
    {
        canvasGroup.alpha = doShow ? 1 : 0;
        canvasGroup.blocksRaycasts = doShow;
        canvasGroup.interactable = doShow;
    }

    private void SetIconTransparency(float tVal)
    {
        Color clr = spellIcon.color;
        spellIcon.color = new Color(clr.r, clr.g, clr.b, tVal);
    }
}
