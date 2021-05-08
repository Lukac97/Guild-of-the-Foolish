using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationInfoPanel : MonoBehaviour
{
    [Header("Sections")]
    public GameObject sectionQuests;
    public GameObject sectionEnemies;

    // Start is called before the first frame update
    void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += newLocationInfoPanel;
    }

    public void newLocationInfoPanel()
    {

    }
}
