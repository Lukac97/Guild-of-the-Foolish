using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SortingController : MonoBehaviour
{
    public List<SortingObject> sortingObjects;
    public InventoryList inventoryList;

    public Color enabledOrderColor;
    public Color disabledOrderColor;

    private void Start()
    {
        inventoryList = GetComponentInParent<InventoryList>();
        sortingObjects = new List<SortingObject>();
        foreach(Transform child in gameObject.transform)
        {
            SortingObject sortObj = child.GetComponent<SortingObject>();
            sortObj.InitSortingObject();
            sortingObjects.Add(sortObj);
        }
    }

    public void TurnOffAllOrder()
    {
        foreach(SortingObject child in sortingObjects)
        {
            child.TurnOffOrder();
        }
    }

}
