using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SortingController : MonoBehaviour
{
    public List<SortingObject> sortingObjects;
    public List<FilteringObject> filteringObjects;
    public InventoryList inventoryList;

    [SerializeField] private GameObject sortingPanel;
    [SerializeField] private GameObject filteringPanel;

    [Header("Sorting colors")]
    public Color enabledOrderColor;
    public Color disabledOrderColor;

    [Header("Filtering colors")]
    public Color enabledFilterColor;
    public Color disabledFilterColor;

    private void Start()
    {
        inventoryList = GetComponentInParent<InventoryList>();
        sortingObjects = new List<SortingObject>();
        filteringObjects = new List<FilteringObject>();

        foreach(Transform child in sortingPanel.transform)
        {
            SortingObject sortObj = child.GetComponent<SortingObject>();
            sortObj.InitSortingObject();
            sortingObjects.Add(sortObj);
        }

        foreach(Transform child in filteringPanel.transform)
        {
            FilteringObject filteringObj = child.GetComponent<FilteringObject>();
            filteringObj.InitFilteringObject();
            filteringObjects.Add(filteringObj);
        }
    }

    public void TurnOffAllOrder()
    {
        foreach(SortingObject child in sortingObjects)
        {
            child.TurnOffOrder();
        }
    }

    public void UpdateFilterButtons()
    {
        foreach(FilteringObject child in filteringObjects)
        {
            child.EnableCorrectIndicators();
        }
    }

}
