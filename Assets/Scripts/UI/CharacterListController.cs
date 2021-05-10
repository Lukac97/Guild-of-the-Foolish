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
    // Start is called before the first frame update
    private void Start()
    {
        CharactersController.Instance.CharactersUpdated += OnUpdateCharacters;
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

    private void OnUpdateCharacters()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject newChar in CharactersController.Instance.characters)
        {
            GameObject createdChar = Instantiate(characterPrefab, panel.transform);
            createdChar.name = newChar.name;
            createdChar.GetComponent<CharListElement>().LinkCharacter(newChar.GetComponent<CharStats>());
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
