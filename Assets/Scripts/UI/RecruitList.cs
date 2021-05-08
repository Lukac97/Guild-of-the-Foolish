using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitList : MonoBehaviour
{
    public GameObject panel;
    public GameObject recruitPrefab;
    public PopUp chooseNamePopUp;
    // Start is called before the first frame update
    void Start()
    {
        InitRecruits();
    }

    public void InitRecruits()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (CharClass charClass in GlobalInput.Instance.possibleClasses)
        {
            GameObject newRecruit = Instantiate(recruitPrefab, panel.transform);
            newRecruit.GetComponent<RecruitListElement>().InitRecruitText(charClass, 100);
        }
    }
}
