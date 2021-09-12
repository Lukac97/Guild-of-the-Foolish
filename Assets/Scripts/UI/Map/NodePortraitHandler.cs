using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodePortraitHandler : MonoBehaviour
{
    public CharStats connectedCharacter;

    [SerializeField] private Image charIcon;

    private MapMain mapMain;
    private void Awake()
    {
        mapMain = gameObject.GetComponentInParent<MapMain>();
    }

    public void InitNodePortrait(CharStats charStats)
    {
        Vector2 scrollsize = mapMain.transform.parent.GetComponent<RectTransform>().rect.size;
        float sizeToSet = Mathf.Min(scrollsize.x / 8, scrollsize.y / 8);
        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeToSet, sizeToSet);
        connectedCharacter = charStats;
        charIcon.sprite = charStats.characterClass.classIcon;
    }

    public void OnClickPortrait()
    {
        GlobalInput.Instance.setSelectedEntity(connectedCharacter.gameObject);
    }

    public void UpdateLocation(int spot)
    {
        MapNode mapNode = connectedCharacter.location.connectedNode;
        transform.localPosition = new Vector3(mapNode.transform.localPosition.x,
            mapNode.transform.localPosition.y + mapNode.GetComponent<RectTransform>().sizeDelta.y / 2 + (1 + 2 * spot) * GetComponent<RectTransform>().sizeDelta.y / 2,
            mapNode.transform.localPosition.z);
    }

    private bool CheckIfNodeInRange()
    {
        Vector3 mapNodePos = connectedCharacter.location.connectedNode.GetComponent<RectTransform>().anchoredPosition;
        RectTransform scrollTransform = mapMain.transform.parent.GetComponent<RectTransform>();

        if (scrollTransform.rect.Contains(mapNodePos))
        {
            return true;
        }
        return false;
    }
}
