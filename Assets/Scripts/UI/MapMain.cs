using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMain : MonoBehaviour
{
    private static MapMain _instance;
    public static MapMain Instance
    {
        get
        {
            return _instance;
        }
    }

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

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    void Start()
    {
        GlobalInput.Instance.onChangedSelectedEntity += UpdateMapMain;

        EnableMoveButton();
    }


    public void UpdateMapMain()
    {
        if(GlobalInput.CheckIfSelectedCharacter())
        {
            HighlightLocationOfSelectedChar();
        }
        else if(GlobalInput.CheckIfSelectedLocation())
        {
            HighlightSelectedLocation();
        }
        EnableMoveButton();
    }

    public void HighlightLocationOfSelectedChar()
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
            return;
        currentlyUsedColor = characterLocationColor;
        DeHighlightAllLocations();
        GameObject selectedChar = GlobalInput.Instance.selectedEntity;
        GameObject highlightNode = selectedChar.GetComponent<CharStats>().location.connectedNode.gameObject;
        lastHighlitedNodes.Add(highlightNode);
        HighlightLocations();
    }

    public void HighlightSelectedLocation()
    {
        if (!GlobalInput.CheckIfSelectedLocation())
            return;
        currentlyUsedColor = selectedLocationColor;
        DeHighlightAllLocations();
        lastHighlitedNodes.Add(GlobalInput.Instance.selectedEntity.GetComponent<Location>().connectedNode.gameObject);
        HighlightLocations();
    }

    public void EnableMoveButton()
    {
        if (GlobalInput.CheckIfSelectedCharacter())
        {
            moveButtonPanel.SetActive(true);
        }
        else
        {
            moveButtonPanel.SetActive(false);
        }
    }

    //-------------------------------------------------------------------------------------

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
