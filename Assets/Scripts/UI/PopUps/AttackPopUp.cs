using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackPopUp : MonoBehaviour
{
    public CharacterListController charListCtrl;
    public Location.PossibleEnemy enemyToAttack;
    public TextMeshProUGUI wrongSelectedAlert;
    public GameObject activatable;
    private PopUpController popUpPanel;

    private void Start()
    {
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void ActivateThisPopUp(Location loc, Location.PossibleEnemy selEnemy)
    {
        charListCtrl.ChangeLocationFilter(loc);
        enemyToAttack = selEnemy;
        popUpPanel.ActivatePopUp(activatable);
    }

    public void ClosePopUp()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }

    public void ConfirmSelection()
    {
        if (GlobalInput.CheckIfSelectedCharacter())
        {
            CharStats currCharStats = GlobalInput.Instance.selectedEntity.GetComponent<CharStats>();
            if (currCharStats.location == charListCtrl.onlyOnThisLoc
                | charListCtrl.onlyOnThisLoc == null)
            {
                CombatEncounterController.Instance.CreateEncounter(currCharStats, enemyToAttack, charListCtrl.onlyOnThisLoc);
                wrongSelectedAlert.text = "";
                ClosePopUp();
            }
            else
            {
                wrongSelectedAlert.text = "Please select one of the characters shown below!";
            }
        }
        else
            wrongSelectedAlert.text = "Please select one of the characters shown below!";
    }
}
