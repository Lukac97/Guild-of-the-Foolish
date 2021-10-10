using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Canvas canvas;
    
    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    private void Start()
    {
        StopObjectDrag();
    }

    private void SetIconSize(RectTransform referenceEl)
    {
        dragObjectCanvasGroup.GetComponent<RectTransform>().sizeDelta = referenceEl.sizeDelta;
    }

    public void StartDraggingObject(InventoryListElement iLElement)
    {
        StartDraggingObject(iLElement.itemObject.item, iLElement.GetComponent<RectTransform>());
    }

    public void StartDraggingObject(EquipmentWeaponSlot wSlot)
    {
        StartDraggingObject(wSlot.weaponSlotItem.item, wSlot.GetComponent<RectTransform>());
    }

    public void StartDraggingObject(EquipmentArmorSlot aSlot)
    {
        StartDraggingObject(aSlot.armorSlotItem.item, aSlot.GetComponent<RectTransform>());
    }

    private void StartDraggingObject(Item item, RectTransform rectTransform)
    {
        iconBeingDragged.InitItemIconHandler(item);
        iconBeingDragged.IconSetActive(true);
        iconBeingDragged.GetComponent<RectTransform>().position = rectTransform.position;
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
        GlobalFuncs.SetActiveCanvasGroup(dragObjectCanvasGroup, false);
    }
}
