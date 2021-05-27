using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyOnLocation : MonoBehaviour
{
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyLevel;
    public Location.PossibleEnemy enemyMould;

    private LocationInfoPanel locationInfoPanel;
    private void Start()
    {
        locationInfoPanel = GetComponentInParent<LocationInfoPanel>();
    }

    public void InitEnemyOnLocation(Location.PossibleEnemy psEn)
    {
        enemyName.text = psEn.enemyMould.name;
        enemyLevel.text = psEn.minLevel.ToString() + " - " + psEn.maxLevel.ToString();
        enemyMould = psEn;
    }

    public void OnClickAttack()
    {
        if(GlobalInput.CheckIfSelectedLocation())
            locationInfoPanel.attackPopUp.ActivateThisPopUp(GlobalInput.Instance.selectedEntity.GetComponent<Location>(), enemyMould);
    }
}
