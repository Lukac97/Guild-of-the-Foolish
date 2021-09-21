using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoveringInfoDisplay : MonoBehaviour
{
    private static HoveringInfoDisplay _instance;
    public static HoveringInfoDisplay Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private List<RectTransform> hoveringInfoRectTransform;
    private List<ItemDescriptionController> itemDescController;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        itemDescController = new List<ItemDescriptionController>();
        foreach(RectTransform hoverDisp in hoveringInfoRectTransform)
        {
            itemDescController.Add(hoverDisp.GetComponentInChildren<ItemDescriptionController>());
        }
    }

    private void Start()
    {
        HideItemDetailsDisplay();
    }

    public void ShowItemDetailsDisplay(EquipmentArmorSlot eqSlot, bool toTheRight)
    {
        if (eqSlot.armorSlotItem == null)
        {
            HideItemDetailsDisplay();
            return;
        }
        ShowItemDetailsDisplay(eqSlot.GetComponent<RectTransform>(), eqSlot.armorSlotItem.item, toTheRight, false);
    }

    public void ShowItemDetailsDisplay(EquipmentWeaponSlot eqSlot, bool toTheRight)
    {
        if (eqSlot.weaponSlotItem == null)
        {
            HideItemDetailsDisplay();
            return;
        }
        ShowItemDetailsDisplay(eqSlot.GetComponent<RectTransform>(), eqSlot.weaponSlotItem.item, toTheRight, false);
    }

    public void ShowItemDetailsDisplay(InventoryListElement iListEl, bool toTheRight)
    {
        if (iListEl.itemObject == null)
        {
            HideItemDetailsDisplay();
            return;
        }
        ShowItemDetailsDisplay(iListEl.GetComponent<RectTransform>(), iListEl.itemObject.item, toTheRight, true);
    }

    private void ShowItemDetailsDisplay(RectTransform hoveredObject, Item item, bool toTheRight, bool fromInventory)
    {
        // Regular item details showing
        hoveringInfoRectTransform[0].sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.2f,
            GetComponent<RectTransform>().rect.height * 0.3f);

        Vector2 idealPosition = new Vector2(
                hoveredObject.localPosition.x + hoveredObject.rect.width / 2 * (toTheRight ? 1 : -1),
                hoveredObject.localPosition.y + hoveredObject.rect.height / 2);

        //Transform to world space
        idealPosition = hoveredObject.parent.TransformPoint(idealPosition);
        itemDescController[0].ShowItemDescription(item);

        //Set position after full creation
        hoveringInfoRectTransform[0].position = idealPosition;
        Vector2 newPosition = new Vector2(
                hoveringInfoRectTransform[0].localPosition.x + hoveringInfoRectTransform[0].rect.width / 2 * (toTheRight ? 1 : -1),
                hoveringInfoRectTransform[0].localPosition.y + hoveringInfoRectTransform[0].rect.height / 2);

        hoveringInfoRectTransform[0].localPosition = newPosition;

        //Additional descriptions

        CharEquipment charEquipment = GlobalInput.CheckIfSelectedCharacter() ? 
            GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>() : null;

        int hideCounter = 0;

        if (MenuHandler.Instance.equipmentMenu.isActive & fromInventory & charEquipment != null)
        {
            if (item.GetType() == typeof(ArmorItem))
            {
                List<CharEquipment.ArmorSlotItem> itemSlots = charEquipment.FindBestSlotToEquipItem((ArmorItem)item);
                for (int i = 0; i < Mathf.Min(itemSlots.Count, 2); i++)
                {
                    hoveringInfoRectTransform[i + 1].sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.2f,
                        GetComponent<RectTransform>().rect.height * 0.3f);
                    newPosition = new Vector2(
                        hoveringInfoRectTransform[0].localPosition.x +
                            (hoveringInfoRectTransform[0].rect.width / 2 + hoveringInfoRectTransform[i + 1].rect.width / 2) * (toTheRight ? 1 : -1),
                        hoveringInfoRectTransform[0].localPosition.y);
                    hoveringInfoRectTransform[i + 1].localPosition = newPosition;
                    itemDescController[i + 1].ShowItemDescription(itemSlots[i].item);
                }
                hideCounter = Mathf.Min(itemSlots.Count, 2);
            }
            else if (item.GetType() == typeof(WeaponItem))
            {
                List<CharEquipment.WeaponSlotItem> itemSlots = charEquipment.FindBestSlotToEquipItem((WeaponItem)item);
                for (int i = 0; i < Mathf.Min(itemSlots.Count, 2); i++)
                {
                    hoveringInfoRectTransform[i + 1].sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.2f,
                        GetComponent<RectTransform>().rect.height * 0.3f);
                    newPosition = new Vector2(
                        hoveringInfoRectTransform[0].localPosition.x +
                            (hoveringInfoRectTransform[0].sizeDelta.x / 2 + hoveringInfoRectTransform[i + 1].sizeDelta.x * (1 + 2 * i) / 2) * (toTheRight ? 1 : -1),
                        hoveringInfoRectTransform[0].localPosition.y);
                    hoveringInfoRectTransform[i + 1].localPosition = newPosition;
                    itemDescController[i + 1].ShowItemDescription(itemSlots[i].item);
                }
                hideCounter = Mathf.Min(itemSlots.Count, 2);
            }
        }

        for (int i = hideCounter; i < 2; i++)
        {
            itemDescController[i + 1].HideItemDescription();
        }
    }

    public void HideItemDetailsDisplay()
    {
        foreach (ItemDescriptionController itemDesc in itemDescController)
        {
            itemDesc.HideItemDescription();
        }
    }
}
