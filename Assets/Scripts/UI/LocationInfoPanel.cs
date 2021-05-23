using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationInfoPanel : MonoBehaviour
{
    public GameObject enemyOnLocationUIPrefab;
    public GameObject container;
    [Header("Sections")]
    public GameObject sectionQuests;
    public GameObject sectionEnemies;
    [Header("Text")]
    public TextMeshProUGUI locationName;

    private Location currentLocation;
    // Start is called before the first frame update
    void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += newLocationInfoPanel;
        newLocationInfoPanel();
    }

    public void newLocationInfoPanel()
    {
        CanvasGroup cg = container.GetComponent<CanvasGroup>();
        if (GlobalInput.Instance.CheckIfSelectedLocation())
        {
            if (currentLocation == GlobalInput.Instance.selectedEntity.GetComponent<Location>())
                return;
            foreach (EnemyOnLocation eol in GetComponentsInChildren<EnemyOnLocation>())
            {
                Destroy(eol.gameObject);
            }
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;

            currentLocation = GlobalInput.Instance.selectedEntity.GetComponent<Location>();
            foreach (Location.PossibleEnemy posEn in currentLocation.possibleEnemies)
            {
                GameObject gO = Instantiate(enemyOnLocationUIPrefab, sectionEnemies.transform);
                gO.GetComponent<EnemyOnLocation>().InitEnemyOnLocation(posEn);
            }
            locationName.text = currentLocation.locationName;
        }
        else
        {
            currentLocation = null;
            foreach(EnemyOnLocation eol in GetComponentsInChildren<EnemyOnLocation>())
            {
                Destroy(eol.gameObject);
            }
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
}
