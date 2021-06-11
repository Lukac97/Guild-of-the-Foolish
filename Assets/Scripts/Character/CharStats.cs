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
    public float experiencePoints;

    [Space(5)]
    public Attributes charAttributes;
    public Attributes totalAttributes;

    [Space(5)]
    public int availablePoints;

    private CharEquipment charEquipment;
    private CharCombat charCombat;

    private void Start()
    {
        charEquipment = GetComponent<CharEquipment>();
        charCombat = GetComponent<CharCombat>();
        //Placeholder until serialization happens
        if(charAttributes.SumOfAllAttributes() == 0)
            charAttributes = new Attributes(characterClass.startingAttributes);
        //End of placeholder
        UpdateTotalAttributes();
    }

    public void InitCharStats(CharClass newCharClass, string newCharName)
    {
        gameObject.name = newCharName;
        characterName = newCharName;
        characterClass = newCharClass;
        location = GlobalInput.Instance.defaultLocation;
        level = 1;
        availablePoints = 0;
        experiencePoints = 0;
        charAttributes = new Attributes(newCharClass.startingAttributes);
    }

    public void ChangeLocation(Location loc)
    {
        location = loc;
        CharactersController.CharactersUpdated.Invoke();
    }

    public void UpdateTotalAttributes()
    {
        totalAttributes = charAttributes + charEquipment.equipmentAttributes;
        charCombat.UpdateCharCombat();
    }

    public void IncreaseAttribute(AttributesEnum attributeEnum)
    {
        if (availablePoints <= 0)
            return;
        if (attributeEnum == AttributesEnum.STRENGTH)
        {
            charAttributes.strength += 1;
            availablePoints -= 1;
        }
        else if (attributeEnum == AttributesEnum.AGILITY)
        {
            charAttributes.agility += 1;
            availablePoints -= 1;
        }
        else if (attributeEnum == AttributesEnum.INTELLECT)
        {
            charAttributes.intellect += 1;
            availablePoints -= 1;
        }
        else if (attributeEnum == AttributesEnum.LUCK)
        {
            charAttributes.luck += 1;
            availablePoints -= 1;
        }
        UpdateTotalAttributes();
    }

    public void AddExperience(float amount)
    {
        experiencePoints += amount;
        //TODO: leveling up
    }
}
