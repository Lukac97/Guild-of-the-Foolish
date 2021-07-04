using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersController : MonoBehaviour
{
    private static CharactersController _instance;
    public static CharactersController Instance { get { return _instance; } }

    public delegate void UpdateCharactersDelegate();
    public static UpdateCharactersDelegate CharactersUpdated;

    public delegate void NumberOfCharsChangedDelegate();
    public static NumberOfCharsChangedDelegate NrOfCharsChanged;

    public delegate void UpdateCharactersResourcesDelegate();
    public static UpdateCharactersResourcesDelegate CharactersResourcesUpdated;

    public delegate void ChosenSpellsUpdatedDelegate();
    public static ChosenSpellsUpdatedDelegate ChangedChosenSpells;

    public List<GameObject> characters;

    public GameObject newCharPrefab;

    public void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
    }

    public void CreateNewCharater(CharClass newCharClass, string charName, int price)
    {
        bool hasEnough = GuildMain.Instance.SubtractGold(price);
        if(!hasEnough)
        {
            Debug.Log("Not enough gold!");
            return;
        }
        GameObject newChar = Instantiate(newCharPrefab, gameObject.transform);
        newChar.GetComponent<CharStats>().InitCharStats(newCharClass, charName);
        characters.Add(newChar);
        NrOfCharsChanged.Invoke();
        CharactersUpdated.Invoke();
    }
}
