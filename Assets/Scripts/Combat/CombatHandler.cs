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
    public bool isRooted = false;
    public bool isBlind = false;
    public bool isDazed = false;

    public bool isImmuneToCC = false;
    public bool isPrecise = false;
    public bool hasTruestrike = false;
    public bool isEvasive = false;

    public float damageReceivedMultiplier = 1.0f;
    public float healingReceivedMultiplier = 1.0f;

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
            if (!spell.IsOnCooldown() & HasEnoughSpellResource(spell))
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

        UsedSpellResult spellResult = CastASpell(chosenSpell, enemy);
        
        return spellResult;
    }

    public UsedSpellResult CastASpell(EquippedCombatSpell chosenEquippedCombatSpell, CombatHandler enemy)
    {
        if (chosenEquippedCombatSpell == null)
            return null;
        if (!combatSpells.Contains(chosenEquippedCombatSpell))
            return null;
        if (chosenEquippedCombatSpell.cooldownLeft > 0)
            return null;


        UsedSpellResult spellResult = new UsedSpellResult();
        if (HasEnoughSpellResource(chosenEquippedCombatSpell))
        {
            spellResult = chosenEquippedCombatSpell.combatSpell.UseSpell(this, enemy);
            combatStats.LowerSpellResource(chosenEquippedCombatSpell.combatSpell.spellCost);
            chosenEquippedCombatSpell.PutOnCooldown();
        }
        else
        {
            spellResult.notEnoughSpellResource = true;
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

    public void ResetAllCooldowns()
    {
        foreach (EquippedCombatSpell spell in combatSpells)
        {
            spell.cooldownLeft = 0;
        }
    }

    private bool HasEnoughSpellResource(EquippedCombatSpell spell)
    {
        if(combatStats.currentSpellResource >= spell.combatSpell.spellCost)
        {
            return true;
        }
        return false;
    }

    public List<AppliedStatusEffect> TurnStart()
    {
        LowerAllCooldowns(1);
        return InvokeStatusEffects();
    }

    public void EndOfCombat()
    {
        isInjured = false;
        ResetAllCooldowns();
        ClearAllStatusEffects();
        ReapplyStatusEffects();
    }

    public void SetAllStatesToFalse()
    {
        isStunned = false;
        isRooted = false;
        isBlind = false;
        isDazed = false;

        isImmuneToCC = false;
        isPrecise = false;
        hasTruestrike = false;
        isEvasive = false;
    }

    public List<AppliedStatusEffect> InvokeStatusEffects()
    {
        SetAllStatesToFalse();
        List<AppliedStatusEffect> newStatusEffects = new List<AppliedStatusEffect>();
        foreach (AppliedStatusEffect appliedStatusEffect in statusEffects)
        {
            appliedStatusEffect.statusEffect.turnDuration -= 1;
            if (appliedStatusEffect.statusEffect.turnDuration >= 0)
            {
                newStatusEffects.Add(appliedStatusEffect);
            }
            else
            {
                continue;
            }
        }
        statusEffects = newStatusEffects;
        ReapplyStatusEffects();
        return newStatusEffects;
    }

    public void ReapplyStatusEffects()
    {
        //IMPLEMENT MORE TEMP EFFECTS
        CombatStats.StatFields statFlatBonus = new CombatStats.StatFields(0f);
        CombatStats.StatFields statMultiplier = new CombatStats.StatFields(1f);
        damageReceivedMultiplier = 1;
        healingReceivedMultiplier = 1;
        SetAllStatesToFalse();
        foreach (AppliedStatusEffect appliedStatusEffect in statusEffects)
        {
            if(appliedStatusEffect.statusEffect.turnDuration < 0)
            {
                continue;
            }

            if (appliedStatusEffect.statusEffect.GetType() == typeof(BeneficialStatusEffect))
            {
                switch (((BeneficialStatusEffect)appliedStatusEffect.statusEffect).statusEffectType)
                {
                    case BeneficialStatusEffectType.ANTI_CC:
                        isImmuneToCC = true;
                        break;

                    case BeneficialStatusEffectType.PRECISE:
                        isPrecise = true;
                        break;

                    case BeneficialStatusEffectType.EVASIVE:
                        isEvasive = true;
                        break;

                    case BeneficialStatusEffectType.TRUESTRIKE:
                        hasTruestrike = true;
                        break;

                    case BeneficialStatusEffectType.HEALING_OVER_TIME:
                        appliedStatusEffect.intensityToReceive = CalculateHealingReceived(appliedStatusEffect.intensityToReceive);
                        break;

                    case BeneficialStatusEffectType.INCREASE_AVOID_POTENCY:
                        statFlatBonus.avoidPotency += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.avoidPotency *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_CRITICAL_POTENCY:
                        statFlatBonus.criticalPotency += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.criticalPotency *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_HIT_POTENCY:
                        statFlatBonus.hitPotency += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.hitPotency *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_MAGIC_RESISTANCE:
                        statFlatBonus.magicalResistance += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.magicalResistance *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_PHYSICAL_RESISTANCE:
                        statFlatBonus.physicalResistance += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.physicalResistance *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_MAX_HEALTH:
                        statFlatBonus.maxHealth += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.maxHealth *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_MAX_SPELL_RESOURCE:
                        statFlatBonus.maxSpellResource += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.maxSpellResource *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_MAGIC_INTENSITY:
                        statFlatBonus.magicalDamage += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.magicalDamage *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_PHYSICAL_INTENSITY:
                        statFlatBonus.physicalDamage += appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.physicalDamage *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.DECREASE_DAMAGE_RECEIVED:
                        damageReceivedMultiplier /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case BeneficialStatusEffectType.INCREASE_HEALING_RECEIVED:
                        healingReceivedMultiplier *= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;
                }
            }

            else if (appliedStatusEffect.statusEffect.GetType() == typeof(HarmfulStatusEffect))
            {
                switch (((HarmfulStatusEffect)appliedStatusEffect.statusEffect).statusEffectType)
                {
                    case HarmfulStatusEffectType.STUN:
                        isStunned = true;
                        break;

                    case HarmfulStatusEffectType.BLIND:
                        isStunned = true;
                        break;

                    case HarmfulStatusEffectType.ROOT:
                        isRooted = true;
                        break;

                    case HarmfulStatusEffectType.DAZED:
                        isDazed = true;
                        break;

                    case HarmfulStatusEffectType.DAMAGE_OVER_TIME:
                        appliedStatusEffect.intensityToReceive = CalculateDamageReceived(appliedStatusEffect.intensityToReceive);
                        break;

                    case HarmfulStatusEffectType.DECREASE_AVOID_POTENCY:
                        statFlatBonus.avoidPotency -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.avoidPotency /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_CRITICAL_POTENCY:
                        statFlatBonus.criticalPotency -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.criticalPotency /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_HIT_POTENCY:
                        statFlatBonus.hitPotency -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.hitPotency /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_MAGIC_RESISTANCE:
                        statFlatBonus.magicalResistance -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.magicalResistance /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_PHYSICAL_RESISTANCE:
                        statFlatBonus.physicalResistance -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.physicalResistance /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_MAX_HEALTH:
                        statFlatBonus.maxHealth -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.maxHealth /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_MAX_SPELL_RESOURCE:
                        statFlatBonus.maxSpellResource -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.maxSpellResource /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_MAGIC_INTENSITY:
                        statFlatBonus.magicalDamage -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.magicalDamage /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;

                    case HarmfulStatusEffectType.DECREASE_PHYSICAL_INTENSITY:
                        statFlatBonus.physicalDamage -= appliedStatusEffect.intensityToReceive.statFlatIntensity;
                        statMultiplier.physicalDamage /= appliedStatusEffect.intensityToReceive.statMultiplier;
                        break;
                }
            }
        }

        combatStats.SetTmpCombatStats(statFlatBonus, statMultiplier);
    }

    public void DispellStatusEffectsOfType(BeneficialStatusEffectType statusType)
    {
        statusEffects.RemoveAll(s => (s.GetType() == typeof(BeneficialStatusEffect) & ((BeneficialStatusEffect)s.statusEffect).statusEffectType == statusType));
    }

    public void ClearAllStatusEffects()
    {
        statusEffects.Clear();
    }

    public bool TryInflictHarmfulStatusEffect(AppliedStatusEffect statusEffect)
    {
        if (isImmuneToCC)
            return false;
        if (DoAvoidCheck(statusEffect.intensityToReceive))
        {
            return false;
        }
        statusEffects.Add(new AppliedStatusEffect(statusEffect));
        ReapplyStatusEffects();
        return true;
    }

    public bool TryInflictBeneficialStatusEffect(AppliedStatusEffect statusEffect)
    {
        statusEffects.Add(new AppliedStatusEffect(statusEffect));
        ReapplyStatusEffects();
        return true;
    }

    public AppliedIntensityInstance CalculateDamageReceived(AppliedIntensityInstance dmg)
    {
        AppliedIntensityInstance finalDmg = new AppliedIntensityInstance(dmg);

        //Calculate resistances
        float maxRes = 100 + (dmg.originLevel - 1) * 20;
        if (dmg.primaryIntensityType == PrimaryIntensityType.PHYSICAL)
        {
            float finalRes = combatStats.totalStats.physicalResistance / maxRes;
            if (finalRes > 0.95f)
                finalRes = 0.95f;
            finalDmg.intensity = dmg.intensity * (1 - finalRes);
        }
        else if (dmg.primaryIntensityType == PrimaryIntensityType.MAGICAL)
        {
            float finalRes = combatStats.totalStats.magicalResistance / maxRes;
            if (finalRes > 0.9f)
                finalRes = 0.9f;
            finalDmg.intensity = dmg.intensity * (1 - finalRes);
        }

        //Avoid check (after resistance calculation to show potential damage on log)
        if (DoAvoidCheck(dmg))
        {
            //If successfully avoided
            finalDmg.hasBeenAvoided = true;
            //Return information about damage avoided
            return finalDmg;
        }

        //Roll for critical

        if(!DoCriticalCheck(dmg))
        {
            //If critical check fails
            finalDmg.intensity *= 2;
            finalDmg.isCritical = true;
        }

        //Apply multiplier
        finalDmg.intensity *= damageReceivedMultiplier;
        //Deal damage
        LowerHealth(finalDmg.intensity);
        

        //Return information about damage dealt
        return finalDmg;
    }

    public AppliedIntensityInstance CalculateHealingReceived(AppliedIntensityInstance heal)
    {
        AppliedIntensityInstance finalHeal = new AppliedIntensityInstance(heal);
        //Apply multiplier
        finalHeal.intensity *= healingReceivedMultiplier;
        RaiseHealth(finalHeal.intensity);
        return finalHeal;
    }

    public bool DoAvoidCheck(AppliedIntensityInstance intInstance)
    {
        //Calcuting avoidanceRank based on states of character and states of attacker
        int avoidanceRank = (isRooted ? -1 : 0) + (isEvasive ? 1 : 0) + (intInstance.castWithPrecise ? -1 : 0) + (intInstance.castWithBlind ? 1 : 0);

        if (avoidanceRank > 0)
            return true;
        else if (avoidanceRank < 0)
            return false;

        //Checks if normal attack is avoided
        float finalAvoid = combatStats.totalStats.avoidPotency / (combatStats.totalStats.avoidPotency + intInstance.originHitPotency + 100);
        if (RollChance(finalAvoid))
        {
            return true;
        }
        return false;
    }

    public bool DoCriticalCheck(AppliedIntensityInstance intInstance)
    {
        //Calcuting avoidanceRank based on states of character and states of attacker
        int avoidanceRank = (intInstance.castWithTruestrike ? -1 : 0) + (intInstance.castWithDazed ? 1 : 0);

        if (avoidanceRank > 0)
            return true;
        else if (avoidanceRank < 0)
            return false;

        //Checks if critical is avoided
        float finalCritical = (2f * combatStats.totalStats.avoidPotency + 100) / (2* combatStats.totalStats.avoidPotency + intInstance.originCriticalPotency + 100);

        //TODO: create a critical multiplier
        if (RollChance(finalCritical))
        {
            return true;
        }
        return false;
    }

    public bool RollChance(float percentage)
    {
        //Just for safety
        if (percentage > 1)
            percentage = 1;
        if (percentage <= 0)
            return false;

        //Random check
        if (Random.Range(0.0f, 1.0f) <= percentage)
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
