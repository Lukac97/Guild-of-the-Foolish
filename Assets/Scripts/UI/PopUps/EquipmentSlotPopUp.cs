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

    private PopUpController popUpPanel;
    private CharEquipment.ArmorSlotItem armorSlot;
    private CharEquipment.WeaponSlotItem weaponSlot;
    private GameObject currentChar;

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
        currentChar = selectedChar;
        weaponSlot = _weaponSlot;
        armorSlot = null;
        InitContent();
        popUpPanel.ActivatePopUp(activatable);
    }

    private void InitContent()
    {
        foreach (Transform child in attributesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (weaponSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(weaponSlot.slot);

            WeaponItem equippedWeapon = weaponSlot.item;

            if (equippedWeapon == null)
            {
                itemIcon.enabled = false;
                itemDescription.enabled = false;
                attributesPanel.SetActive(false);
            }
            else
            {
                itemIcon.enabled = true;
                itemDescription.enabled = true;
                attributesPanel.SetActive(true);

                itemDescription.text = equippedWeapon.itemDescription;
                itemIcon.sprite = equippedWeapon.itemIcon;

                if (equippedWeapon.attackDamageMultiplier != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("ATK",
                        Mathf.FloorToInt(equippedWeapon.attackDamageMultiplier * 100), "%");
                }
                if (equippedWeapon.armorValue != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedWeapon.armorValue));
                }
                if (equippedWeapon.attributes.strength != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("STR", equippedWeapon.attributes.strength);
                }
                if (equippedWeapon.attributes.agility != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("AGI", equippedWeapon.attributes.agility);
                }
                if (equippedWeapon.attributes.intellect != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("INT", equippedWeapon.attributes.intellect);
                }
                if (equippedWeapon.attributes.luck != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("LUK", equippedWeapon.attributes.luck);
                }
            }
        }
        else if (armorSlot != null)
        {
            titleText.text = EnumToString.ToNiceString(armorSlot.slot);

            ArmorItem equippedArmor = armorSlot.item;

            if (equippedArmor == null)
            {
                itemIcon.enabled = false;
                itemDescription.enabled = false;
                attributesPanel.SetActive(false);
            }
            else
            {
                itemIcon.enabled = true;
                itemDescription.enabled = true;
                attributesPanel.SetActive(true);
                
                itemDescription.text = equippedArmor.itemDescription;
                itemIcon.sprite = equippedArmor.itemIcon;



                if (equippedArmor.armorValue != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("ARMOR", Mathf.FloorToInt(equippedArmor.armorValue));
                }
                if (equippedArmor.attributes.strength != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("STR", equippedArmor.attributes.strength);
                }
                if (equippedArmor.attributes.agility != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("AGI", equippedArmor.attributes.agility);
                }
                if (equippedArmor.attributes.intellect != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("INT", equippedArmor.attributes.intellect);
                }
                if (equippedArmor.attributes.luck != 0)
                {
                    GameObject gO = Instantiate(singleStatPrefab, attributesPanel.transform);
                    gO.GetComponent<SingleEqStat>().SetText("LUK", equippedArmor.attributes.luck);
                }
            }
        }
    }

    public void CloseEquipmentSlot()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }
}
