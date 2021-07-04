using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharTabMain : MonoBehaviour
{
    private static CharTabMain _instance;
    public static CharTabMain Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject currentChar;
    public TMP_Dropdown charDropdown;

    public delegate void CharTabSelectedCharDelegate();
    public static CharTabSelectedCharDelegate CharTabChangedChar;

    private List<GameObject> availableChars;
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }

        CharactersController.NrOfCharsChanged += UpdateCharDropdown;
    }

    void Start()
    {
        UpdateCharDropdown();
    }

    public void OnDropdownChoice(int charNr)
    {
        SetCurrentChar(availableChars[charNr]);
    }

    public void SetCurrentChar(GameObject charObject)
    {
        if (charObject == null)
        {
            currentChar = null;
        }
        else
        {
            currentChar = charObject;
        }

        CharTabChangedChar.Invoke();
    }

    public void OnShowStats()
    {
        CharStatsPopUp.Instance.OpenCharStats(currentChar);
    }


    private void UpdateCharDropdown()
    {
        availableChars = CharactersController.Instance.characters;
        charDropdown.ClearOptions();
        List<string> charNames = new List<string>();
        foreach (GameObject character in availableChars)
        {
            charNames.Add(character.GetComponent<CharStats>().characterName);
        }
        charDropdown.AddOptions(charNames);
        if (currentChar == null)
        {
            charDropdown.value = 0;
            OnDropdownChoice(0);
        }
    }
}
