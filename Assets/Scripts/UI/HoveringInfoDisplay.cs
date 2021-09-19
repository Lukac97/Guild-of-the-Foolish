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

    [SerializeField] private RectTransform hoveringInfoRectTransform;
    private ItemDescriptionController itemDescController;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        itemDescController = hoveringInfoRectTransform.GetComponentInChildren<ItemDescriptionController>();
    }

    private void Start()
    {
        HideItemDetailsDisplay();
    }

    public void ShowItemDetailsDisplay(InventoryListElement iListEl, bool toTheRight)
    {
        if (iListEl.itemObject == null)
            return;
        hoveringInfoRectTransform.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.2f,
            GetComponent<RectTransform>().rect.height * 0.3f);
        RectTransform iListElRectTransform = iListEl.GetComponent<RectTransform>();
        Vector2 idealPosition = new Vector2(
                iListElRectTransform.localPosition.x + iListElRectTransform.sizeDelta.x / 2 * (toTheRight ? 1 : -1),
                iListElRectTransform.localPosition.y + iListElRectTransform.sizeDelta.y / 2);

        //Transform to world space
        idealPosition = iListElRectTransform.parent.TransformPoint(idealPosition);
        hoveringInfoRectTransform.position = idealPosition;
        Vector2 newPosition = new Vector2(
                hoveringInfoRectTransform.localPosition.x + hoveringInfoRectTransform.sizeDelta.x / 2 * (toTheRight ? 1 : -1),
                hoveringInfoRectTransform.localPosition.y + hoveringInfoRectTransform.sizeDelta.y / 2);
        hoveringInfoRectTransform.localPosition = newPosition;
        itemDescController.ShowItemDescription(iListEl.itemObject);
    }

    public void HideItemDetailsDisplay()
    {
        itemDescController.HideItemDescription();
    }
}
