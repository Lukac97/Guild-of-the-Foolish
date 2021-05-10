using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionController : MonoBehaviour
{
    public GameObject itemNamePanel;
    public GameObject itemDescriptionPanel;
    [Header("Item info")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemLevel;
    public TextMeshProUGUI itemDescription;
    public Image icon;

    private void Start()
    {
        GlobalInput.Instance.changeSelectedItemObject += UpdateSelectedItem;
        UpdateSelectedItem();
    }

    public void UpdateSelectedItem()
    {
        ItemObject itemObject = GlobalInput.Instance.selectedItemObject;
        if(itemObject == null)
        {
            itemNamePanel.SetActive(false);
            itemDescriptionPanel.SetActive(false);
            return;
        }
        itemNamePanel.SetActive(true);
        itemDescriptionPanel.SetActive(true);
        itemName.text = itemObject.item.name;
        itemLevel.text = itemObject.item.level.ToString();
        itemDescription.text = itemObject.item.itemDescription;
        icon.sprite = itemObject.item.itemIcon;
    }
}
