using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggingIconHandler : MonoBehaviour
{
    private static DraggingIconHandler _instance;
    public static DraggingIconHandler Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private CanvasGroup dragObjectCanvasGroup;
    [SerializeField] private ItemIconHandler iconBeingDragged;
    [SerializeField] private Image imageBeingDragged;
    public Canvas canvas;
    
    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    private void Start()
    {
        imageBeingDragged.raycastTarget = false;
        StopObjectDrag();
    }

    private void SetIconSize(RectTransform referenceEl)
    {
        dragObjectCanvasGroup.GetComponent<RectTransform>().sizeDelta = referenceEl.sizeDelta;
    }

    //Start dragging item InventoryListElement
    public void StartDraggingObject(InventoryListElement iLElement)
    {
        StartDraggingObject(iLElement.itemObject.item, iLElement.GetComponent<RectTransform>());
    }

    //Start dragging item EquipmentWeaponSlot
    public void StartDraggingObject(EquipmentWeaponSlot wSlot)
    {
        StartDraggingObject(wSlot.weaponSlotItem.item, wSlot.GetComponent<RectTransform>());
    }

    //Start dragging item EquipmentArmorSlot
    public void StartDraggingObject(EquipmentArmorSlot aSlot)
    {
        StartDraggingObject(aSlot.armorSlotItem.item, aSlot.GetComponent<RectTransform>());
    }

    //Main function for start dragging item
    private void StartDraggingObject(Item item, RectTransform rectTransform)
    {
        SetIconSize(rectTransform);
        imageBeingDragged.enabled = false;
        iconBeingDragged.IconSetActive(true);
        iconBeingDragged.InitItemIconHandler(item);

        dragObjectCanvasGroup.GetComponent<RectTransform>().position = rectTransform.position;
        GlobalFuncs.SetActiveCanvasGroup(dragObjectCanvasGroup, true);
    }

    public void StartDraggingObject(SingleSpellDisplay ssDisplay)
    {
        StartDraggingObject(ssDisplay.linkedSpell.spellIcon, ssDisplay.GetComponent<RectTransform>());
    }

    public void StartDraggingObject(SingleChosenSpellDisplay scsDisplay)
    {
        StartDraggingObject(scsDisplay.spellIcon.sprite, scsDisplay.GetComponent<RectTransform>());
    }

    private void StartDraggingObject(Sprite imageIcon, RectTransform rectTransform)
    {
        SetIconSize(rectTransform);
        iconBeingDragged.IconSetActive(false);
        imageBeingDragged.sprite = imageIcon;
        imageBeingDragged.enabled = true;

        dragObjectCanvasGroup.GetComponent<RectTransform>().position = rectTransform.position;
        GlobalFuncs.SetActiveCanvasGroup(dragObjectCanvasGroup, true);
    }

    public void UpdateObjectDrag(Vector3 newPos)
    {
        dragObjectCanvasGroup.GetComponent<RectTransform>().position = newPos;
    }

    public void StopObjectDrag()
    {
        iconBeingDragged.InitItemIconHandler(null);
        iconBeingDragged.IconSetActive(false);
        imageBeingDragged.enabled = false;
        GlobalFuncs.SetActiveCanvasGroup(dragObjectCanvasGroup, false);
    }
}
