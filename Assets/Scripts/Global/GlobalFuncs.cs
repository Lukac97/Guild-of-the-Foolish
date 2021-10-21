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

        itemRes.itemID = itemPredef.armorItem.itemID;

        return itemRes;
    }

    public static Item GenerateItemFromPredefined(WeaponItemPredefined itemPredef)
    {
        WeaponItem itemRes = new WeaponItem(itemPredef.weaponItem);

        itemRes.itemID = itemPredef.weaponItem.itemID;

        return itemRes;
    }

    public static Item GenerateItemFromPredefined(ConsumableItemPredefined itemPredef)
    {
        ConsumableItem itemRes = new ConsumableItem(itemPredef.consumableItem);

        itemRes.itemID = itemPredef.consumableItem.itemID;

        return itemRes;
    }

    public static Item GenerateItemFromPredefined(IngredientItemPredefined itemPredef)
    {
        IngredientItem itemRes = new IngredientItem(itemPredef.ingredientItem);

        itemRes.itemID = itemPredef.ingredientItem.itemID;

        return itemRes;
    }

    public static Item GenerateItemFromMould(ItemMould itemMould, int level)
    {
        Item newItem = null;
        if (itemMould.GetType() == typeof(ArmorItemMould))
            newItem = GenerateItemFromMould((ArmorItemMould)itemMould, level);
        else if (itemMould.GetType() == typeof(WeaponItemMould))
            newItem = GenerateItemFromMould((WeaponItemMould)itemMould, level);

        newItem.itemID = GlobalRules.CreateIDForMouldedItem(itemMould);

        return newItem;
    }


    private static Item GenerateItemFromMould(ArmorItemMould itemMould, int level)
    {
        //TODO: Implement logic for generating item based on item mould
        ArmorItem itemRes = new ArmorItem();
        itemRes.isMoulded = true;
        itemRes.itemSlot = itemMould.itemSlot;
        itemRes.armorType = itemMould.armorType;
        itemRes.level = level;
        itemRes.attributes = GenerateRandomEquipmentAttributes(level);
        itemRes.armorValue = GlobalRules.GetEquipmentArmorPointsForLevel(level);

        itemRes.itemName = GenerateItemName(itemMould);
        itemRes.itemIcon = new List<IconPartWithShadow>(GenerateSpriteIcon(itemMould));

        return itemRes;
    }

    private static Item GenerateItemFromMould(WeaponItemMould itemMould, int level)
    {
        //TODO: Implement logic for generating item based on item mould
        WeaponItem itemRes = new WeaponItem();
        itemRes.isMoulded = true;
        itemRes.weaponWielding = itemMould.weaponWielding;
        itemRes.weaponRange = itemMould.weaponRange;
        itemRes.weaponType = itemMould.weaponType;
        itemRes.level = level;
        itemRes.attributes = GenerateRandomEquipmentAttributes(level);
        itemRes.armorValue = GlobalRules.GetEquipmentArmorPointsForLevel(level);
        itemRes.attackDamageMultiplier = Random.Range(GlobalRules.equipmentAttackMultiplierRangeMin, GlobalRules.equipmentAttackMultiplierRangeMax);

        itemRes.itemName = GenerateItemName(itemMould);
        itemRes.itemIcon = new List<IconPartWithShadow>(GenerateSpriteIcon(itemMould));

        return itemRes;
    }

    private static Attributes GenerateRandomEquipmentAttributes(int level)
    {
        int attributePts = GlobalRules.GetEquipmentAttributePointsForLevel(level);
        Attributes newAttributes = new Attributes();
        Attributes rndProportions = new Attributes();
        rndProportions.RandomizeAttributes(0, 10);
        newAttributes.ProportionallyAssignPts(rndProportions, attributePts);
        return newAttributes;
    }

    private static string GenerateItemName(ItemMould itemMould)
    {
        //TODO: Create logic for generating random item name
        string newItemName = itemMould.itemNamePrefix[Random.Range(0, itemMould.itemNamePrefix.Count)] +
             " " + itemMould.itemNameType[Random.Range(0, itemMould.itemNameType.Count)] +
            " of " + itemMould.itemNameBonus[Random.Range(0, itemMould.itemNameBonus.Count)];
        return newItemName;
    }

    private static List<IconPartWithShadow> GenerateSpriteIcon(ItemMould itemMould)
    {
        List<ItemMould.SpriteChoice> allSpriteChoices = new List<ItemMould.SpriteChoice>
             { itemMould.primarySpriteChoices, itemMould.secondarySpriteChoices, itemMould.tertiarySpriteChoices };
        List<ItemMould.IconMouldWithShadow> iconSpriteMoulds = new List<ItemMould.IconMouldWithShadow>();
        List<ItemMould.ColorCustomization> iconColorCustomizations = new List<ItemMould.ColorCustomization>();
        foreach(ItemMould.SpriteChoice spriteChoice in allSpriteChoices)
        {
            if (spriteChoice.spriteChoices.Count > 0)
            {
                iconSpriteMoulds.Add(spriteChoice.spriteChoices
                    [Random.Range(0, spriteChoice.spriteChoices.Count)]);
                iconColorCustomizations.Add(spriteChoice.colorCustomization);
            }
        }

        List<IconPartWithShadow> finalIcon = new List<IconPartWithShadow>();
        for(int i = 0; i < iconSpriteMoulds.Count; i++)
        {
            IconPartWithShadow iconPart = new IconPartWithShadow();
            iconPart.spritePart = iconSpriteMoulds[i].spritePart;
            iconPart.spritePartShadow = iconSpriteMoulds[i].spritePartShadow;
            iconPart.partColor = GenerateColorForIconPartMould(iconSpriteMoulds[i], iconColorCustomizations[i]);
            finalIcon.Add(iconPart);
        }

        return finalIcon;
    }

    private static Color GenerateColorForIconPartMould(ItemMould.IconMouldWithShadow iconPartMould, ItemMould.ColorCustomization colorC)
    {
        float H;
        float S;
        float V;
        if(colorC.colorSaturationRangeMin == 0 && colorC.colorSaturationRangeMax == 0)
        {
            S = Random.Range(0.2f, 0.8f);
        }
        else
        {
            S = Random.Range(colorC.colorSaturationRangeMin, colorC.colorSaturationRangeMax);
        }

        if(colorC.colorValueMin == 0 && colorC.colorValueMax == 0)
        {
            V = Random.Range(0.2f, 0.8f);
        }
        else
        {
            V = Random.Range(colorC.colorValueMin, colorC.colorValueMax);
        }

        H = Random.Range(0.0f, 1.0f);
        Color finalColor = Color.HSVToRGB(H, S, V);
        finalColor.a = 1;
        return finalColor;
    }

    /// <summary>
    /// Gets item from scriptable object which implements PredefinedItemsInterface.
    /// </summary>
    /// <param name="predefinedItem">ScriptableObject which implements PredefinedItemsInterface</param>
    /// <returns></returns>
    public static Item GetItemFromScriptableObject(ScriptableObject predefinedItem)
    {
        if (predefinedItem == null)
            return null;
        if (predefinedItem is PredefinedItemsInterface)
        {
            if (predefinedItem.GetType() == typeof(ArmorItemPredefined))
            {
                return ((ArmorItemPredefined)predefinedItem).armorItem;
            }
            else if (predefinedItem.GetType() == typeof(WeaponItemPredefined))
            {
                return ((WeaponItemPredefined)predefinedItem).weaponItem;
            }
            else if (predefinedItem.GetType() == typeof(ConsumableItemPredefined))
            {
                return ((ConsumableItemPredefined)predefinedItem).consumableItem;
            }
            else if (predefinedItem.GetType() == typeof(IngredientItemPredefined))
            {
                return ((IngredientItemPredefined)predefinedItem).ingredientItem;
            }
            else
            {
                Debug.Log("Scriptable is of PredefinedItemsInterface, but not of any specific predefined items type.");
                return null;
            }
        }
        else
        {
            Debug.Log("Scriptable is not of PredefinedItemsInterface.");
            return null;
        }
    }

    #endregion Item_Helper

    #region UI_Helper

    public static void SetActiveCanvasGroup(CanvasGroup canvasGroup, bool activate)
    {
        if (canvasGroup == null)
            return;
        canvasGroup.alpha = activate ? 1 : 0;
        canvasGroup.blocksRaycasts = activate;
        canvasGroup.interactable = activate;
    }

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
