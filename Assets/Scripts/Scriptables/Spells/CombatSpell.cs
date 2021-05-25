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
        
        List<AppliedStatusEffect> _harmfulEffectsToTarget = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _harmfulEffectsToSelf = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _beneficialEffectsToTarget = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _beneficialEffectsToSelf = new List<AppliedStatusEffect>();
        foreach(HarmfulStatusEffect hse in harmfulEffectsToTarget)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(hse);
            newAppliedSE.intensityToReceive = hse.intensity.flatIntensity
                + hse.intensity.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + hse.intensity.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            if (target.TryInflictHarmfulStatusEffect(newAppliedSE))
                _harmfulEffectsToTarget.Add(newAppliedSE);
        }
        foreach(HarmfulStatusEffect hse in harmfulEffectsToSelf)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(hse);
            newAppliedSE.intensityToReceive = hse.intensity.flatIntensity
                + hse.intensity.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + hse.intensity.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            if (caster.TryInflictHarmfulStatusEffect(newAppliedSE))
                _harmfulEffectsToSelf.Add(newAppliedSE);
        }
        foreach(BeneficialStatusEffect bse in beneficialEffectsToTarget)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(bse);
            newAppliedSE.intensityToReceive = bse.intensity.flatIntensity
                + bse.intensity.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + bse.intensity.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            if (target.TryInflictBeneficialStatusEffect(newAppliedSE))
                _beneficialEffectsToTarget.Add(newAppliedSE);
        }
        foreach(BeneficialStatusEffect bse in beneficialEffectsToSelf)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(bse);
            newAppliedSE.intensityToReceive = bse.intensity.flatIntensity
                + bse.intensity.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + bse.intensity.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            if (caster.TryInflictBeneficialStatusEffect(newAppliedSE))
                _beneficialEffectsToSelf.Add(newAppliedSE);
        }

        UsedSpellResult usedSpellResult =
            new UsedSpellResult(this, _damageToTarget, _damageToSelf, _healingToTarget, _healingToSelf
                                , _harmfulEffectsToTarget, _harmfulEffectsToSelf
                                , _beneficialEffectsToTarget, _beneficialEffectsToSelf);

        return usedSpellResult;
    }
}
