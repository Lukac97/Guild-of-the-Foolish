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

    [Header("Layout groups")]
    [Space(3)]
    [SerializeField]
    private LayoutGroup itemInfoLG;
    [SerializeField]
    private LayoutGroup itemComparisonLG;

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
    private GameObject equippedStats;
    [SerializeField]
    private TextMeshProUGUI toEquipName;
    [SerializeField]
    private TextMeshProUGUI equippedName;
    [SerializeField]
    private Image toEquipIcon;
    [SerializeField]
    private Image equippedIcon;

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
        DisplayItemsDetails(itemToEquip.item, toEquipStats, toEquipIcon, null, toEquipName, itemComparisonLG);
        if (armorSlot != null)
        {
            DisplayItemsDetails(armorSlot.item, equippedStats, equippedIcon, null, equippedName, itemComparisonLG);
        }
        else
        {
            DisplayItemsDetails(weaponSlot.item, equippedStats, equippedIcon, null, equippedName, itemComparisonLG);
        }
    }

    private void InitContent()
    {
        ActivateMainSlotPanel();

        if (weaponSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(weaponSlot.slot);
            DisplayItemsDetails(weaponSlot.item, attributesPanel, itemIcon, itemDescription, itemName, itemInfoLG);
        }
        else if (armorSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(armorSlot.slot);
            DisplayItemsDetails(armorSlot.item, attributesPanel, itemIcon, itemDescription, itemName, itemInfoLG);
        }
        else
        {
            titleText.text = "ERROR";
        }
    }

    private void DisplayItemsDetails(Item item, GameObject parentPanel, Image iconToSet,
        TextMeshProUGUI descToSet, TextMeshProUGUI nameToSet, LayoutGroup layoutGroup)
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


            if (equippedWeapon.attackDamageMultiplier != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ATK",
                    Mathf.FloorToInt(equippedWeapon.attackDamageMultiplier * 100), "%");
            }
            if (equippedWeapon.armorValue != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedWeapon.armorValue));
            }
            if (equippedWeapon.attributes.strength != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedWeapon.attributes.strength);
            }
            if (equippedWeapon.attributes.agility != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedWeapon.attributes.agility);
            }
            if (equippedWeapon.attributes.intellect != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedWeapon.attributes.intellect);
            }
            if (equippedWeapon.attributes.luck != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedWeapon.attributes.luck);
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

            if (equippedArmor.armorValue != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedArmor.armorValue));
            }
            if (equippedArmor.attributes.strength != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedArmor.attributes.strength);
            }
            if (equippedArmor.attributes.agility != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedArmor.attributes.agility);
            }
            if (equippedArmor.attributes.intellect != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedArmor.attributes.intellect);
            }
            if (equippedArmor.attributes.luck != 0)
            {
                GameObject gO = Instantiate(singleStatPrefab, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedArmor.attributes.luck);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
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
