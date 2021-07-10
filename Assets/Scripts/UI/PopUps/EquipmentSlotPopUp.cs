using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotPopUp : MonoBehaviour
{
    private static EquipmentSlotPopUp _instance;
    public static EquipmentSlotPopUp Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject activatable;
    [SerializeField]
    private InventoryList inventoryList;

    [Space(5)]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemName;
    public Image itemIcon;
    [Space(5)]
    public GameObject attributesPanel;
    public GameObject singleStatPrefab;
    public GameObject cmpStatPrefab;

    [Header("Layout groups")]
    [Space(3)]

    [Space(6)]
    [SerializeField]
    private CanvasGroup mainSlotPanel;
    [SerializeField]
    private CanvasGroup changeEquippedSlotPanel;

    [Header("Comparison")]
    [Space(10)]

    [SerializeField]
    private CanvasGroup itemDetailsPanel;
    [SerializeField]
    private CanvasGroup itemDetailsButtonsPanel;

    [SerializeField]
    private CanvasGroup availableItemsPanel;
    [SerializeField]
    private CanvasGroup availableItemsButtonsPanel;

    [Space(10)]
    [SerializeField]
    private GameObject toEquipStats;
    [SerializeField]
    private TextMeshProUGUI toEquipName;
    [SerializeField]
    private TextMeshProUGUI toEquipDesc;
    [SerializeField]
    private Image toEquipIcon;

    private CharEquipment.ArmorSlotItem armorSlot;
    private CharEquipment.WeaponSlotItem weaponSlot;
    private GameObject currentChar;
    private ItemObject itemToEquip;

    private PopUpController popUpPanel;

    void Awake()
    {
        if (Instance == null)
            _instance = this;
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void OpenEquipmentSlot(GameObject selectedChar, CharEquipment.ArmorSlotItem _armorSlot)
    {
        if (selectedChar == null)
            return;
        if (_armorSlot == null)
            return;
        currentChar = selectedChar;
        armorSlot = _armorSlot;
        weaponSlot = null;
        InitContent();
        popUpPanel.ActivatePopUp(activatable);
    }

    public void OpenEquipmentSlot(GameObject selectedChar, CharEquipment.WeaponSlotItem _weaponSlot)
    {
        if (selectedChar == null)
            return;
        if (_weaponSlot == null)
            return;
        currentChar = selectedChar;
        weaponSlot = _weaponSlot;
        armorSlot = null;
        //for filtering of 
        InitContent();
        popUpPanel.ActivatePopUp(activatable);
    }

    public void OnClickChange()
    {
        InitItemList();
        ActivateChangeEquippedPanel();
    }

    public void OnClickEquipYes()
    {
        bool result = false;
        if(armorSlot != null)
        {
            result = currentChar.GetComponent<CharEquipment>().EquipItem(itemToEquip, armorSlot);
        }
        else if (weaponSlot != null)
        {
            result = currentChar.GetComponent<CharEquipment>().EquipItem(itemToEquip, weaponSlot);
        }

        if (result)
        {
            InitContent();
        }
        else
        {
            Debug.Log("Cant equip this item");
        }
    }

    public void OnClickEquipNo()
    {
        InitItemList();
        ActivateAvailableItemsPanel();
    }

    public void OnClickOnItem(ItemObject itemObj)
    {
        itemToEquip = itemObj;
        InitItemComparison();
        ActivateItemDetailsPanel();
    }
    
    public void OnClickCancelChange()
    {
        ActivateMainSlotPanel();
    }

    private void InitItemList()
    {
        inventoryList.EnableFilterByEquipmentSlotPreset(currentChar.GetComponent<CharEquipment>(),
            armorSlot == null ? null : (ArmorSlot?)armorSlot.slot,
            weaponSlot == null ? null : (WeaponSlot?)weaponSlot.slot);
    }

    private void InitItemComparison()
    {
        DisplayItemsDetails(itemToEquip.item, toEquipStats, toEquipIcon, toEquipDesc, toEquipName,
            bolCmp: true,
            itemComparison: weaponSlot != null ? (Item)weaponSlot.item : (armorSlot != null ? (Item)armorSlot.item : null));
        //if (armorSlot != null)
        //{
        //    DisplayItemsDetails(armorSlot.item, equippedStats, equippedIcon, null, equippedName, itemComparisonLG);
        //}
        //else
        //{
        //    DisplayItemsDetails(weaponSlot.item, esquippedStats, equippedIcon, null, equippedName, itemComparisonLG);
        //}
    }

    private void InitContent()
    {
        ActivateMainSlotPanel();

        if (weaponSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(weaponSlot.slot);
            DisplayItemsDetails(weaponSlot.item, attributesPanel, itemIcon, itemDescription, itemName);
        }
        else if (armorSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(armorSlot.slot);
            DisplayItemsDetails(armorSlot.item, attributesPanel, itemIcon, itemDescription, itemName);
        }
        else
        {
            titleText.text = "ERROR";
        }
    }

    private void DisplayItemsDetails(Item item, GameObject parentPanel, Image iconToSet,
        TextMeshProUGUI descToSet, TextMeshProUGUI nameToSet, bool bolCmp = false, Item itemComparison = null)
    {
        foreach (Transform child in parentPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (item == null)
        {
            if (iconToSet != null)
                iconToSet.enabled = false;
            if (nameToSet != null)
                nameToSet.text = "";
            if (descToSet != null)
                descToSet.text = "";
            return;
        }

        GameObject prefabToUse = bolCmp ? cmpStatPrefab : singleStatPrefab;

        if (item is WeaponItem)
        {
            WeaponItem equippedWeapon = (WeaponItem)item;


            if (descToSet != null)
            {
                descToSet.text = equippedWeapon.itemDescription;
            }
            if (iconToSet != null)
            {
                iconToSet.enabled = true;
                iconToSet.sprite = equippedWeapon.itemIcon;
            }
            if (nameToSet != null)
            {
                nameToSet.text = equippedWeapon.name;
            }

            if (equippedWeapon.attackDamageMultiplier != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attackDamageMultiplier != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ATK",
                    Mathf.FloorToInt(equippedWeapon.attackDamageMultiplier * 100), "%",
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((WeaponItem) itemComparison).attackDamageMultiplier * 100) : 0));
            }
            if (equippedWeapon.armorValue != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).armorValue != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedWeapon.armorValue),
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((WeaponItem)itemComparison).armorValue) : 0));
            }
            if (equippedWeapon.attributes.strength != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.strength != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedWeapon.attributes.strength,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((WeaponItem)itemComparison).attributes.strength) : 0));
            }
            if (equippedWeapon.attributes.agility != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.agility != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedWeapon.attributes.agility,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((WeaponItem)itemComparison).attributes.agility) : 0));
            }
            if (equippedWeapon.attributes.intellect != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.intellect != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedWeapon.attributes.intellect,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((WeaponItem)itemComparison).attributes.intellect) : 0));
            }
            if (equippedWeapon.attributes.luck != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.luck != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedWeapon.attributes.luck,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((WeaponItem)itemComparison).attributes.luck) : 0));
            }
        }
        else if (item is ArmorItem)
        {
            ArmorItem equippedArmor = (ArmorItem) item;

            parentPanel.SetActive(true);

            if (descToSet != null)
            {
                descToSet.enabled = true;
                descToSet.text = equippedArmor.itemDescription;
            }
            if (iconToSet != null)
            {
                iconToSet.enabled = true;
                iconToSet.sprite = equippedArmor.itemIcon;
            }
            if (nameToSet != null)
            {
                nameToSet.enabled = true;
                nameToSet.text = equippedArmor.name;
            }

            if (equippedArmor.armorValue != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).armorValue != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedArmor.armorValue),
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((ArmorItem)itemComparison).armorValue) : 0));
            }
            if (equippedArmor.attributes.strength != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.strength != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedArmor.attributes.strength,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((ArmorItem)itemComparison).attributes.strength) : 0));
            }
            if (equippedArmor.attributes.agility != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.agility != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedArmor.attributes.agility,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((ArmorItem)itemComparison).attributes.agility) : 0));
            }
            if (equippedArmor.attributes.intellect != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.intellect != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedArmor.attributes.intellect,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((ArmorItem)itemComparison).attributes.intellect) : 0));
            }
            if (equippedArmor.attributes.luck != 0 |
                (bolCmp & (itemComparison != null ? ((WeaponItem)itemComparison).attributes.luck != 0 : false)))
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedArmor.attributes.luck,
                    _statChange: (itemComparison != null ?
                    Mathf.FloorToInt(((ArmorItem)itemComparison).attributes.luck) : 0));
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
    }

    private void ActivateMainSlotPanel()
    {
        ActivateCanvasGroup(mainSlotPanel, true);
        ActivateCanvasGroup(changeEquippedSlotPanel, false);
    }

    private void ActivateChangeEquippedPanel()
    {
        ActivateCanvasGroup(changeEquippedSlotPanel, true);
        ActivateCanvasGroup(mainSlotPanel, false);

        ActivateAvailableItemsPanel();
    }

    private void ActivateAvailableItemsPanel()
    {
        ActivateCanvasGroup(availableItemsPanel, true);
        ActivateCanvasGroup(availableItemsButtonsPanel, true);
        ActivateCanvasGroup(itemDetailsPanel, false);
        ActivateCanvasGroup(itemDetailsButtonsPanel, false);
    }

    private void ActivateItemDetailsPanel()
    {
        ActivateCanvasGroup(itemDetailsPanel, true);
        ActivateCanvasGroup(itemDetailsButtonsPanel, true);
        ActivateCanvasGroup(availableItemsPanel, false);
        ActivateCanvasGroup(availableItemsButtonsPanel, false);
    }

    public void ActivateCanvasGroup(CanvasGroup cg, bool doActivate)
    {
        cg.alpha = doActivate ? 1 : 0;
        cg.interactable = doActivate;
        cg.blocksRaycasts = doActivate;
    }

    public void CloseEquipmentSlot()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }
}
