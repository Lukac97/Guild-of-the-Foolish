using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class InventoryList : MonoBehaviour
{
    public Color highlightColor;
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

    [Space(3)]
    public UnityEvent<ItemObject> SingleClickEvent;
    public UnityEvent<ItemObject> DoubleClickEvent;

    private int pageNumber;
    private int maxPageNumber;
    private int itemsPerPage;
    private List<InventoryListElement> listElements;

    private CharEquipment equippableForChar;
    private ArmorSlot? filterByArmorSlot;
    private WeaponSlot? filterByWeaponSlot;

    private bool markedForUpdate;

    private void Awake()
    {
        listElements = new List<InventoryListElement>(itemPanel.GetComponentsInChildren<InventoryListElement>(true));
        itemsPerPage = listElements.Count;
        markedForUpdate = true;
    }

    private void Start()
    {
        GuildInventory.Instance.InventoryChanged += UpdateInventoryItems;
        GlobalInput.Instance.changeSelectedItemObject += HighlightInventorySlot;
        if (markedForUpdate)
        {
            UpdateInventoryItems();
            markedForUpdate = false;
        }
        GlobalFuncs.PackGridLayoutSquare(itemPanel, itemsPerPage);
    }

    private void Update()
    {
        if(markedForUpdate)
        {
            UpdateInventoryItems();
            markedForUpdate = false;
        }
    }

    public void HighlightInventorySlot()
    {
        ItemObject iO = GlobalInput.Instance.selectedItemObject;
        if (iO == null)
            return;
        foreach(InventoryListElement elem in listElements)
        {
            if(elem.itemObject == iO)
            {
                elem.SetHighlight(true, highlightColor);
            }
            else
            {
                elem.SetHighlight(false);
            }
        }
    }

    public void UpdateInventoryItems()
    {
        itemObjects = GuildInventory.Instance.GetAllItemObjects();
        ApplyFilters();
        InsertionSort();
        maxPageNumber = 1 + (itemObjects.Count > 0 ? itemObjects.Count - 1 : 0) / itemsPerPage;
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
        foreach (InventoryListElement ilElement in listElements)
        {
            if (startCnt < itemObjects.Count)
            {
                ilElement.AssignItemObject(itemObjects[startCnt]);
                ilElement.AssignEvents(SingleClickEvent, DoubleClickEvent);
            }
            else
            {
                ilElement.AssignItemObject(null);
                ilElement.AssignEvents(SingleClickEvent, DoubleClickEvent);
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
        markedForUpdate = true;
    }

    #region Filter Functions
    public void EnableFilterByEquippableForChar(CharEquipment charForFilter)
    {
        equippableForChar = charForFilter;
        markedForUpdate = true;
    }

    public void DisableFilterByEquippableForChar()
    {
        equippableForChar = null;
        markedForUpdate = true;
    }

    public void EnableFilterByItemSlot(WeaponSlot weaponSlot)
    {
        filterByWeaponSlot = weaponSlot;
        filterByArmorSlot = null;
        markedForUpdate = true;
    }

    public void EnableFilterByItemSlot(ArmorSlot armorSlot)
    {
        filterByWeaponSlot = null;
        filterByArmorSlot = armorSlot;
        markedForUpdate = true;
    }

    public void DisableFilterByItemSlot()
    {
        filterByWeaponSlot = null;
        filterByArmorSlot = null;
        markedForUpdate = true;
    }

    public void EnableFilterByEquipmentSlotPreset(CharEquipment charForFilter, ArmorSlot? armorSlot, WeaponSlot? weaponSlot)
    {
        equippableForChar = charForFilter;
        filterByWeaponSlot = weaponSlot;
        filterByArmorSlot = armorSlot;
        markedForUpdate = true;
    }
    #endregion Filter Functions

    private void ApplyFilters()
    {
        List<ItemObject> newItemList = new List<ItemObject>(itemObjects);
        foreach (ItemObject itemObj in itemObjects)
        {
            if(equippableForChar != null)
            { 
                if (!equippableForChar.CanEquipItem(itemObj.item))
                {
                    newItemList.Remove(itemObj);
                }
            }
            if(filterByArmorSlot != null)
            {
                if (!(itemObj.item is ArmorItem))
                {
                    newItemList.Remove(itemObj);
                }
                else
                {
                    ArmorSlot aSlot = (filterByArmorSlot ?? default(ArmorSlot));
                    if (((ArmorItem)itemObj.item).itemSlot != aSlot)
                    {
                        newItemList.Remove(itemObj);
                    }
                }
            }
            if(filterByWeaponSlot != null)
            {
                if (!(itemObj.item is WeaponItem))
                {
                    newItemList.Remove(itemObj);
                }
                else
                {
                    WeaponSlot wSlot = (filterByWeaponSlot ?? default(WeaponSlot));
                    if ((wSlot == WeaponSlot.MAIN_HAND &
                        ((WeaponItem)itemObj.item).weaponWielding == WeaponWielding.OFF_HAND) |
                        (wSlot == WeaponSlot.OFF_HAND &
                        ((WeaponItem)itemObj.item).weaponWielding == WeaponWielding.MAIN_HAND))
                    {
                        newItemList.Remove(itemObj);
                    }
                }
            }
        }
        itemObjects = newItemList;
    }

    private void InsertionSort()
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

    private bool CheckIfShouldBeInFront (ItemObject obj1, ItemObject obj2)
    {
        bool returnValue = false;
        if (sortBy == InventorySortBy.LEVEL)
        {
            returnValue = obj1.item.level > obj2.item.level;
        }
        if (sortBy == InventorySortBy.NAME)
        {
            returnValue = String.Compare(obj1.item.itemName, obj2.item.itemName, comparisonType: StringComparison.OrdinalIgnoreCase) > 0 ? true : false;
        }
        if (sortBy == InventorySortBy.QUANTITY)
        {
            returnValue = obj1.quantity > obj2.quantity;
        }
        if (sortOrder == InventorySortOrder.DESCENDING)
            returnValue = !returnValue;
        return returnValue;
    }

    public void UseClickedItemObject(ItemObject itemObject)
    {
        if(itemObject.item.GetType() == typeof(ConsumableItem))
        {
            if (GlobalInput.CheckIfSelectedCharacter())
            {
                ((ConsumableItem)itemObject.item).UseConsumableOnCharacter(GlobalInput.Instance.selectedEntity.GetComponent<CharStats>());
            }
        }
        else if (itemObject.item.GetType() == typeof(WeaponItem) | itemObject.item.GetType() == typeof(ArmorItem))
        {
            if (GlobalInput.CheckIfSelectedCharacter())
            {
                GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>().EquipItem(itemObject);
            }
        }
    }
}
