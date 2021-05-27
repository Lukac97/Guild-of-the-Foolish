using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Text.RegularExpressions;

public class PopUp : MonoBehaviour
{
    public PopUpController popUpPanel;
    public GameObject activatable;
    public Button confirmButton;
    public Button cancelButton;
    [Space(3)]
    public TextMeshProUGUI alert;
    public TextMeshProUGUI inputTextField;

    public UnityAction<string> fctAfterConfirm;
    public UnityAction fctAfterCancel;
    private void Start()
    {
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void ClosePopUp()
    {
        if (fctAfterCancel != null)
            fctAfterCancel.Invoke();
        popUpPanel.DeactivatePopUp(activatable);
    }

    public void ConfirmPopUp()
    {
        if (fctAfterConfirm != null)
        {
            Regex regEmpty = new Regex(@"^[\s\u200B]+$");
            if (regEmpty.IsMatch(inputTextField.text) | inputTextField.text == "")
            {
                alert.text = "Please enter a name!";
            }
            else
            {
                fctAfterConfirm.Invoke(inputTextField.text);
                alert.text = "";
                ClosePopUp();
            }
        }
    }

    public void ActivateThisPopUp(UnityAction<string> fctForConfirm, UnityAction fctForCancel)
    {
        //cancelButton.onClick.RemoveAllListeners();
        //confirmButton.onClick.RemoveAllListeners();
        //cancelButton.onClick.AddListener(fctForCancel);
        fctAfterCancel = null;
        fctAfterConfirm = null;
        if(fctForCancel != null)
            fctAfterCancel += fctForCancel; 
        if(fctForConfirm != null)
            fctAfterConfirm += fctForConfirm;
        //confirmButton.onClick.AddListener(delegate { fctForConfirm(inputTextField.text); });
        //confirmButton.onClick.AddListener(ClosePopUp);

        popUpPanel.ActivatePopUp(activatable);
    }
}
