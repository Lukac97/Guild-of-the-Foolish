using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanelController : MonoBehaviour
{

    [SerializeField] private GameObject mapScroll;

    [Header("Options")]
    [SerializeField] private float normalZoomMultiplier = 2;

    private GameObject mapGO;

    void Start()
    {
        mapGO = mapScroll.transform.GetChild(0).gameObject;
        NormalizeMapZoom();
    }

    private void NormalizeMapZoom()
    {
        RectTransform mapTransform = mapGO.GetComponent<RectTransform>();
        //float widthToHeightRatio = mapTransform.rect.width / mapTransform.rect.height;
        //float newWidth = mapTransform.rect.width / normalZoomMultiplier;
        //float newHeight = newWidth / widthToHeightRatio;
        mapTransform.localScale = new Vector3(1 / normalZoomMultiplier, 1 / normalZoomMultiplier, 1 / normalZoomMultiplier);
    }
}
