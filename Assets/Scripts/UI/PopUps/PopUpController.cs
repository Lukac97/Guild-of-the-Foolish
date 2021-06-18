using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopUpController : MonoBehaviour
{
    private static PopUpController _instance;
    public static PopUpController Instance
    {
        get
        {
            return _instance;
        }
    }
    public GameObject background;
    [Range(0.0f, 100.0f)]
    public float shadeLevel;
    public List<GameObject> allPopUps;

    private List<GameObject> activatedPopUps;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    private void Start()
    {
        activatedPopUps = new List<GameObject>();
        allPopUps = new List<GameObject>();
        foreach (Transform child in gameObject.transform)
        {
            allPopUps.Add(child.gameObject);
        }
        DeactivateAllPopUps();
    }

    private void SetShaderValue(float shaderVal)
    {
        Image image = background.GetComponent<Image>();
        Color currentColor = image.color;
        if (shaderVal == 0)
            image.raycastTarget = false;
        else
            image.raycastTarget = true;
        currentColor.a = shaderVal / 100;
        background.GetComponent<Image>().color = currentColor;
    }

    public void ActivatePopUp(GameObject popUpActivatable)
    {
        if(activatedPopUps.Count == 0)
        {
            SetShaderValue(shadeLevel);
        }
        popUpActivatable.SetActive(true);
        activatedPopUps.Add(popUpActivatable);
        background.GetComponent<RectTransform>().SetAsLastSibling();
        popUpActivatable.transform.parent.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void DeactivatePopUp(GameObject popUp)
    {
        if(activatedPopUps.Count == 0)
        {
            return;
        }
        popUp.SetActive(false);
        activatedPopUps.Remove(popUp);
        if(activatedPopUps.Count == 0)
        {
            SetShaderValue(0);
        }
        else
        {
            activatedPopUps[activatedPopUps.Count - 1].transform.parent.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }

    public void DeactivateAllPopUps()
    {
        foreach(GameObject gO in new List<GameObject>(activatedPopUps))
        {
            DeactivatePopUp(gO);
        }
    }
}
