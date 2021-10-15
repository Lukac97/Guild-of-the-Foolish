using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDescriptionController : MonoBehaviour
{
    public GameObject itemStatsPanel;
    public GameObject singleItemStatPrefab;
    [Header("Item info")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemLevel;
    public TextMeshProUGUI itemDescription;
    public ItemIconHandler icon;

    private Item currentItem = null;
    private void Start()
    {
        UpdateSelectedItem();
    }

    public void ShowItemDescription(Item item)
    {
        currentItem = item;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        UpdateSelectedItem();
    }

    public void HideItemDescription()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void UpdateSelectedItem()
    {
        if(currentItem == null)
        {
            HideItemDescription();
            return;
        }
        itemName.text = currentItem.itemName;
        itemLevel.text = currentItem.level.ToString();
        itemDescription.text = currentItem.itemDescription;
        icon.InitItemIconHandler(currentItem);

        foreach(SingleEqStat child in itemStatsPanel.GetComponentsInChildren<SingleEqStat>())
        {
            Destroy(child.gameObject);
        }

        if (currentItem is WeaponItem)
        {
            itemStatsPanel.SetActive(true);
            WeaponItem equippedWeapon = (WeaponItem)currentItem;

            if (equippedWeapon.attackDamageMultiplier != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ATK",
                    Mathf.FloorToInt(equippedWeapon.attackDamageMultiplier * 100), "%");
            }
            if (equippedWeapon.armorValue != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedWeapon.armorValue));
            }
            if (equippedWeapon.attributes.strength != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedWeapon.attributes.strength);
            }
            if (equippedWeapon.attributes.agility != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedWeapon.attributes.agility);
            }
            if (equippedWeapon.attributes.intellect != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedWeapon.attributes.intellect);
            }
            if (equippedWeapon.attributes.luck != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedWeapon.attributes.luck);
            }
        }
        else if (currentItem is ArmorItem)
        {
            itemStatsPanel.SetActive(true);
            ArmorItem equippedArmor = (ArmorItem)currentItem;

            if (equippedArmor.armorValue != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedArmor.armorValue));
            }
            if (equippedArmor.attributes.strength != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedArmor.attributes.strength);
            }
            if (equippedArmor.attributes.agility != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedArmor.attributes.agility);
            }
            if (equippedArmor.attributes.intellect != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedArmor.attributes.intellect);
            }
            if (equippedArmor.attributes.luck != 0)
            {
                GameObject gO = Instantiate(singleItemStatPrefab, itemStatsPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedArmor.attributes.luck);
            }
        }
        else
        {
            itemStatsPanel.SetActive(false);
        }
        GlobalFuncs.PackGridLayoutWithScroll(itemStatsPanel, itemStatsPanel, 5, spacingPercentage: .1f);
    }
}
