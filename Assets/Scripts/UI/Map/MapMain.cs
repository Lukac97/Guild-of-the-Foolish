using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public GameObject nodePortraits;
    [SerializeField] private GameObject nodePortraitPrefab;
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

    public float portraitIconSize;

    private void Awake()
    {
        GlobalInput.Instance.onChangedSelectedEntity += UpdateMapMain;
        if (Instance == null)
            _instance = this;

    }

    void Start()
    {
        Vector2 scrollsize = transform.parent.GetComponent<RectTransform>().rect.size;
        portraitIconSize = Mathf.Min(scrollsize.x / 8, scrollsize.y / 8);
        InitNodePortraits();
        CharactersController.NrOfCharsChanged += InitNodePortraits;
    }

    private void Update()
    {
        UpdateNodePortraits();
    }

    private void UpdateNodePortraits()
    {
        List<NodePortraitHandler> portraits = new List<NodePortraitHandler>(nodePortraits.GetComponentsInChildren<NodePortraitHandler>());
        for(int i=0; i<portraits.Count; i++)
        {
            int sameLocCounter = 0;
            for(int j=0; j<i; j++)
            {
                if(portraits[j].connectedCharacter.location == portraits[i].connectedCharacter.location)
                {
                    sameLocCounter += 1;
                }
            }
            portraits[i].UpdateLocation(sameLocCounter);
        }
    }

    private void InitNodePortraits()
    {
        foreach(Transform child in nodePortraits.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(GameObject charGO in CharactersController.Instance.characters)
        {
            GameObject gO = Instantiate(nodePortraitPrefab, nodePortraits.transform);
            gO.GetComponent<NodePortraitHandler>().InitNodePortrait(charGO.GetComponent<CharStats>());
        }
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
