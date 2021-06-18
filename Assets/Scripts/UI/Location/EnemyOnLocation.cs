using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyOnLocation : MonoBehaviour
{
    public bool forDetailedInfo;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyLevel;
    public Location.PossibleEnemy enemyMould;

    public string levelPrefix = "";

    private LocationInfoPanel locationInfoPanel;
    private DetailedLocationInfo detailedLocationInfo;
    private void Start()
    {
        if(forDetailedInfo)
            detailedLocationInfo = GetComponentInParent<DetailedLocationInfo>();
        else
            locationInfoPanel = GetComponentInParent<LocationInfoPanel>();
    }

    public void InitEnemyOnLocation(Location.PossibleEnemy psEn)
    {
        enemyName.text = psEn.enemyMould.name;
        enemyLevel.text = levelPrefix + (levelPrefix == "" ? "" : " ") + psEn.minLevel.ToString() + " - " + psEn.maxLevel.ToString();
        enemyMould = psEn;
    }

    public void OnClickAttack()
    {
        if (forDetailedInfo)
        {
            AttackPopUp.Instance.ActivateThisPopUp(detailedLocationInfo.currentLocation, enemyMould, true);
        }
        else
        {
            if (GlobalInput.CheckIfSelectedLocation())
                AttackPopUp.Instance.ActivateThisPopUp(locationInfoPanel.currentLocation, enemyMould, true);
        }
    }

    public void OnClickHunt()
    {
        if (forDetailedInfo)
        {
            AttackPopUp.Instance.ActivateThisPopUp(detailedLocationInfo.currentLocation, enemyMould, false);
        }
        else
        {
            if (GlobalInput.CheckIfSelectedLocation())
                AttackPopUp.Instance.ActivateThisPopUp(locationInfoPanel.currentLocation, enemyMould, false);
        }
    }
}
