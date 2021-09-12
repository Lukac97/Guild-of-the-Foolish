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
    private Attributes tempAttributes;

    [Space(5)]
    public int availablePoints;
    [Header("Global states")]
    public bool isMoving;
    public bool isBattling;

    private CharEquipment charEquipment;
    private CharCombat charCombat;

    private List<AppliedConsumableEffect> appliedConsumableEffects = new List<AppliedConsumableEffect>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        charEquipment = GetComponent<CharEquipment>();
        charCombat = GetComponent<CharCombat>();
        //Placeholder until serialization happens
        if(charAttributes.SumOfAllAttributes() == 0)
            charAttributes = new Attributes(characterClass.startingAttributes);
        if (tempAttributes == null)
            tempAttributes = new Attributes();
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

    public void MoveCharacter(Location loc)
    {
        isMoving = true;
        GlobalTime.CreateCharacterMoveAction(this, loc);
        CharactersController.CharactersUpdated.Invoke();
    }

    public void ChangeLocation(Location loc)
    {
        isMoving = false;
        location = loc;
        CharactersController.CharactersUpdated.Invoke();
    }

    public void UpdateTotalAttributes()
    {
        totalAttributes = charAttributes + charEquipment.equipmentAttributes + tempAttributes;
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
        float maxExperiencePoints = GlobalRules.maxExperienceForLevel(level);
        if (experiencePoints + amount >= maxExperiencePoints)
        {
            float tempExpPoints = experiencePoints;
            LevelUpCharacter();
            experiencePoints = tempExpPoints + amount - maxExperiencePoints;
        }
        else
        {
            experiencePoints += amount;
        }
    }

    public void LevelUpCharacter()
    {
        experiencePoints = 0;
        level += 1;
        availablePoints += GlobalRules.attributePointsPerLevel;

        CharactersController.CharactersUpdated.Invoke();
    }

    public void ApplyConsumableEffect(ConsumableEffect consumableEffect)
    {
        AppliedConsumableEffect appliedCE = new AppliedConsumableEffect(consumableEffect);
        HandleConsumableEffect(appliedCE);
        appliedConsumableEffects.Add(appliedCE);
    }

    public void InvokeConsumableEffects()
    {
        List<AppliedConsumableEffect> newAppliedConsumableEffects = new List<AppliedConsumableEffect>(appliedConsumableEffects);
        foreach(AppliedConsumableEffect appliedCE in appliedConsumableEffects)
        {
            appliedCE.halfCyclesLeft -= 1;
            if(appliedCE.halfCyclesLeft <= 0)
            {
                HandleConsumableEffect(appliedCE, true);
                newAppliedConsumableEffects.Remove(appliedCE);
            }
        }
        appliedConsumableEffects = newAppliedConsumableEffects;
    }

    private void HandleConsumableEffect(AppliedConsumableEffect appliedCE, bool toRemove = false)
    {
        if (appliedCE.consumableEffect.GetType() == typeof(BeneficialConsumableEffect))
        {
            if (((BeneficialConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                BeneficialConsumableEffectType.INCREASE_STRENGTH)
            {
                tempAttributes.strength += appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
            else if (((BeneficialConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                BeneficialConsumableEffectType.INCREASE_AGILITY)
            {
                tempAttributes.agility += appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
            else if (((BeneficialConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                BeneficialConsumableEffectType.INCREASE_INTELLECT)
            {
                tempAttributes.intellect += appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
            else if (((BeneficialConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                BeneficialConsumableEffectType.INCREASE_LUCK)
            {
                tempAttributes.luck += appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
        }
        else if (appliedCE.consumableEffect.GetType() == typeof(HarmfulConsumableEffect))
        {
            if (((HarmfulConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                HarmfulConsumableEffectType.DECREASE_STRENGTH)
            {
                tempAttributes.strength -= appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
            else if (((HarmfulConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                HarmfulConsumableEffectType.DECREASE_AGILITY)
            {
                tempAttributes.agility -= appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
            else if (((HarmfulConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                HarmfulConsumableEffectType.DECREASE_INTELLECT)
            {
                tempAttributes.intellect -= appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
            else if (((HarmfulConsumableEffect)appliedCE.consumableEffect).consumableEffectType ==
                HarmfulConsumableEffectType.DECREASE_LUCK)
            {
                tempAttributes.luck -= appliedCE.consumableEffect.intensity * (toRemove ? -1 : 1);
            }
        }
        UpdateTotalAttributes();
    }
}
