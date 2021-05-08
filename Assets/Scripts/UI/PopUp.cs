using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PopUp : MonoBehaviour
{
    public PopUpController popUpPanel;
    public GameObject activatable;
    public Button confirmButton;
    public Button cancelButton;
    [Space(3)]
    public TextMeshProUGUI inputTextField;

    private void Start()
    {
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void ClosePopUp()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }

    public void ActivateThisPopUp(UnityAction<string> fctForConfirm, UnityAction fctForCancel)
    {
        if(fctForCancel == null)
        {
            fctForCancel = ClosePopUp;
        }
        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(fctForCancel);
        confirmButton.onClick.AddListener(delegate { fctForConfirm(inputTextField.text); });
        confirmButton.onClick.AddListener(ClosePopUp);

        popUpPanel.ActivatePopUp(activatable);
    }
}
