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

        List<EquipmentArmorSlot> leftArmorSlots = armorSlots;
        foreach(CharEquipment.ArmorSlotItem armorSlotItem in currentCharEquipment.armorSlots)
        {
            EquipmentArmorSlot eqSlot = FindEquipmentSlot(armorSlotItem.slot);
            if(eqSlot != null)
            {
                eqSlot.gameObject.SetActive(true);
                eqSlot.SetItemSlot(armorSlotItem.item);
                leftArmorSlots.Remove(eqSlot);
            }
        }
        foreach (EquipmentArmorSlot eqSlot in leftArmorSlots)
        {
            eqSlot.gameObject.SetActive(false);
        }

        List<EquipmentWeaponSlot> leftWeaponSlots = weaponSlots;
        foreach (CharEquipment.WeaponSlotItem weaponSlotItem in currentCharEquipment.weaponSlots)
        {
            EquipmentWeaponSlot eqSlot = FindEquipmentSlot(weaponSlotItem.slot);
            if (eqSlot != null)
            {
                eqSlot.gameObject.SetActive(true);
                eqSlot.SetItemSlot(weaponSlotItem.item);
                leftWeaponSlots.Remove(eqSlot);
            }
        }
        foreach (EquipmentWeaponSlot eqSlot in leftWeaponSlots)
        {
            eqSlot.gameObject.SetActive(false);
        }
    }

    private EquipmentArmorSlot FindEquipmentSlot(ArmorSlot slot)
    {
        foreach(EquipmentArmorSlot eqSlot in armorSlots)
        {
            if(eqSlot.armorSlot == slot)
            {
                return eqSlot;
            }
        }
        return null;
    }

    private EquipmentWeaponSlot FindEquipmentSlot(WeaponSlot slot)
    {
        foreach (EquipmentWeaponSlot eqSlot in weaponSlots)
        {
            if (eqSlot.weaponSlot == slot)
            {
                return eqSlot;
            }
        }
        return null;
    }
}
