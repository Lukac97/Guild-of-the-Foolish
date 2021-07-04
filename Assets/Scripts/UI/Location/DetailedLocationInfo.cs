using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailedLocationInfo : MonoBehaviour
{
    private static DetailedLocationInfo _instance;
    public static DetailedLocationInfo Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject activatable;
    public GameObject enemyOnLocationUIPrefab;

    [Header("Sections")]
    public GameObject sectionQuests;
    public GameObject sectionEnemies;
    public GameObject sectionNPCs;
    [Header("UI")]
    public TextMeshProUGUI locationName;

    public Location currentLocation;
    private PopUpController popUpPanel;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    private void Start()
    {
    }

    public void OpenDetailedLocationInfo(Location locationToOpen)
    {
        if (locationToOpen == null)
            return;
        currentLocation = locationToOpen;
        UpdateDetailedLocationContent();
        popUpPanel.ActivatePopUp(activatable);
    }

    private void UpdateDetailedLocationContent()
    {
        locationName.text = currentLocation.locationName;

        foreach (Transform child in sectionEnemies.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Location.PossibleEnemy posEn in currentLocation.possibleEnemies)
        {
            GameObject gO = Instantiate(enemyOnLocationUIPrefab, sectionEnemies.transform);
            gO.GetComponent<EnemyOnLocation>().InitEnemyOnLocation(posEn);
        }
    }

    public void CloseDetailedLocationInfo()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }
}
