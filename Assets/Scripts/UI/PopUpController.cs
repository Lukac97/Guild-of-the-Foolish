using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopUpController : MonoBehaviour
{
    public GameObject background;
    [Range(0.0f, 100.0f)]
    public float shadeLevel;
    [HideInInspector]
    public List<GameObject> allPopUps;

    private List<GameObject> activatedPopUps;

    private void Start()
    {
        activatedPopUps = new List<GameObject>();
        allPopUps = new List<GameObject>();
        foreach (Transform child in gameObject.transform)
        {
            allPopUps.Add(child.gameObject);
        }
    }

    private void SetShaderValue(float shaderVal)
    {
        Color currentColor = background.GetComponent<Image>().color;
        currentColor.a = shaderVal / 100;
        background.GetComponent<Image>().color = currentColor;
    }

    public void ActivatePopUp(GameObject popUp)
    {
        if(activatedPopUps.Count == 0)
        {
            SetShaderValue(shadeLevel);
        }
        popUp.SetActive(true);
        activatedPopUps.Add(popUp);
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
    }
}
