using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public MapMain mapMain;
    public Location location;

    private void Awake()
    {
        mapMain = gameObject.GetComponentInParent<MapMain>();
    }

    public string GetLocationName()
    {
        return location.locationName;
    }

    public void OnLocationClick()
    {
        if(mapMain.moveActivated)
        {
            //If MOVE button is activated and this node is in possible locations - move the selected character to it
            if(mapMain.GetHighlitedNodes().Contains(gameObject))
            {
                GlobalInput.Instance.selectedEntity.GetComponent<CharStats>().MoveCharacter(location);
                mapMain.OnClickMoveButton();
            }
        }
        else
        {
            DetailedLocationInfo.Instance.OpenDetailedLocationInfo(location);
            //GlobalInput.Instance.setSelectedEntity(location.gameObject);
        }
    }
}
