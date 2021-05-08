using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitListElement : MonoBehaviour
{
    private RecruitList recruitList;
    private CharClass thisClass;
    private int purchasePrice;
    public TextMeshProUGUI className;
    public TextMeshProUGUI price;

    void Start()
    {
        recruitList = GetComponentInParent<RecruitList>();
    }

    public void InitRecruitText(CharClass charClass, int _price)
    {
        className.text = charClass.className;
        price.text = _price.ToString();
        thisClass = charClass;
        purchasePrice = _price;
    }

    public void OnClickRecruit()
    {
        recruitList.chooseNamePopUp.ActivateThisPopUp(OnConfirmName, null);
    }

    public void OnConfirmName(string passedName)
    {
        CharactersController.Instance.CreateNewCharater(thisClass, passedName, purchasePrice);
    }
}
