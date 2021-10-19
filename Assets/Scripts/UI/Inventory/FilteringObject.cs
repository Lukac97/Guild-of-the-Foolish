using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilteringObject : MonoBehaviour
{
    public InventoryFilterBy filterBy;
    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    public SortingController sortingController;

    private void Awake()
    {
        buttonText.text = filterBy.ToString();
    }

    public void OnPressFilter()
    {
        sortingController.inventoryList.ChangeFilters(filterBy);
        sortingController.UpdateFilterButtons();
    }
    public void InitFilteringObject()
    {
        sortingController = GetComponentInParent<SortingController>();
        EnableCorrectIndicators();
    }

    public void EnableCorrectIndicators()
    {
        if (sortingController.inventoryList.filterBy == filterBy)
        {
            buttonImage.color = sortingController.enabledFilterColor;
        }
        else
        {
            buttonImage.color = sortingController.disabledFilterColor;
        }
    }
}
