using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotController : MonoBehaviour
{
    public GameObject equipmentPanel;
    public CharEquipment currentCharEquipment;
    public List<EquipmentArmorSlot> armorSlots;
    public List<EquipmentWeaponSlot> weaponSlots;

    void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += OnEntityChanged;
        CharactersController.Instance.CharactersUpdated += OnEntityChanged;
        armorSlots = new List<EquipmentArmorSlot>();
        armorSlots.AddRange(equipmentPanel.GetComponentsInChildren<EquipmentArmorSlot>());
        weaponSlots = new List<EquipmentWeaponSlot>();
        weaponSlots.AddRange(equipmentPanel.GetComponentsInChildren<EquipmentWeaponSlot>());
        foreach(EquipmentArmorSlot slot in armorSlots)
        {
            slot.SetItemSlot(null);
        }
        foreach (EquipmentWeaponSlot slot in weaponSlots)
        {
            slot.SetItemSlot(null);
        }
        OnEntityChanged();
    }



    private void OnEntityChanged()
    {
        if (!GlobalInput.Instance.CheckIfSelectedCharacter())
        {
            equipmentPanel.SetActive(false);
            return;
        }
        if (!equipmentPanel.activeSelf)
            equipmentPanel.SetActive(true);
        currentCharEquipment = GlobalInput.Instance.selectedEntity.GetComponent<CharEquipment>();

        List<EquipmentArmorSlot> leftArmorSlots = new List<EquipmentArmorSlot>(armorSlots);
        foreach(CharEquipment.ArmorSlotItem armorSlotItem in currentCharEquipment.armorSlots)
        {
            List<EquipmentArmorSlot> eqSlots = FindEquipmentSlots(armorSlotItem.slot);
            EquipmentArmorSlot eqSlot = null;
            foreach(EquipmentArmorSlot eSlot in eqSlots)
            {
                if (leftArmorSlots.Contains(eSlot))
                {
                    eqSlot = eSlot;
                    break;
                }
            }
            if(eqSlot != null & leftArmorSlots.Contains(eqSlot))
            {
                eqSlot.gameObject.SetActive(true);
                eqSlot.SetItemSlot(armorSlotItem);
                leftArmorSlots.Remove(eqSlot);
            }
        }
        foreach (EquipmentArmorSlot eqSlot in leftArmorSlots)
        {
            eqSlot.gameObject.SetActive(false);
        }

        List<EquipmentWeaponSlot> leftWeaponSlots = new List<EquipmentWeaponSlot>(weaponSlots);
        foreach (CharEquipment.WeaponSlotItem weaponSlotItem in currentCharEquipment.weaponSlots)
        {
            List<EquipmentWeaponSlot> eqSlots = FindEquipmentSlots(weaponSlotItem.slot);
            EquipmentWeaponSlot eqSlot = null;
            foreach(EquipmentWeaponSlot eSlot in eqSlots)
            {
                if(leftWeaponSlots.Contains(eSlot))
                {
                    eqSlot = eSlot;
                    break;
                }
            }
            if (eqSlot != null)
            {
                eqSlot.gameObject.SetActive(true);
                eqSlot.SetItemSlot(weaponSlotItem);
                leftWeaponSlots.Remove(eqSlot);
            }
        }
        foreach (EquipmentWeaponSlot eqSlot in leftWeaponSlots)
        {
            eqSlot.gameObject.SetActive(false);
        }
    }

    private List<EquipmentArmorSlot> FindEquipmentSlots(ArmorSlot slot)
    {
        List<EquipmentArmorSlot> listOfSlots = new List<EquipmentArmorSlot>();
        foreach (EquipmentArmorSlot eqSlot in armorSlots)
        {
            if(eqSlot.armorSlot == slot)
            {
                listOfSlots.Add(eqSlot);
            }
        }
        return listOfSlots;
    }

    private List<EquipmentWeaponSlot> FindEquipmentSlots(WeaponSlot slot)
    {
        List<EquipmentWeaponSlot> listOfSlots = new List<EquipmentWeaponSlot>();
        foreach (EquipmentWeaponSlot eqSlot in weaponSlots)
        {
            if (eqSlot.weaponSlot == slot)
            {
                listOfSlots.Add(eqSlot);
            }
        }
        return listOfSlots;
    }
}
