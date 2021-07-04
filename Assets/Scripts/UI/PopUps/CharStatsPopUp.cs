using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharStatsPopUp : MonoBehaviour
{
    private static CharStatsPopUp _instance;
    public static CharStatsPopUp Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject activatable;
    public TextMeshProUGUI titleText;
    [SerializeField]
    private CmbStatsView cmbStatsView;
    [SerializeField]
    private AttributesView attributesView;

    public GameObject currentChar;

    private PopUpController popUpPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        popUpPanel = GetComponentInParent<PopUpController>();
    }

    public void OpenCharStats(GameObject selectedChar)
    {
        if (selectedChar == null)
            return;
        currentChar = selectedChar;
        UpdateCharStatsContent();
        popUpPanel.ActivatePopUp(activatable);
    }

    public void UpdateCharStatsContent()
    {
        titleText.text = currentChar.GetComponent<CharStats>().characterName;
        cmbStatsView.ChangeInformation();
        attributesView.UpdateAttributesView();
    }

    public void CloseCharStats()
    {
        popUpPanel.DeactivatePopUp(activatable);
    }
}
