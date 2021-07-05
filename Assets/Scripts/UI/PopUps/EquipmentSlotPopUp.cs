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

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI itemDescription;
    public Image itemIcon;
    public GameObject attributesPanel;
    public GameObject singleStatPrefab;

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

    private PopUpController popUpPanel;
    private CharEquipment.ArmorSlotItem armorSlot;
    private CharEquipment.WeaponSlotItem weaponSlot;
    private GameObject currentChar;
    private ItemObject itemToEquip;
    void Awake()
    {
        if (Instance == null)
            _instance = this;
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenEquipmentSlot(GameObject selectedChar, CharEquipment.ArmorSlotItem _armorSlot)
    {
        if (selectedChar == null)
            return;
        if (armorSlot == null)
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
        if (weaponSlot == null)
            return;
        currentChar = selectedChar;
        weaponSlot = _weaponSlot;
        armorSlot = null;
        InitContent();
        popUpPanel.ActivatePopUp(activatable);
    }

    public void OnClickChange()
    {
        ActivateChangeEquippedPanel();
    }

    public void OnClickEquipYes()
    {
        currentChar.GetComponent<CharEquipment>().EquipItem(itemToEquip);
    }

    public void OnClickEquipNo()
    {
        ActivateAvailableItemsPanel();
    }

    public void OnClickOnItem(ItemObject itemObj)
    {
        itemToEquip = itemObj;
        ActivateItemDetailsPanel();
    }

    public void OnClickCancelChange()
    {
        ActivateMainSlotPanel();
    }

    private void InitContent()
    {
        ActivateMainSlotPanel();

        if (weaponSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(weaponSlot.slot);
            DisplayItemsDetails(weaponSlot.item, attributesPanel, itemIcon, itemDescription);
        }
        else if (armorSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(armorSlot.slot);
            DisplayItemsDetails(armorSlot.item, attributesPanel, itemIcon, itemDescription);
        }
        else
        {
            titleText.text = "ERROR";
        }
    }

    private void DisplayItemsDetails(Item item, GameObject parentPanel, Image iconToSet, TextMeshProUGUI descToSet)
    {
        if (item == null)
            return;

        foreach (Transform child in parentPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (item is WeaponItem)
        {
            WeaponItem equippedWeapon = (WeaponItem)item;

            if (equippedWeapon == null)
            {
                iconToSet.enabled = false;
                descToSet.enabled = false;
                parentPanel.SetActive(false);
            }
            else
            {
                iconToSet.enabled = true;
                descToSet.enabled = true;
                parentPanel.SetActive(true);

                descToSet.text = equippedWeapon.itemDescription;
                iconToSet.sprite = equippedWeapon.itemIcon;

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
        }
        else if (item is ArmorItem)
        {
            ArmorItem equippedArmor = (ArmorItem) item;

            if (equippedArmor == null)
            {
                iconToSet.enabled = false;
                descToSet.enabled = false;
                parentPanel.SetActive(false);
            }
            else
            {
                iconToSet.enabled = true;
                descToSet.enabled = true;
                parentPanel.SetActive(true);

                descToSet.text = equippedArmor.itemDescription;
                iconToSet.sprite = equippedArmor.itemIcon;



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
        }
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
