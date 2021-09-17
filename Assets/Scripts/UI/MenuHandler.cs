using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [System.Serializable]
    public class SingularMenu
    {
        public CanvasGroup menuCanvasGroup;
        public bool rightMenu;
        public bool isActive;
    }

    public SingularMenu inventoryMenu;
    public SingularMenu equipmentMenu;
    public SingularMenu spellMenu;

    private void Start()
    {
        CloseAllRightOrLeftMenus(true);
        CloseAllRightOrLeftMenus(false);
    }

    public void HandleInventoryMenu(bool forceOpen = false)
    {
        if (forceOpen | !inventoryMenu.isActive)
        {
            CloseAllRightOrLeftMenus(inventoryMenu.rightMenu);
            GlobalFuncs.SetActiveCanvasGroup(inventoryMenu.menuCanvasGroup, true);
            inventoryMenu.isActive = true;
        }
        else
        {
            GlobalFuncs.SetActiveCanvasGroup(inventoryMenu.menuCanvasGroup, false);
            inventoryMenu.isActive = false;
        }
    }

    public void HandleEquipmentMenu(bool forceOpen = false)
    {
        if (forceOpen | !equipmentMenu.isActive)
        {
            CloseAllRightOrLeftMenus(equipmentMenu.rightMenu);
            HandleInventoryMenu(forceOpen: true);
            GlobalFuncs.SetActiveCanvasGroup(equipmentMenu.menuCanvasGroup, true);
            equipmentMenu.isActive = true;
        }
        else
        {
            GlobalFuncs.SetActiveCanvasGroup(equipmentMenu.menuCanvasGroup, false);
            equipmentMenu.isActive = false;
        }
    }

    public void HandleSpellMenu(bool forceOpen = false)
    {
        if (forceOpen | !spellMenu.isActive)
        {
            CloseAllRightOrLeftMenus(spellMenu.rightMenu);
            GlobalFuncs.SetActiveCanvasGroup(spellMenu.menuCanvasGroup, true);
            spellMenu.isActive = true;
        }
        else
        {
            GlobalFuncs.SetActiveCanvasGroup(spellMenu.menuCanvasGroup, false);
            spellMenu.isActive = false;
        }
    }

    private void CloseAllRightOrLeftMenus(bool closeRight)
    {
        if (inventoryMenu.rightMenu == closeRight)
        {
            GlobalFuncs.SetActiveCanvasGroup(inventoryMenu.menuCanvasGroup, false);
            inventoryMenu.isActive = false;
        }
        if (equipmentMenu.rightMenu == closeRight)
        {
            GlobalFuncs.SetActiveCanvasGroup(equipmentMenu.menuCanvasGroup, false);
            equipmentMenu.isActive = false;
        }
        if (spellMenu.rightMenu == closeRight)
        {
            GlobalFuncs.SetActiveCanvasGroup(spellMenu.menuCanvasGroup, false);
            spellMenu.isActive = false;
        }
    }
}
