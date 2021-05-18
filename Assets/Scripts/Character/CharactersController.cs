using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersController : MonoBehaviour
{
    private static CharactersController _instance;
    public static CharactersController Instance { get { return _instance; } }

    public delegate void UpdateCharactersDelegate();
    public UpdateCharactersDelegate CharactersUpdated;

    public delegate void UpdateCharactersResourcesDelegate();
    public UpdateCharactersResourcesDelegate CharactersResourcesUpdated;

    public List<GameObject> characters;

    public GameObject newCharPrefab;

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        foreach (Transform child in gameObject.transform)
        {
            if (!characters.Contains(child.gameObject))
            {
                characters.Add(child.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
        Instance.CharactersUpdated.Invoke();
    }
}
