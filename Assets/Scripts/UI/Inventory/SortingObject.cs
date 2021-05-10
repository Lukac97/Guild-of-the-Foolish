using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortingObject : MonoBehaviour
{
    public SortingController sortingController;
    public InventorySortBy sortBy;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI ascendingText;
    public TextMeshProUGUI descendingText;

    private void Awake()
    {
        buttonText.text = sortBy.ToString();
    }


    public void OnPressSort()
    {
        sortingController.TurnOffAllOrder();
        sortingController.inventoryList.ChangeSorting(sortBy);
        EnableCorrectIndicators();
    }

    public void InitSortingObject()
    {
        sortingController = GetComponentInParent<SortingController>();
        EnableCorrectIndicators();
    }

    public void EnableCorrectIndicators()
    {
        InventorySortOrder order = sortingController.inventoryList.sortOrder;
        InventorySortBy checkSortBy = sortingController.inventoryList.sortBy;
        TurnOffOrder();
        if (checkSortBy == sortBy)
        {
            if (order == InventorySortOrder.ASCENDING)
                ascendingText.color = sortingController.enabledOrderColor;
            if (order == InventorySortOrder.DESCENDING)
                descendingText.color = sortingController.enabledOrderColor;
        }
    }

    public void TurnOffOrder()
    {
        ascendingText.color = sortingController.disabledOrderColor;
        descendingText.color = sortingController.disabledOrderColor;
    }
}
