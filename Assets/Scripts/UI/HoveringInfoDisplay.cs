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

    public void ShowItemDetailsDisplay(InventoryListElement iListEl, bool toTheRight)
    {
        if (iListEl.itemObject == null)
            return;

        // Regular item details showing
        hoveringInfoRectTransform[0].sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.2f,
            GetComponent<RectTransform>().rect.height * 0.3f);

        RectTransform iListElRectTransform = iListEl.GetComponent<RectTransform>();
        Vector2 idealPosition = new Vector2(
                iListElRectTransform.localPosition.x + iListElRectTransform.rect.width / 2 * (toTheRight ? 1 : -1),
                iListElRectTransform.localPosition.y + iListElRectTransform.rect.height / 2);

        //Transform to world space
        idealPosition = iListElRectTransform.parent.TransformPoint(idealPosition);
        itemDescController[0].ShowItemDescription(iListEl.itemObject.item);

        //Set position after full creation
        hoveringInfoRectTransform[0].position = idealPosition;
        Vector2 newPosition = new Vector2(
                hoveringInfoRectTransform[0].localPosition.x + hoveringInfoRectTransform[0].rect.width / 2 * (toTheRight ? 1 : -1),
                hoveringInfoRectTransform[0].localPosition.y + hoveringInfoRectTransform[0].rect.height / 2);

        hoveringInfoRectTransform[0].localPosition = newPosition;

        //Check if equipment comparison details should be displayed
        if (!MenuHandler.Instance.equipmentMenu.isActive)
            return;

        if (!GlobalInput.CheckIfSelectedCharacter())
            return;

        CharEquipment charEquipment = GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>();

        if (iListEl.itemObject.item.GetType() == typeof(ArmorItem))
        {
            List<CharEquipment.ArmorSlotItem> itemSlots = charEquipment.FindBestSlotToEquipItem((ArmorItem)iListEl.itemObject.item);
            for (int i = 0; i < Mathf.Min(itemSlots.Count, 2); i++)
            {
                hoveringInfoRectTransform[i+1].sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.2f,
                    GetComponent<RectTransform>().rect.height * 0.3f);
                newPosition = new Vector2(
                    hoveringInfoRectTransform[0].localPosition.x +
                        (hoveringInfoRectTransform[0].rect.width / 2 + hoveringInfoRectTransform[i+1].rect.width / 2) * (toTheRight ? 1 : -1),
                    hoveringInfoRectTransform[0].localPosition.y);
                hoveringInfoRectTransform[i+1].localPosition = newPosition;
                itemDescController[i + 1].ShowItemDescription(itemSlots[i].item);
            }
            for (int i = Mathf.Min(itemSlots.Count, 2); i < 2; i++)
            {
                itemDescController[i + 1].HideItemDescription();
            }
        }
        else if (iListEl.itemObject.item.GetType() == typeof(WeaponItem))
        {
            List<CharEquipment.WeaponSlotItem> itemSlots = charEquipment.FindBestSlotToEquipItem((WeaponItem)iListEl.itemObject.item);
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
            for (int i = Mathf.Min(itemSlots.Count, 2); i < 2; i++)
            {
                itemDescController[i + 1].HideItemDescription();
            }
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
