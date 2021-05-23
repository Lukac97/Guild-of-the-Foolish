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
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

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

    public void TurnStart()
    {
        LowerAllCooldowns(1);
        InvokeStatusEffects();
    }

    public void SetAllStatesToFalse()
    {
        isInjured = false;
        isStunned = false;
        isImmuneToCC = false;
    }

    public void InvokeStatusEffects()
    {
        SetAllStatesToFalse();
        List<StatusEffect> newStatusEffects = new List<StatusEffect>();
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.GetType() == typeof(BeneficialStatusEffect))
                if (((BeneficialStatusEffect)statusEffect).statusEffectType == BeneficialStatusEffectType.ANTI_CC)
                    isImmuneToCC = true;

            if (statusEffect.GetType() == typeof(HarmfulStatusEffect))
                if (((HarmfulStatusEffect)statusEffect).statusEffectType == HarmfulStatusEffectType.STUN)
                    isStunned = true;
            statusEffect.turnDuration -= 1;
            Debug.Log(statusEffect.turnDuration);
            if (statusEffect.turnDuration > 0)
                newStatusEffects.Add(statusEffect);
        }
        statusEffects = newStatusEffects;
    }

    public bool TryInflictHarmfulStatusEffect(HarmfulStatusEffect statusEffect)
    {
        //TODO: Roll chance based on combatStats on whether or not to inflict status effect
        if (isImmuneToCC)
            return false;
        statusEffects.Add(new HarmfulStatusEffect(statusEffect));
        return true;
    }

    public bool TryInflictBeneficialStatusEffect(BeneficialStatusEffect statusEffect)
    {
        //TODO: Roll chance based on combatStats on whether or not to inflict status effect
        statusEffects.Add(new BeneficialStatusEffect(statusEffect));
        return true;
    }

    public float CalculateDamageReceived(float dmg)
    {
        float finalDmg = dmg;
        //TODO: Calculate damage reduction based on combatStats

        LowerHealth(finalDmg);
        return finalDmg;
    }

    public float CalculateHealingReceived(float heal)
    {
        float finalHeal = heal;
        //TODO: Calculate heal amplification based on combatStats
        RaiseHealth(finalHeal);
        return finalHeal;
    }

    #region Resource Management
    public void RaiseHealth(float amount)
    {
        combatStats.RaiseHealth(amount);
        if (isInjured & combatStats.currentHealth > 0)
            isInjured = false;
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

}
