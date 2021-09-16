using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanelController : MonoBehaviour
{

    [SerializeField] private GameObject mapScroll;

    [Header("Options")]
    [SerializeField] private float zoomAmountPerScroll = 0.2f;
    [SerializeField] private float normalZoomMultiplier = 2;
    [SerializeField] private float maxZoomMultiplier = 5;

    private GameObject mapGO;

    private float mapRatio;
    private float currentZoom;
    void Start()
    {
        mapGO = mapScroll.transform.GetChild(0).gameObject;
        SetStartingMapSize();
        NormalizeMapZoom();
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            ZoomMap(zoomAmountPerScroll);
        }
        else if (scroll < 0)
        {
            ZoomMap(-zoomAmountPerScroll);
        }
    }

    private void ZoomMap(float zoomAmount)
    {
        if (currentZoom + zoomAmount > maxZoomMultiplier)
            zoomAmount = maxZoomMultiplier - currentZoom;
        RectTransform mapTransform = mapGO.GetComponent<RectTransform>();
        Vector2 maxZoomedOut = GetMaxZoomedOut();
        Vector2 newSize = new Vector2(maxZoomedOut.x * (currentZoom + zoomAmount),
            maxZoomedOut.y * (currentZoom + zoomAmount));
        if (newSize.x < maxZoomedOut.x | newSize.y < maxZoomedOut.y)
        {
            currentZoom = 1;
            newSize = maxZoomedOut;
        }
        mapTransform.sizeDelta = newSize;
        currentZoom += zoomAmount;
    }

    private void SetStartingMapSize()
    {
        RectTransform mapTransform = mapGO.GetComponent<RectTransform>();
        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
        mapRatio = mapTransform.rect.width / mapTransform.rect.height;
        Vector2 newSizeDelta = GetMaxZoomedOut();
        mapTransform.sizeDelta = newSizeDelta;
        currentZoom = 1;
    }

    private Vector2 GetMaxZoomedOut()
    {
        RectTransform mapTransform = mapGO.GetComponent<RectTransform>();
        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
        Vector2 newSizeDelta = new Vector2();
        if (canvasScaler.referenceResolution.y * mapRatio > canvasScaler.referenceResolution.x)
        {
            newSizeDelta.y = canvasScaler.referenceResolution.y;
            newSizeDelta.x = newSizeDelta.y * mapRatio;
        }
        else
        {
            newSizeDelta.x = canvasScaler.referenceResolution.x;
            newSizeDelta.y = newSizeDelta.x / mapRatio;
        }
        return newSizeDelta;
    }

    private void NormalizeMapZoom()
    {
        RectTransform mapTransform = mapGO.GetComponent<RectTransform>();
        //float widthToHeightRatio = mapTransform.rect.width / mapTransform.rect.height;
        //float newWidth = mapTransform.rect.width / normalZoomMultiplier;
        //float newHeight = newWidth / widthToHeightRatio;
        mapTransform.sizeDelta = new Vector2(mapTransform.sizeDelta.x*normalZoomMultiplier, mapTransform.sizeDelta.y*normalZoomMultiplier);
        currentZoom = normalZoomMultiplier;
    }
}
