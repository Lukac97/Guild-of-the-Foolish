using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttributesView : MonoBehaviour
{
    public TextMeshProUGUI attributesTitle;
    [Space(6)]
    public TextMeshProUGUI strengthTMP;
    public GameObject strengthButton;
    [Space(3)]
    public TextMeshProUGUI agilityTMP;
    public GameObject agilityButton;
    [Space(3)]
    public TextMeshProUGUI intellectTMP;
    public GameObject intellectButton;
    [Space(3)]
    public TextMeshProUGUI luckTMP;
    public GameObject luckButton;
    [Space(3)]
    public CanvasGroup bodyPanel;

    public string defaultTitle = "Attributes";

    private CharStats currentChar = null;

    private void Awake()
    {
        CharactersController.CharactersUpdated += UpdateAttributesView;
        CharTabMain.CharTabChangedChar += UpdateAttributesView;
    }

    private void Start()
    {
        UpdateAttributesView();
    }

    public void UpdateAttributesView()
    {
        if (CharTabMain.Instance.currentChar == null)
        {
            bodyPanel.alpha = 0;
            bodyPanel.interactable = false;
            bodyPanel.blocksRaycasts = false;

            strengthButton.SetActive(false);
            agilityButton.SetActive(false);
            intellectButton.SetActive(false);
            luckButton.SetActive(false);

            return;
        }

        bodyPanel.alpha = 1;
        bodyPanel.interactable = true;
        bodyPanel.blocksRaycasts = true;
        currentChar = CharTabMain.Instance.currentChar.GetComponent<CharStats>();
        if (currentChar.availablePoints > 0)
        {
            attributesTitle.text = defaultTitle + " (" + currentChar.availablePoints.ToString() + ")";

            strengthButton.SetActive(true);
            agilityButton.SetActive(true);
            intellectButton.SetActive(true);
            luckButton.SetActive(true);
        }
        else
        {
            attributesTitle.text = defaultTitle;

            strengthButton.SetActive(false);
            agilityButton.SetActive(false);
            intellectButton.SetActive(false);
            luckButton.SetActive(false);
        }
        strengthTMP.text = currentChar.totalAttributes.strength.ToString();
        agilityTMP.text = currentChar.totalAttributes.agility.ToString();
        intellectTMP.text = currentChar.totalAttributes.intellect.ToString();
        luckTMP.text = currentChar.totalAttributes.luck.ToString();
    }

    public void IncreaseStrength()
    {
        currentChar.IncreaseAttribute(AttributesEnum.STRENGTH);
    }

    public void IncreaseAgility()
    {
        currentChar.IncreaseAttribute(AttributesEnum.AGILITY);
    }

    public void IncreaseIntellect()
    {
        currentChar.IncreaseAttribute(AttributesEnum.INTELLECT);
    }

    public void IncreaseLuck()
    {
        currentChar.IncreaseAttribute(AttributesEnum.LUCK);
    }
}
