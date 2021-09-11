using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumableItem : Item
{
    public float healthRestorationFlat;
    [Tooltip("0-1")]
    public float healthRestorationPct;

    public float resourceRestorationFlat;
    [Tooltip("0-1")]
    public float resourceRestorationPct;

    public List<BeneficialConsumableEffect> beneficialConsumableEffects;
    public List<HarmfulConsumableEffect> harmfulConsumableEffects;

    public ConsumableItem(ConsumableItem _consumableItem)
    {
        healthRestorationFlat = _consumableItem.healthRestorationFlat;
        healthRestorationPct = _consumableItem.healthRestorationPct;
        resourceRestorationFlat = _consumableItem.resourceRestorationFlat;
        resourceRestorationPct = _consumableItem.resourceRestorationPct;
        beneficialConsumableEffects = _consumableItem.beneficialConsumableEffects;
        harmfulConsumableEffects = _consumableItem.harmfulConsumableEffects;
    }

    public bool UseConsumableOnCharacter(CharStats charStats)
    {
        ItemObject itemObject = GuildInventory.Instance.FindItem(this);
        if (itemObject == null)
            return false;


        CharCombat charCombat = charStats.GetComponent<CharCombat>();
        charCombat.RaiseHealth(healthRestorationFlat + charCombat.combatStats.baseStats.maxHealth * healthRestorationPct);
        charCombat.RaiseHealth(resourceRestorationFlat + charCombat.combatStats.baseStats.maxSpellResource * resourceRestorationPct);

        foreach(BeneficialConsumableEffect bEf in beneficialConsumableEffects)
        {
            charStats.ApplyConsumableEffect(bEf);
        }

        foreach(HarmfulConsumableEffect hEf in harmfulConsumableEffects)
        {
            charStats.ApplyConsumableEffect(hEf);
        }

        return true;
    }
}
