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
    public List<IntensityInstance> intensityInstances = new List<IntensityInstance>();

    public List<HarmfulStatusEffect> harmfulEffectsToTarget = new List<HarmfulStatusEffect>();
    public List<HarmfulStatusEffect> harmfulEffectsToSelf = new List<HarmfulStatusEffect>();
    public List<BeneficialStatusEffect> beneficialEffectsToTarget = new List<BeneficialStatusEffect>();
    public List<BeneficialStatusEffect> beneficialEffectsToSelf = new List<BeneficialStatusEffect>();

    public UsedSpellResult UseSpell(CombatHandler caster, CombatHandler target)
    {
        List<AppliedIntensityInstance> _appliedIntensityInstances = new List<AppliedIntensityInstance>();
        int casterLevel = caster.GetLevel();
        foreach(IntensityInstance intInstance in intensityInstances)
        {
            float newIntensity = intInstance.intensityDescription.flatIntensity +
                intInstance.intensityDescription.scaleToMagicIntensity * caster.combatStats.magicalDamage +
                intInstance.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            AppliedIntensityInstance appliedIntensityInstance =
                new AppliedIntensityInstance(intInstance, newIntensity, casterLevel);
            _appliedIntensityInstances.Add(appliedIntensityInstance);
        }
        foreach (AppliedIntensityInstance appliedIntInstance in _appliedIntensityInstances)
        {
            if (appliedIntInstance.onSelf)
            {
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.DAMAGE)
                    appliedIntInstance.intensity = caster.CalculateDamageReceived(appliedIntInstance).intensity;
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.HEAL)
                    appliedIntInstance.intensity = caster.CalculateHealingReceived(appliedIntInstance).intensity;
            }
            else
            {
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.DAMAGE)
                    appliedIntInstance.intensity = target.CalculateDamageReceived(appliedIntInstance).intensity;
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.HEAL)
                    appliedIntInstance.intensity = target.CalculateHealingReceived(appliedIntInstance).intensity;
            }
        }
        
        List<AppliedStatusEffect> _harmfulEffectsToTarget = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _harmfulEffectsToSelf = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _beneficialEffectsToTarget = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _beneficialEffectsToSelf = new List<AppliedStatusEffect>();
        foreach(HarmfulStatusEffect hse in harmfulEffectsToTarget)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(hse);
            float intensityCalc = hse.intensity.intensityDescription.flatIntensity
                + hse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + hse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            newAppliedSE.intensityToReceive = new AppliedIntensityInstance(hse.intensity, intensityCalc, casterLevel);
            if (target.TryInflictHarmfulStatusEffect(newAppliedSE))
                _harmfulEffectsToTarget.Add(newAppliedSE);
        }
        foreach(HarmfulStatusEffect hse in harmfulEffectsToSelf)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(hse);
            float intensityCalc = hse.intensity.intensityDescription.flatIntensity
                + hse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + hse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            newAppliedSE.intensityToReceive = new AppliedIntensityInstance(hse.intensity, intensityCalc, casterLevel);
            if (caster.TryInflictHarmfulStatusEffect(newAppliedSE))
                _harmfulEffectsToSelf.Add(newAppliedSE);
        }
        foreach(BeneficialStatusEffect bse in beneficialEffectsToTarget)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(bse);
            float intensityCalc = bse.intensity.intensityDescription.flatIntensity
                + bse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + bse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            newAppliedSE.intensityToReceive = new AppliedIntensityInstance(bse.intensity, intensityCalc, casterLevel);
            if (target.TryInflictBeneficialStatusEffect(newAppliedSE))
                _beneficialEffectsToTarget.Add(newAppliedSE);
        }
        foreach(BeneficialStatusEffect bse in beneficialEffectsToSelf)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(bse);
            float intensityCalc = bse.intensity.intensityDescription.flatIntensity
                + bse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.magicalDamage
                + bse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.physicalDamage;
            newAppliedSE.intensityToReceive = new AppliedIntensityInstance(bse.intensity, intensityCalc, casterLevel);
            if (caster.TryInflictBeneficialStatusEffect(newAppliedSE))
                _beneficialEffectsToSelf.Add(newAppliedSE);
        }

        UsedSpellResult usedSpellResult =
            new UsedSpellResult(this, _appliedIntensityInstances
                                , _harmfulEffectsToTarget, _harmfulEffectsToSelf
                                , _beneficialEffectsToTarget, _beneficialEffectsToSelf);

        return usedSpellResult;
    }
}
