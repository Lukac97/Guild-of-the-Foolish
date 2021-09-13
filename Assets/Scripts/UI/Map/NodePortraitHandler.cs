using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodePortraitHandler : MonoBehaviour
{
    public CharStats connectedCharacter;

    [SerializeField] private Image charIcon;
    [SerializeField] private RectTransform boundingRectTransform;

    private MapMain mapMain;
    private RectTransform scrollRect;
    private Vector3[] scrollRectPoints;

    private bool isOutOfRange;
    private float artiMapNodeY;
    private void Awake()
    {
        mapMain = gameObject.GetComponentInParent<MapMain>();
    }

    public void InitNodePortrait(CharStats charStats)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(mapMain.portraitIconSize, mapMain.portraitIconSize);
        connectedCharacter = charStats;
        charIcon.sprite = charStats.characterClass.classIcon;
        isOutOfRange = true;
        scrollRectPoints = new Vector3[4];
        scrollRect = mapMain.transform.parent.GetComponent<RectTransform>();
        scrollRect.GetWorldCorners(scrollRectPoints);
    }

    public void OnClickPortrait()
    {
        GlobalInput.Instance.setSelectedEntity(connectedCharacter.gameObject);
    }

    public void UpdateLocation(int spot)
    {
        MapNode mapNode = connectedCharacter.location.connectedNode;
        Vector3 mapNodeLocalPos = connectedCharacter.location.connectedNode.GetComponent<RectTransform>().localPosition;
        artiMapNodeY = mapMain.transform.TransformPoint(
                new Vector3(mapNodeLocalPos.x, mapNodeLocalPos.y + mapMain.portraitIconSize / 2, mapNodeLocalPos.z)).y;
        if (CheckIfNodeInRange())
        {
            if (isOutOfRange)
            {
                isOutOfRange = false;
                GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                transform.localPosition = new Vector3(mapNode.transform.localPosition.x,
                    mapNode.transform.localPosition.y + mapNode.GetComponent<RectTransform>().sizeDelta.y / 2 + (1 + 2 * spot) * GetComponent<RectTransform>().sizeDelta.y / 2,
                    mapNode.transform.localPosition.z);
            }
        }
        else
        {
            isOutOfRange = true;
            //if (lastPosition == null || lastPosition != transform.position)
            //{
            //    Vector2? newPos = new Vector2(GetIntersectionPointWithRect(scrollRect.position, mapNode.transform.position);
            //    if (newPos != null)
            //        transform.position = (Vector3)newPos;
            //    isOutOfRange = true;
            //    lastPosition = transform.position;
            //}

            Vector3 newPosition = new Vector3();
            float pivotX = 0;
            float pivotY = 0;

            if (mapNode.transform.position.x > scrollRectPoints[2].x)
            {
                newPosition.x = scrollRectPoints[2].x;
                pivotX = 1;
            }
            else if (mapNode.transform.position.x < scrollRectPoints[1].x)
            {
                newPosition.x = scrollRectPoints[1].x;
                pivotX = 0;
            }
            else
            {
                newPosition.x = mapNode.transform.position.x;
                pivotX = 0.5f;
            }

            if (mapNode.transform.position.y > scrollRectPoints[1].y)
            {
                newPosition.y = scrollRectPoints[1].y;
                pivotY = 1;
            }
            else if (mapNode.transform.position.y < scrollRectPoints[0].y)
            {
                newPosition.y = scrollRectPoints[0].y;
                pivotY = 0;
            }
            else
            {
                newPosition.y = mapNode.transform.position.y;
                pivotY = 0.5f;
            }

            transform.position = newPosition;
            GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
        }
    }

    private bool CheckIfNodeInRange()
    {
        Vector3 mapNodePos = connectedCharacter.location.connectedNode.GetComponent<RectTransform>().position;
        Vector3[] rectPoints = new Vector3[4];

        float minX = Mathf.Min(scrollRectPoints[0].x, scrollRectPoints[1].x, scrollRectPoints[2].x, scrollRectPoints[3].x);
        float maxX = Mathf.Max(scrollRectPoints[0].x, scrollRectPoints[1].x, scrollRectPoints[2].x, scrollRectPoints[3].x);
        float minY = Mathf.Min(scrollRectPoints[0].y, scrollRectPoints[1].y, scrollRectPoints[2].y, scrollRectPoints[3].y);
        float maxY = Mathf.Max(scrollRectPoints[0].y, scrollRectPoints[1].y, scrollRectPoints[2].y, scrollRectPoints[3].y);

        if (mapNodePos.x < minX | mapNodePos.x > maxX |
            mapNodePos.y < minY |
            mapNodePos.y > maxY)
        {
            return false;
        }
        return true;
    }

    //private Vector2? GetIntersectionPointWithRect(Vector2 ps1, Vector2 pe1)
    //{
    //    Vector2? intersectionPoint = null;
    //    intersectionPoint = GetIntersectionPoint(ps1, pe1, scrollRectPoints[0], scrollRectPoints[1]);
    //    if (intersectionPoint == null)
    //    {
    //        intersectionPoint = GetIntersectionPoint(ps1, pe1, scrollRectPoints[1], scrollRectPoints[2]);
    //    }
    //    if (intersectionPoint == null)
    //    {
    //        intersectionPoint = GetIntersectionPoint(ps1, pe1, scrollRectPoints[2], scrollRectPoints[3]);
    //    }
    //    if (intersectionPoint == null)
    //    {
    //        intersectionPoint = GetIntersectionPoint(ps1, pe1, scrollRectPoints[3], scrollRectPoints[0]);
    //    }
    //    return intersectionPoint;
    //}

    ////ps2 and pe2 are points defining an edge of rectangle
    //private Vector2? GetIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    //{
    //    //Get A and B of first point
    //    float A1 = pe1.y - ps1.y;
    //    float B1 = ps1.x - pe1.x;

    //    //Get A and B of second point
    //    float A2 = pe2.y - ps2.y;
    //    float B2 = ps2.x - pe2.x;

    //    //Parallel check
    //    float delta = A1 * B2 - A2 * B1;
    //    if (delta == 0)
    //        return null;

    //    //Get C of first and second line
    //    float C2 = A2 * ps2.x + B2 * ps2.y;
    //    float C1 = A1 * ps1.x + B1 * ps1.y;

    //    float invDelta = 1 / delta;
    //    Vector2 lineIntersectionPoint = new Vector2((B2 * C1 - B1 * C2) * invDelta, (A1 * C2 - A2 * C1) * invDelta);

    //    bool liesOnLine = true;

    //    //float lenTotal = Mathf.Sqrt((pe2.x - ps2.x) * (pe2.x - ps2.x) + (pe2.y - ps2.y) * (pe2.y - ps2.y));
    //    //float lenFromStart = Mathf.Sqrt((lineIntersectionPoint.x - ps2.x) * (lineIntersectionPoint.x - ps2.x)
    //    //    + (lineIntersectionPoint.y - ps2.y) * (lineIntersectionPoint.y - ps2.y));
    //    //float lenFromEnd = Mathf.Sqrt((pe2.x - lineIntersectionPoint.x) * (pe2.x - lineIntersectionPoint.x)
    //    //    + (pe2.y - lineIntersectionPoint.y) * (pe2.y - lineIntersectionPoint.y));

    //    if(pe2.x == ps2.x)
    //    {
    //        if (lineIntersectionPoint.x > Mathf.Max(pe2.x, ps2.x) | lineIntersectionPoint.x < Mathf.Min(pe2.x, ps2.x))
    //            liesOnLine = false;
    //    }
    //    else
    //    {
    //        if (lineIntersectionPoint.y > Mathf.Max(pe2.y, ps2.y) | lineIntersectionPoint.y < Mathf.Min(pe2.y, ps2.y))
    //            liesOnLine = false;
    //    }

    //    //Debug.Log(Mathf.Abs(lenTotal - lenFromStart + lenFromEnd));
    //    //if (Mathf.Abs(lenTotal - lenFromStart + lenFromEnd) < 0.1)
    //    //    liesOnLine = true;

    //    ////line is "more" horizontal
    //    //if (Mathf.Abs(B2) >= Mathf.Abs(A2))
    //    //{
    //    //    liesOnLine = B2 > 0 ? ps2.x <= lineIntersectionPoint.x && lineIntersectionPoint.x <= pe2.x :
    //    //        pe2.x <= lineIntersectionPoint.x && lineIntersectionPoint.x <= ps2.x;
    //    //}
    //    //else
    //    //{
    //    //    liesOnLine = A2 > 0 ? ps2.y <= lineIntersectionPoint.y && lineIntersectionPoint.y <= pe2.y :
    //    //        pe2.y <= lineIntersectionPoint.y && lineIntersectionPoint.y <= ps2.y;
    //    //}

    //    if (liesOnLine)
    //    {
    //        return lineIntersectionPoint;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
}
