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
        List<ItemObject> newItemObjects = GuildInventory.Instance.GetAllItemObjects();
        //TODO: Sorting functions depending on chosen attribute to sort by
        itemObjects = newItemObjects;
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
}
