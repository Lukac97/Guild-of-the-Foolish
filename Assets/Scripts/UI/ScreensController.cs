using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensController : MonoBehaviour
{
    private static ScreensController _instance;
    public static ScreensController Instance
    {
        get
        {
            return _instance;
        }
    }

    public CanvasGroup combatScreen;
    public CanvasGroup mainScreen;

    private void Start()
    {
        if (Instance == null)
            _instance = this;
        ActivateCombatScreen(false);
    }

    public void ActivateCombatScreen(bool activate)
    {
        combatScreen.alpha = activate ? 1 : 0;
        combatScreen.interactable = activate;
        combatScreen.blocksRaycasts = activate;
        
        mainScreen.alpha = activate ? 0 : 1;
        mainScreen.interactable = !activate;
        mainScreen.blocksRaycasts = !activate;
    }
}
