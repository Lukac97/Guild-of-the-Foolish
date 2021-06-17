using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenController : MonoBehaviour
{
    private static UIScreenController _instance;
    public static UIScreenController Instance
    {
        get
        {
            return _instance;
        }
    }

    public CanvasGroup mainScreen;
    public CanvasGroup combatScreen;

    private void Awake()
    {
        if(Instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        //activate main screen first
        ActivateMainScreen();
    }

    public void ActivateCombatScreen()
    {
        DeactivateCanvasGroup(mainScreen);
        ActivateCanvasGroup(combatScreen);
    }

    public void ActivateMainScreen()
    {
        DeactivateCanvasGroup(combatScreen);
        ActivateCanvasGroup(mainScreen);
    }

    private void DeactivateCanvasGroup(CanvasGroup cvGrp)
    {
        cvGrp.alpha = 0;
        cvGrp.interactable = false;
        cvGrp.blocksRaycasts = false;
    }

    private void ActivateCanvasGroup(CanvasGroup cvGrp)
    {
        cvGrp.alpha = 1;
        cvGrp.interactable = true;
        cvGrp.blocksRaycasts = true;
    }
}
