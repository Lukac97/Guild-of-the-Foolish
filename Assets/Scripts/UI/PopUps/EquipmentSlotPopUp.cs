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
    [SerializeField] private GameObject itemPanel;
    [Space(10)]
    [SerializeField] private GameObject deselectButton;
    [SerializeField] private GameObject toEquipStats;
    [SerializeField] private TextMeshProUGUI toEquipName;
    [SerializeField] private TextMeshProUGUI toEquipDesc;
    [SerializeField] private TextMeshProUGUI equipButtonText;
    [SerializeField] private Image toEquipIcon;

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

    private void Start()
    {
        GlobalFuncs.PackGridLayoutSquare(itemPanel, itemPanel.transform.childCount);
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

    public void OnClickEquip()
    {
        bool result = false;
        Item temp = null;
        if(armorSlot != null)
        {
            temp = armorSlot.item;
            if (itemToEquip == null)
            {
                result = currentChar.GetComponent<CharEquipment>().UnequipSlot(armorSlot);
            }
            else
            {
                result = currentChar.GetComponent<CharEquipment>().EquipItem(itemToEquip, armorSlot);
            }
        }
        else if (weaponSlot != null)
        {
            temp = weaponSlot.item;
            if (itemToEquip == null)
            {
                result = currentChar.GetComponent<CharEquipment>().UnequipSlot(weaponSlot);
            }
            else
            {
                result = currentChar.GetComponent<CharEquipment>().EquipItem(itemToEquip, weaponSlot);
            }
        }

        if (result)
        {
            itemToEquip = GuildInventory.Instance.FindItem(temp);
            InitContent();
        }
        else
        {
            Debug.Log("Cant equip this item");
        }
    }

    public void OnClickDeselectItemToEquip()
    {
        itemToEquip = null;
        UpdateItemToEquip();
    }

    public void OnClickOnItem(ItemObject itemObj)
    {
        itemToEquip = itemObj;
        UpdateItemToEquip();
    }

    private void InitItemList()
    {
        inventoryList.EnableFilterByEquipmentSlotPreset(currentChar.GetComponent<CharEquipment>(),
            armorSlot == null ? null : (ArmorSlot?)armorSlot.slot,
            weaponSlot == null ? null : (WeaponSlot?)weaponSlot.slot);
    }

    private void UpdateItemToEquip()
    {
        if (itemToEquip == null)
        {
            deselectButton.SetActive(false);
            equipButtonText.text = "UNEQUIP";
            DisplayItemsDetails(null, toEquipStats, toEquipIcon, toEquipDesc, toEquipName);
        }
        else
        {
            deselectButton.SetActive(true);
            equipButtonText.text = "EQUIP";
            DisplayItemsDetails(itemToEquip.item, toEquipStats, toEquipIcon, toEquipDesc, toEquipName);
        }
    }

    private void InitContent()
    {
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
        InitItemList();
        UpdateItemToEquip();
    }

    private void DisplayItemsDetails(Item item, GameObject parentPanel, Image iconToSet,
        TextMeshProUGUI descToSet, TextMeshProUGUI nameToSet)
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
        GameObject prefabToUse = singleStatPrefab;

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
                nameToSet.text = equippedWeapon.itemName;
            }

            if (equippedWeapon.attackDamageMultiplier != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ATK",
                    Mathf.FloorToInt(equippedWeapon.attackDamageMultiplier * 100), "%");
            }
            if (equippedWeapon.armorValue != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedWeapon.armorValue));
            }
            if (equippedWeapon.attributes.strength != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedWeapon.attributes.strength);
            }
            if (equippedWeapon.attributes.agility != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedWeapon.attributes.agility);
            }
            if (equippedWeapon.attributes.intellect != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedWeapon.attributes.intellect);
            }
            if (equippedWeapon.attributes.luck != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
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
                nameToSet.text = equippedArmor.itemName;
            }

            if (equippedArmor.armorValue != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedArmor.armorValue));
            }
            if (equippedArmor.attributes.strength != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("STR", equippedArmor.attributes.strength);
            }
            if (equippedArmor.attributes.agility != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("AGI", equippedArmor.attributes.agility);
            }
            if (equippedArmor.attributes.intellect != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("INT", equippedArmor.attributes.intellect);
            }
            if (equippedArmor.attributes.luck != 0)
            {
                GameObject gO = Instantiate(prefabToUse, parentPanel.transform);
                gO.GetComponent<SingleEqStat>().SetText("LUK", equippedArmor.attributes.luck);
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
    }

    public void CloseEquipmentSlot()
    {
        itemToEquip = null;
        popUpPanel.DeactivatePopUp(activatable);
    }
}
