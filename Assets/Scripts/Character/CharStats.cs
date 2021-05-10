using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string characterName;

    public CharClass characterClass;

    public Location location;

    [Space(5)]
    public int level;
    public int experiencePoints;

    [Space(5)]
    public Attributes charAttributes;
    public Attributes totalAttributes;

    private CharEquipment charEquipment;

    private void Start()
    {
        charEquipment = GetComponent<CharEquipment>();
        //Placeholder until serialization happens
        if(charAttributes == null)
        {
            charAttributes = new Attributes();
        }
    }

    public void InitCharStats(CharClass newCharClass, string newCharName)
    {
        characterName = newCharName;
        characterClass = newCharClass;
        location = GlobalInput.Instance.defaultLocation;
        level = 1;
        experiencePoints = 0;
        charAttributes = new Attributes(newCharClass.startingAttributes);
    }

    public void ChangeLocation(Location loc)
    {
        location = loc;
        CharactersController.Instance.CharactersUpdated.Invoke();
    }

    public void UpdateTotalAttributes()
    {
        totalAttributes = charAttributes + charEquipment.equipmentAttributes;
    }
}
