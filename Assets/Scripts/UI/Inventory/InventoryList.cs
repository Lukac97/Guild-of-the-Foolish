using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryList : MonoBehaviour
{
    [Header("Items")]
    public GameObject itemPanel;
    public List<ItemObject> itemObjects;

    [Header("Pages")]
    public Button nextPage;
    public TextMeshProUGUI pageNumberMesh;
    public Button previousPage;

    [Space(3)]
    [Header("Sorting")]
    public GameObject sortingPanel;
    public InventorySortBy sortBy = InventorySortBy.NAME;
    public InventorySortOrder sortOrder = InventorySortOrder.ASCENDING;

    private int pageNumber;
    private int maxPageNumber;
    private int itemsPerPage;

    private void Awake()
    {
        itemsPerPage = itemPanel.transform.childCount;
    }

    private void Start()
    {
        GuildInventory.Instance.InventoryChanged += UpdateInventoryItems;
        UpdateInventoryItems();
    }

    public void UpdateInventoryItems()
    {
        itemObjects = GuildInventory.Instance.GetAllItemObjects();
        //TODO: Filtering functions
        InsertionSort();
        maxPageNumber = 1 + itemObjects.Count / itemsPerPage;
        pageNumber = 1;
        UpdatePagePanel();
    }

    public void UpdatePagePanel()
    {
        pageNumberMesh.text = pageNumber.ToString();
        if (pageNumber == 1)
            previousPage.interactable = false;
        else
            previousPage.interactable = true;
        if (pageNumber == maxPageNumber)
            nextPage.interactable = false;
        else
            nextPage.interactable = true;

        AssignItemObjects();
    }

    private void AssignItemObjects()
    {
        int startCnt = (pageNumber-1) * itemsPerPage;
        foreach (Transform child in itemPanel.transform)
        {
            InventoryListElement ilElement = child.GetComponent<InventoryListElement>();
            if (startCnt < itemObjects.Count)
            {
                ilElement.AssignItemObject(itemObjects[startCnt]);
            }
            else
            {
                ilElement.AssignItemObject(null);
            }
            startCnt += 1;
        }
    }

    public void NextPage()
    {
        pageNumber += 1;
        UpdatePagePanel();
    }
    public void PreviousPage()
    {
        pageNumber -= 1;
        UpdatePagePanel();
    }

    public void ChangeSorting(InventorySortBy newSortBy)
    {
        if (sortBy == newSortBy)
        {
            if(sortOrder == InventorySortOrder.ASCENDING)
                sortOrder = InventorySortOrder.DESCENDING;
            else
                sortOrder = InventorySortOrder.ASCENDING;
        }
        else
        {
            sortOrder = InventorySortOrder.ASCENDING;
        }
        sortBy = newSortBy;
        UpdateInventoryItems();
    }

    public void InsertionSort()
    {
        int n = itemObjects.Count;
        for (int i = 1; i < n; ++i)
        {
            ItemObject key = itemObjects[i];
            int j = i - 1;

            // Move elements of arr[0..i-1],
            // that are greater than key,
            // to one position ahead of
            // their current position
            while (j >= 0 && CheckIfShouldBeInFront(itemObjects[j], key))
            {
                itemObjects[j + 1] = itemObjects[j];
                j = j - 1;
            }
            itemObjects[j + 1] = key;
        }
    }

    public bool CheckIfShouldBeInFront (ItemObject obj1, ItemObject obj2)
    {
        bool returnValue = false;
        if (sortBy == InventorySortBy.LEVEL)
        {
            returnValue = obj1.item.level > obj2.item.level;
        }
        if (sortBy == InventorySortBy.NAME)
        {
            returnValue = String.Compare(obj1.item.name, obj2.item.name, comparisonType: StringComparison.OrdinalIgnoreCase) > 0 ? true : false;
        }
        if (sortBy == InventorySortBy.QUANTITY)
        {
            returnValue = obj1.quantity > obj2.quantity;
        }
        if (sortOrder == InventorySortOrder.DESCENDING)
            returnValue = !returnValue;
        return returnValue;
    }

}
