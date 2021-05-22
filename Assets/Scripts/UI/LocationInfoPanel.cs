using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationInfoPanel : MonoBehaviour
{
    public GameObject enemyOnLocationUIPrefab;
    [Header("Sections")]
    public GameObject sectionQuests;
    public GameObject sectionEnemies;

    private Location currentLocation;
    // Start is called before the first frame update
    void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += newLocationInfoPanel;
        newLocationInfoPanel();
    }

    public void newLocationInfoPanel()
    {
        if(GlobalInput.Instance.CheckIfSelectedLocation())
        {
            if (currentLocation == GlobalInput.Instance.selectedEntity.GetComponent<Location>())
                return;
            foreach (EnemyOnLocation eol in GetComponentsInChildren<EnemyOnLocation>())
            {
                Destroy(eol.gameObject);
            }
            currentLocation = GlobalInput.Instance.selectedEntity.GetComponent<Location>();
            foreach (Location.PossibleEnemy posEn in currentLocation.possibleEnemies)
            {
                GameObject gO = Instantiate(enemyOnLocationUIPrefab, sectionEnemies.transform);
                gO.GetComponent<EnemyOnLocation>().InitEnemyOnLocation(posEn);
            }
        }
        else
        {
            currentLocation = null;
            foreach(EnemyOnLocation eol in GetComponentsInChildren<EnemyOnLocation>())
            {
                Destroy(eol.gameObject);
            }
        }
    }
}
