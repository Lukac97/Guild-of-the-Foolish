using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterListController : MonoBehaviour
{
    public GameObject panel;
    public GameObject characterPrefab;

    [SerializeField]
    private List<GameObject> lastHighlitedCharacters = new List<GameObject>();

    private void Awake()
    {
        CharactersController.CharactersUpdated += OnUpdateCharacters;
        CharactersController.CharactersResourcesUpdated += UpdateCharacterSliders;
    }

    private void Start()
    {
        GlobalInput.Instance.changeSelectedEntity += HighlightSelectedCharacter;
        OnUpdateCharacters();
    }

    public void DeHighlightCharacters()
    {
        foreach (GameObject lastHighlightedChar in lastHighlitedCharacters)
        {
            lastHighlightedChar.GetComponent<Outline>().enabled = false;
        }
        lastHighlitedCharacters.Clear();
    }
    private void HighlightCharacters()
    {

        foreach (GameObject highlightChar in lastHighlitedCharacters)
        {
            Outline outline = highlightChar.GetComponent<Outline>();
            outline.enabled = true;
        }
    }

    public void HighlightSelectedCharacter()
    {
        DeHighlightCharacters();
        if (!GlobalInput.Instance.CheckIfSelectedCharacter())
            return;
        lastHighlitedCharacters.Add(FindCharElement(GlobalInput.Instance.selectedEntity.GetComponent<CharStats>()).gameObject);
        HighlightCharacters();
    }

    private void UpdateCharacterSliders()
    {
        foreach(CharListElement child in panel.GetComponentsInChildren<CharListElement>())
        {
            child.UpdateSliders();
        }
    }

    private void OnUpdateCharacters()
    {
        List<CharListElement> charListElements = new List<CharListElement>();
        charListElements.AddRange(panel.GetComponentsInChildren<CharListElement>());
        List<CharListElement> leftoutElements = new List<CharListElement>(charListElements);
        foreach (GameObject newChar in CharactersController.Instance.characters)
        {
            CharStats charStats = newChar.GetComponent<CharStats>();
            bool foundMatch = false;
            foreach (CharListElement child in charListElements)
            {
                if (child.linkedCharacter == charStats)
                {
                    child.UpdateCharText();
                    leftoutElements.Remove(child);
                    foundMatch = true;
                }
            }
            if (foundMatch)
                continue;
            GameObject createdChar = Instantiate(characterPrefab, panel.transform);
            createdChar.name = newChar.name;
            createdChar.GetComponent<CharListElement>().LinkCharacter(charStats);
        }
        foreach(CharListElement charEle in leftoutElements)
        {
            Destroy(charEle.gameObject);
        }
    }

    public CharListElement FindCharElement(CharStats charStats)
    {
        foreach(Transform child in panel.transform)
        {
            CharListElement charEl = child.GetComponent<CharListElement>();
            if(charEl.linkedCharacter == charStats)
            {
                return charEl;
            }
        }
        return null;
    }
}
