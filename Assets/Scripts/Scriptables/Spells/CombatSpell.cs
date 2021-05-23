using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[CreateAssetMenu(fileName = "New Combat Spell", menuName = "Spells/Combat Spell")]
public class CombatSpell : Spell
{
    public int turnCooldown;
    [Header("Intensity")]
    public IntensityDescription damageToTarget;
    public IntensityDescription damageToSelf;
    public IntensityDescription healingToTarget;
    public IntensityDescription healingToSelf;

    public List<HarmfulStatusEffect> harmfulEffectsToTarget = new List<HarmfulStatusEffect>();
    public List<HarmfulStatusEffect> harmfulEffectsToSelf = new List<HarmfulStatusEffect>();
    public List<BeneficialStatusEffect> beneficialEffectsToTarget = new List<BeneficialStatusEffect>();
    public List<BeneficialStatusEffect> beneficialEffectsToSelf = new List<BeneficialStatusEffect>();

    public UsedSpellResult UseSpell(CombatHandler caster, CombatHandler target)
    {
        float _damageToTarget = damageToTarget.flatIntensity
            + damageToTarget.scaleToMagicIntensity * caster.combatStats.magicalDamage
            + damageToTarget.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
        float _damageToSelf = damageToSelf.flatIntensity
            + damageToSelf.scaleToMagicIntensity * caster.combatStats.magicalDamage
            + damageToSelf.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
        float _healingToTarget = healingToTarget.flatIntensity
            + healingToTarget.scaleToMagicIntensity * caster.combatStats.magicalDamage
            + healingToTarget.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
        float _healingToSelf = healingToSelf.flatIntensity
            + healingToSelf.scaleToMagicIntensity * caster.combatStats.magicalDamage
            + healingToSelf.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;

        _damageToTarget = target.CalculateDamageReceived(_damageToTarget);
        _damageToSelf = caster.CalculateDamageReceived(_damageToSelf);
        _healingToTarget = target.CalculateHealingReceived(_healingToTarget);
        _healingToSelf = caster.CalculateHealingReceived(_healingToSelf);
        
        List<HarmfulStatusEffect> _harmfulEffectsToTarget = new List<HarmfulStatusEffect>();
        List<HarmfulStatusEffect> _harmfulEffectsToSelf = new List<HarmfulStatusEffect>();
        List<BeneficialStatusEffect> _beneficialEffectsToTarget = new List<BeneficialStatusEffect>();
        List<BeneficialStatusEffect> _beneficialEffectsToSelf = new List<BeneficialStatusEffect>();
        foreach(HarmfulStatusEffect hse in harmfulEffectsToTarget)
        {
            if (target.TryInflictHarmfulStatusEffect(hse))
                _harmfulEffectsToTarget.Add(hse);
        }
        foreach(HarmfulStatusEffect hse in harmfulEffectsToSelf)
        {
            if (caster.TryInflictHarmfulStatusEffect(hse))
                _harmfulEffectsToSelf.Add(hse);
        }
        foreach(BeneficialStatusEffect bse in beneficialEffectsToTarget)
        {
            if (target.TryInflictBeneficialStatusEffect(bse))
                _beneficialEffectsToTarget.Add(bse);
        }
        foreach(BeneficialStatusEffect bse in beneficialEffectsToSelf)
        {
            if (caster.TryInflictBeneficialStatusEffect(bse))
                _beneficialEffectsToSelf.Add(bse);
        }

        UsedSpellResult usedSpellResult =
            new UsedSpellResult(this, _damageToTarget, _damageToSelf, _healingToTarget, _healingToSelf
                                , _harmfulEffectsToTarget, _harmfulEffectsToSelf
                                , _beneficialEffectsToTarget, _beneficialEffectsToSelf);

        return usedSpellResult;
    }
}
