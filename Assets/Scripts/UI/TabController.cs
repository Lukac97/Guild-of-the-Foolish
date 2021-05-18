using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [Space(4)]
    public List<GameObject> panels;
    public List<GameObject> tabs;
    [HideInInspector]
    public GameObject activePanel;
    [HideInInspector]
    public GameObject activeTab;

    [Space(6)]
    public Color activeTabColor;
    public Color inactiveTabColor;
    void Awake()
    {
        activePanel = panels[0];
        activePanel.GetComponent<CanvasGroup>().alpha = 1;
        activePanel.GetComponent<CanvasGroup>().interactable = true;
        activePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        activeTab = tabs[0];
        activeTab.GetComponent<Image>().color = activeTabColor;
        for(int i=0; i < panels.Count; i++)
        {
            GameObject panel = panels[i];
            GameObject tab = tabs[i];
            if(panel != activePanel)
            {
                panel.GetComponent<CanvasGroup>().alpha = 0;
                panel.GetComponent<CanvasGroup>().interactable = false;
                panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                tab.GetComponent<Image>().color = inactiveTabColor;
            }
        }
    }

    public void ChangeTab(GameObject newTab)
    {
        GameObject panel = panels[tabs.IndexOf(newTab)];
        if(panel == activePanel)
        {
            return;
        }
        activePanel = panel;
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().interactable = true;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        activeTab = tabs[panels.IndexOf(panel)];
        activeTab.GetComponent<Image>().color = activeTabColor;
        for (int i = 0; i < panels.Count; i++)
        {
            GameObject panelle = panels[i];
            GameObject tab = tabs[i];
            if (panelle != activePanel)
            {
                panelle.GetComponent<CanvasGroup>().alpha = 0;
                panelle.GetComponent<CanvasGroup>().interactable = false;
                panelle.GetComponent<CanvasGroup>().blocksRaycasts = false;
                tab.GetComponent<Image>().color = inactiveTabColor;
            }
        }
    }
}
