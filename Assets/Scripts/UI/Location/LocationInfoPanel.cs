using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationInfoPanel : MonoBehaviour
{
    [Header("Container - for small info only")]
    public GameObject enemyOnLocationUIPrefab;
    public CanvasGroup canvasGroup;
    [Header("Sections")]
    public GameObject sectionQuests;
    public GameObject sectionEnemies;
    [Header("Text")]
    public TextMeshProUGUI locationName;

    public Location currentLocation;
    // Start is called before the first frame update
    void Start()
    {
        GlobalInput.Instance.onChangedSelectedEntity += newLocationInfoPanel;
        newLocationInfoPanel();
    }

    public void newLocationInfoPanel()
    {
        if (GlobalInput.CheckIfSelectedLocation())
        {
            if (currentLocation == GlobalInput.Instance.selectedEntity.GetComponent<Location>())
                return;
            foreach (EnemyOnLocation eol in GetComponentsInChildren<EnemyOnLocation>())
            {
                Destroy(eol.gameObject);
            }

            CanvasGroupSetActive(true);

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
            CanvasGroupSetActive(false);
        }
    }

    public void OnClickMoreInfo()
    {
        DetailedLocationInfo.Instance.OpenDetailedLocationInfo(currentLocation);
    }

    private void CanvasGroupSetActive(bool setActive)
    {
        canvasGroup.alpha = setActive ? 1 : 0;
        canvasGroup.interactable = setActive;
        canvasGroup.blocksRaycasts = setActive;
    }
}
