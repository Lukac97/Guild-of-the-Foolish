using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatHandler : MonoBehaviour
{
    [System.Serializable]
    public class EquippedCombatSpell
    {
        public CombatSpell combatSpell;
        public int cooldownLeft;

        public EquippedCombatSpell(CombatSpell _combatSpell)
        {
            combatSpell = _combatSpell;
            cooldownLeft = 0;
        }

        public bool IsOnCooldown()
        {
            return cooldownLeft > 0 ? true : false;
        }

        public void PutOnCooldown()
        {
            cooldownLeft = combatSpell.turnCooldown + 1;
        }

        public void LowerCooldown(int amount)
        {
            if (cooldownLeft - amount <= 0)
                cooldownLeft = 0;
            else
                cooldownLeft -= amount;
        }
    }

    [Space(5)]
    [Header("Combat stats")]
    public CombatStats combatStats;

    [Header("Statuses")]
    public bool isInjured = false;
    public bool isStunned = false;
    public bool isImmuneToCC = false;
    public List<AppliedStatusEffect> statusEffects = new List<AppliedStatusEffect>();

    [Header("Spells")]
    public List<EquippedCombatSpell> combatSpells = new List<EquippedCombatSpell>();

    //For overriding
    protected virtual void NotifyResourcesUpdated()
    {
    }
    
    //For overriding
    public virtual UsedSpellResult ChooseSpell(CombatHandler enemy)
    {

        List<EquippedCombatSpell> availableSpells = new List<EquippedCombatSpell>();
        foreach (EquippedCombatSpell spell in combatSpells)
        {
            if (!spell.IsOnCooldown())
                availableSpells.Add(spell);
        }

        List<EquippedCombatSpell> optimalSpells = new List<EquippedCombatSpell>();
        List<EquippedCombatSpell> nonOptimalSpells = new List<EquippedCombatSpell>();
        foreach (EquippedCombatSpell spell in availableSpells)
        {
            //TODO: Logic for selecting optimal and nonoptimal spells
            optimalSpells.Add(spell);
        }

        EquippedCombatSpell chosenSpell = null;
        //TODO: More advanced AI for choosing spells
        if (optimalSpells.Count > 0)
            chosenSpell = optimalSpells[0];
        if (nonOptimalSpells.Count > 0)
            chosenSpell = nonOptimalSpells[0];

        UsedSpellResult spellResult = null;
        if (chosenSpell != null)
        {
            spellResult = chosenSpell.combatSpell.UseSpell(this, enemy);
            chosenSpell.cooldownLeft = chosenSpell.combatSpell.turnCooldown;
        }
        
        return spellResult;
    }


    public void LowerAllCooldowns(int amount)
    {
        foreach(EquippedCombatSpell spell in combatSpells)
        {
            spell.LowerCooldown(amount);
        }
    }

    public List<AppliedStatusEffect> TurnStart()
    {
        LowerAllCooldowns(1);
        return InvokeStatusEffects();
    }

    public void SetAllStatesToFalse()
    {
        isStunned = false;
        isImmuneToCC = false;
    }

    public List<AppliedStatusEffect> InvokeStatusEffects()
    {
        SetAllStatesToFalse();
        List<AppliedStatusEffect> newStatusEffects = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> inflictedStatusEffects = new List<AppliedStatusEffect>();
        foreach (AppliedStatusEffect appliedStatusEffect in statusEffects)
        {
            if (appliedStatusEffect.statusEffect.GetType() == typeof(BeneficialStatusEffect))
            {
                if (((BeneficialStatusEffect)appliedStatusEffect.statusEffect).statusEffectType == BeneficialStatusEffectType.ANTI_CC)
                {
                    AppliedStatusEffect inflictedSE = new AppliedStatusEffect(appliedStatusEffect);
                    inflictedStatusEffects.Add(inflictedSE);
                    isImmuneToCC = true;
                }
                if (((BeneficialStatusEffect)appliedStatusEffect.statusEffect).statusEffectType == BeneficialStatusEffectType.HEALING_OVER_TIME)
                {
                    AppliedStatusEffect inflictedSE = new AppliedStatusEffect(appliedStatusEffect);
                    inflictedSE.intensityToReceive = CalculateHealingReceived(appliedStatusEffect.intensityToReceive);
                    inflictedStatusEffects.Add(inflictedSE);
                }
            }

            if (appliedStatusEffect.statusEffect.GetType() == typeof(HarmfulStatusEffect))
            {
                if (((HarmfulStatusEffect)appliedStatusEffect.statusEffect).statusEffectType == HarmfulStatusEffectType.STUN)
                {
                    AppliedStatusEffect inflictedSE = new AppliedStatusEffect(appliedStatusEffect);
                    inflictedStatusEffects.Add(inflictedSE);
                    isStunned = true;
                }
                if (((HarmfulStatusEffect)appliedStatusEffect.statusEffect).statusEffectType == HarmfulStatusEffectType.DAMAGE_OVER_TIME)
                {
                    AppliedStatusEffect inflictedSE = new AppliedStatusEffect(appliedStatusEffect);
                    inflictedSE.intensityToReceive = CalculateDamageReceived(appliedStatusEffect.intensityToReceive);
                    inflictedStatusEffects.Add(inflictedSE);
                }
            }

            appliedStatusEffect.statusEffect.turnDuration -= 1;
            if (appliedStatusEffect.statusEffect.turnDuration > 0)
                newStatusEffects.Add(appliedStatusEffect);
        }
        statusEffects = newStatusEffects;
        return inflictedStatusEffects;
    }

    public bool TryInflictHarmfulStatusEffect(AppliedStatusEffect statusEffect)
    {
        //TODO: Roll chance based on combatStats on whether or not to inflict status effect
        if (isImmuneToCC)
            return false;
        statusEffects.Add(new AppliedStatusEffect(statusEffect));
        return true;
    }

    public bool TryInflictBeneficialStatusEffect(AppliedStatusEffect statusEffect)
    {
        //TODO: Roll chance based on combatStats on whether or not to inflict status effect
        statusEffects.Add(new AppliedStatusEffect(statusEffect));
        return true;
    }

    public AppliedIntensityInstance CalculateDamageReceived(AppliedIntensityInstance dmg)
    {
        AppliedIntensityInstance finalDmg = new AppliedIntensityInstance(dmg);

        //Calculate resistances
        float maxRes = 100 + (dmg.originLevel - 1) * 20;
        if (dmg.primaryIntensityType == PrimaryIntensityType.PHYSICAL)
        {
            float finalRes = combatStats.physicalResistance / maxRes;
            if (finalRes > 0.95f)
                finalRes = 0.95f;
            finalDmg.intensity = dmg.intensity * (1 - finalRes);
        }
        else if (dmg.primaryIntensityType == PrimaryIntensityType.MAGICAL)
        {
            float finalRes = combatStats.magicalResistance / maxRes;
            if (finalRes > 0.9f)
                finalRes = 0.9f;
            finalDmg.intensity = dmg.intensity * (1 - finalRes);
        }

        //Avoid check (after resistance calculation to show potential damage on log)
        float finalAvoid = 100 + (dmg.originLevel - 1) * 20;
        finalAvoid = combatStats.avoidPotency / finalAvoid;
        if (finalAvoid > 0.8f)
            finalAvoid = 0.8f;

        if (RollChance(finalAvoid))
        {
            finalDmg.hasBeenAvoided = true;
            //Return information about damage avoided
            return finalDmg;
        }

        //Roll for critical
        float finalCritical = 100 + (dmg.originLevel - 1) * 20;
        finalCritical = dmg.originCriticalPotency / finalCritical;
        if (finalCritical > 0.8f)
            finalCritical = 0.8f;

        //TODO: create a critical multiplier
        if(RollChance(finalCritical))
        {
            finalDmg.intensity *= 2;
            finalDmg.isCritical = true;
        }

        //Deal damage
        LowerHealth(finalDmg.intensity);
        

        //Return information about damage dealt
        return finalDmg;
    }

    public AppliedIntensityInstance CalculateHealingReceived(AppliedIntensityInstance heal)
    {
        AppliedIntensityInstance finalHeal = new AppliedIntensityInstance(heal);
        //TODO: Calculate heal amplification based on combatStats
        RaiseHealth(finalHeal.intensity);
        return finalHeal;
    }

    public bool RollChance(float percentage)
    {
        //Just for safety
        if (percentage > 1)
            percentage = 1;
        if (percentage < 0)
            percentage = 0;

        //Random check
        if(Random.Range(0.0f, 1.0f) < percentage)
        {
            return true;
        }
        return false;
    }


    #region Resource Management
    public void RaiseHealth(float amount)
    {
        if (isInjured)
            return;
        combatStats.RaiseHealth(amount);
        NotifyResourcesUpdated();
    }

    public void LowerHealth(float amount)
    {
        if (combatStats.LowerHealth(amount))
            isInjured = true;
        NotifyResourcesUpdated();
    }

    public void RaiseSpellResource(float amount)
    {
        combatStats.RaiseSpellResource(amount);
        NotifyResourcesUpdated();
    }

    public void LowerSpellResource(float amount)
    {
        combatStats.LowerSpellResource(amount);
        NotifyResourcesUpdated();
    }

    public void FillResources()
    {
        combatStats.FillResources();
        NotifyResourcesUpdated();
    }
    #endregion Resource Management

    public virtual int GetLevel()
    {
        return 0;
    }

}