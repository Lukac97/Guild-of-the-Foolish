using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalFuncs : MonoBehaviour
{
    private static GlobalFuncs _instance;
    public static GlobalFuncs Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    #region Equipment_Helper

    public static bool CheckIfWorse(ArmorItem armor1, ArmorItem armor2)
    {
        if (armor1 == null)
            return true;
        if (armor2 == null)
            return false;
        EquipItemComparison.ArmorComparison armorCmp1 = new EquipItemComparison.ArmorComparison(armor1);
        EquipItemComparison.ArmorComparison armorCmp2 = new EquipItemComparison.ArmorComparison(armor2);
        if (armorCmp1.CalculateRating() < armorCmp2.CalculateRating())
            return true;
        else
            return false;
    }
    public static bool CheckIfWorse(WeaponItem weapon1, WeaponItem weapon2)
    {
        if (weapon1 == null)
            return true;
        if (weapon2 == null)
            return false;
        EquipItemComparison.WeaponComparison weaponCmp1 = new EquipItemComparison.WeaponComparison(weapon1);
        EquipItemComparison.WeaponComparison weaponCmp2 = new EquipItemComparison.WeaponComparison(weapon2);
        if (weaponCmp1.CalculateRating() < weaponCmp2.CalculateRating())
            return true;
        else
            return false;
    }

    #endregion Equipment_Helper

    #region Item_Helper

    public static Item GenerateItemFromPredefined(ArmorItemPredefined itemPredef)
    {
        ArmorItem itemRes = new ArmorItem(itemPredef.armorItem);
        return itemRes;
    }

    public static Item GenerateItemFromPredefined(WeaponItemPredefined itemPredef)
    {
        WeaponItem itemRes = new WeaponItem(itemPredef.weaponItem);
        return itemRes;
    }

    public static Item GenerateItemFromMould(ArmorItemMould itemMould)
    {
        //TODO: Implement logic for generating item based on item mould
        ArmorItem itemRes = new ArmorItem();
        return itemRes;
    }

    private static string GenerateItemName(ItemMould itemMould)
    {
        //TODO: Create logic for generating random item name
        return "";
    }


    #endregion Item_Helper

    #region UI_Helper

    //Returns cell size
    public static int PackGridLayoutSquare(GameObject gridPanel, int itemsPerPage, int fixedCellSize = 0,
        float spacingPercentage = 0.2f)
    {
        GridLayoutGroup gridLayoutGroup = gridPanel.GetComponent<GridLayoutGroup>();
        float maxWidth = gridPanel.GetComponent<RectTransform>().rect.width;
        float maxHeight = gridPanel.GetComponent<RectTransform>().rect.height;
        int columnNumber = gridLayoutGroup.constraintCount;
        int rowNumber = Mathf.CeilToInt(itemsPerPage / columnNumber);

        int cellSize = 0;

        if (fixedCellSize > 0)
        {
            cellSize = fixedCellSize;
        }

        if (fixedCellSize <= 0 | (cellSize * columnNumber > maxWidth) | (cellSize * rowNumber > maxHeight))
        {
            int cellWidth = Mathf.FloorToInt(((1 - spacingPercentage) * maxWidth) / columnNumber);
            int cellHeight = Mathf.FloorToInt(((1 - spacingPercentage) * maxHeight) / rowNumber);
            cellSize = Mathf.Min(cellWidth, cellHeight);
        }

        int spacingX = Mathf.FloorToInt((maxWidth - cellSize * columnNumber) / (columnNumber + 1));
        int spacingY = Mathf.FloorToInt((maxHeight - cellSize * rowNumber) / (rowNumber + 1));

        gridLayoutGroup.spacing = new Vector2(spacingX, spacingY);
        gridLayoutGroup.padding.left = spacingX;
        gridLayoutGroup.padding.right = spacingX;
        gridLayoutGroup.padding.top = spacingY;
        gridLayoutGroup.padding.bottom = spacingY;

        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        return cellSize;
    }

    public static Vector2 PackGridLayoutWithScroll(GameObject scrollGO, GameObject gridPanel, float widthToHeightRatio,
        float spacingPercentage = 0.05f)
    {
        GridLayoutGroup gridLayoutGroup = gridPanel.GetComponent<GridLayoutGroup>();
        float maxWidth = scrollGO.GetComponent<RectTransform>().rect.width;
        int columnNr = gridLayoutGroup.constraintCount;

        int cellWidth = Mathf.FloorToInt(((1-spacingPercentage) * maxWidth) / columnNr);
        int cellHeight = Mathf.FloorToInt(cellWidth / widthToHeightRatio);
        int spacing = Mathf.FloorToInt((spacingPercentage * maxWidth) / (columnNr + 1));

        gridLayoutGroup.spacing = new Vector2(spacing, spacing);
        gridLayoutGroup.padding.left = spacing;
        gridLayoutGroup.padding.right = spacing;
        gridLayoutGroup.padding.top = spacing;
        gridLayoutGroup.padding.bottom = spacing;

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        return gridLayoutGroup.cellSize;
    }

    public static float EqualizeToMinFontSize(List<TextMeshProUGUI> textMeshes)
    {
        if (textMeshes.Count == 0)
            return -1f;

        float minFontSize = textMeshes[0].fontSize;
        foreach(TextMeshProUGUI tmp in textMeshes)
        {
            if(tmp.fontSize < minFontSize)
            {
                minFontSize = tmp.fontSize;
            }
        }

        foreach(TextMeshProUGUI tmp in textMeshes)
        {
            tmp.enableAutoSizing = false;
            tmp.fontSize = minFontSize;
        }

        return minFontSize;
    }

    #endregion UI_Helper

}
