using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMain : MonoBehaviour
{

    public GameObject nodes;
    public World world;
    public GameObject moveButtonPanel;

    [Space(10)]
    public Color characterLocationColor;
    public Color possibleMovementColor;
    public Color selectedLocationColor;

    private Color currentlyUsedColor;

    private List<GameObject> lastHighlitedNodes = new List<GameObject>();
    [HideInInspector]
    public bool moveActivated = false;
    void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += HighlightSelectedLocation;
        GlobalInput.Instance.changeSelectedEntity += HighlightLocationOfSelectedChar;
        GlobalInput.Instance.changeSelectedEntity += EnableMoveButton;

        EnableMoveButton();
    }

    void Update()
    {
        
    }

    private void HighlightLocations()
    {

        foreach(GameObject highlightNode in lastHighlitedNodes)
        {
            Outline outline = highlightNode.GetComponent<Outline>();
            outline.effectColor = currentlyUsedColor;
            outline.enabled = true;
        }
    }

    private void DeHighlightAllLocations()
    {
        foreach (GameObject lastHighlightedNode in lastHighlitedNodes)
        {
            lastHighlightedNode.GetComponent<Outline>().enabled = false;
        }
        lastHighlitedNodes.Clear();
    }

    public void HighlightLocationOfSelectedChar()
    {
        if (!GlobalInput.Instance.CheckIfSelectedCharacter())
            return;
        currentlyUsedColor = characterLocationColor;
        DeHighlightAllLocations();
        GameObject selectedChar = GlobalInput.Instance.selectedEntity;
        GameObject highlightNode = selectedChar.GetComponent<CharStats>().location.connectedNode.gameObject;
        lastHighlitedNodes.Add(highlightNode);
        HighlightLocations();
    }

    public void HighlightConnectedLocations(Location loc)
    {
        currentlyUsedColor = possibleMovementColor;
        DeHighlightAllLocations();
        List<Location> locs = world.GetConnections(loc);
        foreach(Location iloc in locs)
        {
            lastHighlitedNodes.Add(iloc.connectedNode.gameObject);
        }
        HighlightLocations();
    }

    public void HighlightSelectedLocation()
    {
        if (!GlobalInput.Instance.CheckIfSelectedLocation())
            return;
        currentlyUsedColor = selectedLocationColor;
        DeHighlightAllLocations();
        lastHighlitedNodes.Add(GlobalInput.Instance.selectedEntity.GetComponent<Location>().connectedNode.gameObject);
        HighlightLocations();
    }

    public List<GameObject> GetHighlitedNodes()
    {
        return lastHighlitedNodes;
    }

    public void OnClickMoveButton()
    {
        if(moveActivated)
        {
            moveActivated = false;
            HighlightLocationOfSelectedChar();
        }
        else
        {
            moveActivated = true;
            HighlightConnectedLocations(GlobalInput.Instance.selectedEntity.GetComponent<CharStats>().location);
        }
    }

    public void EnableMoveButton()
    {
        if(GlobalInput.Instance.CheckIfSelectedCharacter())
        {
            moveButtonPanel.SetActive(true);
        }
        else
        {
            moveButtonPanel.SetActive(false);
        }
    }

    public MapNode FindMapNodeByLocation(Location loc)
    {
        foreach(Transform child in nodes.transform)
        {
            MapNode potentialNode = child.GetComponent<MapNode>();
            if(potentialNode.location == loc)
            {
                return potentialNode;
            }
        }
        return null;
    }
}
