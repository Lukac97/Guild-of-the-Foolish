using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    public Vector3 defaultPosition;
    [Space(4)]
    public List<GameObject> panels;
    public GameObject activePanel;
    void Awake()
    {
        activePanel = panels[0];
        activePanel.transform.position = defaultPosition;
        foreach(GameObject panel in panels)
        {
            if(panel != activePanel)
            {
                panel.transform.position = new Vector3(defaultPosition.x - 100,
                    defaultPosition.y - 100, defaultPosition.z);
            }
        }
    }

    public void ChangeTab(GameObject panel)
    {
        if(panel == activePanel)
        {
            return;
        }
        activePanel = panel;

        activePanel.transform.position = defaultPosition;
        foreach (GameObject panelle in panels)
        {
            if (panelle != activePanel)
            {
                panelle.transform.position = new Vector3(defaultPosition.x - 100,
                    defaultPosition.y - 100, defaultPosition.z);
            }
        }
    }
}
