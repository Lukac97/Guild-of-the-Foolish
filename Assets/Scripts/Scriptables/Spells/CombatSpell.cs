using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Create initial intensity instances
        foreach(IntensityInstance intInstance in intensityInstances)
        {
            if(intInstance.intensityDescription.forStatScaling)
            {
                Debug.Log("Following spell contains an IntensityInstance with a forStatScaling flag, which is designated for stat modification by buffs/debuffs: " + name);
            }
            float newIntensity = intInstance.intensityDescription.flatIntensity * casterLevel +
                intInstance.intensityDescription.scaleToMagicIntensity * caster.combatStats.totalStats.magicalDamage +
                intInstance.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.totalStats.physicalDamage;
            AppliedIntensityInstance appliedIntensityInstance =
                new AppliedIntensityInstance(intInstance, newIntensity, casterLevel,
                    caster.combatStats.totalStats.criticalPotency, caster.combatStats.totalStats.hitPotency, 0, 0, caster);
            _appliedIntensityInstances.Add(appliedIntensityInstance);
        }

        
        //Apply initial intensities to target
        foreach (AppliedIntensityInstance appliedIntInstance in _appliedIntensityInstances)
        {
            if (appliedIntInstance.onSelf)
            {
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.DAMAGE)
                    appliedIntInstance.SetValues(caster.CalculateDamageReceived(appliedIntInstance));
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.HEAL)
                    appliedIntInstance.SetValues(caster.CalculateHealingReceived(appliedIntInstance));
            }
            else
            {
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.DAMAGE)
                    appliedIntInstance.SetValues(target.CalculateDamageReceived(appliedIntInstance));
                if (appliedIntInstance.intensityPurpose == IntensityPurpose.HEAL)
                    appliedIntInstance.SetValues(target.CalculateHealingReceived(appliedIntInstance));
            }
        }
        
        //Status efffects section
        List<AppliedStatusEffect> _harmfulEffectsToTarget = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _harmfulEffectsToSelf = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _beneficialEffectsToTarget = new List<AppliedStatusEffect>();
        List<AppliedStatusEffect> _beneficialEffectsToSelf = new List<AppliedStatusEffect>();

        //Apply Harmful Status Effects to target
        foreach(HarmfulStatusEffect hse in harmfulEffectsToTarget)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(hse);
            float intensityCalc = hse.intensity.intensityDescription.flatIntensity * casterLevel
                + hse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.totalStats.magicalDamage
                + hse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.totalStats.physicalDamage;

            float totalStatMultiplier = hse.intensity.intensityDescription.statMultiplier
                + hse.intensity.intensityDescription.statMultiplierScaleToMagic * caster.combatStats.totalStats.magicalDamage
                + hse.intensity.intensityDescription.statMultiplierScaleToPhysical * caster.combatStats.totalStats.physicalDamage;

            float totalStatFlatIntensity = hse.intensity.intensityDescription.statFlatIntensity * casterLevel;

            newAppliedSE.intensityToReceive =
                new AppliedIntensityInstance(hse.intensity, intensityCalc, casterLevel,
                    caster.combatStats.totalStats.criticalPotency, caster.combatStats.totalStats.hitPotency, totalStatFlatIntensity, totalStatMultiplier, caster);

            if (target.TryInflictHarmfulStatusEffect(newAppliedSE))
                _harmfulEffectsToTarget.Add(newAppliedSE);
        }

        //Apply Harmful Status Effects to self
        foreach (HarmfulStatusEffect hse in harmfulEffectsToSelf)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(hse);
            float intensityCalc = hse.intensity.intensityDescription.flatIntensity * casterLevel
                + hse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.totalStats.magicalDamage
                + hse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.totalStats.physicalDamage;

            float totalStatMultiplier = hse.intensity.intensityDescription.statMultiplier
                + hse.intensity.intensityDescription.statMultiplierScaleToMagic * caster.combatStats.totalStats.magicalDamage
                + hse.intensity.intensityDescription.statMultiplierScaleToPhysical * caster.combatStats.totalStats.physicalDamage;

            float totalStatFlatIntensity = hse.intensity.intensityDescription.statFlatIntensity * casterLevel;

            newAppliedSE.intensityToReceive =
                new AppliedIntensityInstance(hse.intensity, intensityCalc, casterLevel,
                    caster.combatStats.totalStats.criticalPotency, caster.combatStats.totalStats.hitPotency, totalStatFlatIntensity, totalStatMultiplier, caster);

            if (caster.TryInflictHarmfulStatusEffect(newAppliedSE))
                _harmfulEffectsToSelf.Add(newAppliedSE);
        }

        //Apply Beneficial Status Effects to target
        foreach (BeneficialStatusEffect bse in beneficialEffectsToTarget)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(bse);
            float intensityCalc = bse.intensity.intensityDescription.flatIntensity * casterLevel
                + bse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.totalStats.magicalDamage
                + bse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.totalStats.physicalDamage;

            float totalStatMultiplier = bse.intensity.intensityDescription.statMultiplier
                + bse.intensity.intensityDescription.statMultiplierScaleToMagic * caster.combatStats.totalStats.magicalDamage
                + bse.intensity.intensityDescription.statMultiplierScaleToPhysical * caster.combatStats.totalStats.physicalDamage;

            float totalStatFlatIntensity = bse.intensity.intensityDescription.statFlatIntensity * casterLevel;

            newAppliedSE.intensityToReceive =
                new AppliedIntensityInstance(bse.intensity, intensityCalc, casterLevel,
                    caster.combatStats.totalStats.criticalPotency, caster.combatStats.totalStats.hitPotency, totalStatFlatIntensity, totalStatMultiplier, caster);

            if (target.TryInflictBeneficialStatusEffect(newAppliedSE))
                _beneficialEffectsToTarget.Add(newAppliedSE);
        }

        //Apply Beneficial Status Effects to self
        foreach (BeneficialStatusEffect bse in beneficialEffectsToSelf)
        {
            AppliedStatusEffect newAppliedSE = new AppliedStatusEffect(bse);
            float intensityCalc = bse.intensity.intensityDescription.flatIntensity * casterLevel
                + bse.intensity.intensityDescription.scaleToMagicIntensity * caster.combatStats.totalStats.magicalDamage
                + bse.intensity.intensityDescription.scaleToPhysicalIntensity * caster.combatStats.totalStats.physicalDamage;

            float totalStatMultiplier = bse.intensity.intensityDescription.statMultiplier
                + bse.intensity.intensityDescription.statMultiplierScaleToMagic * caster.combatStats.totalStats.magicalDamage
                + bse.intensity.intensityDescription.statMultiplierScaleToPhysical * caster.combatStats.totalStats.physicalDamage;

            float totalStatFlatIntensity = bse.intensity.intensityDescription.statFlatIntensity * casterLevel;

            newAppliedSE.intensityToReceive =
                new AppliedIntensityInstance(bse.intensity, intensityCalc, casterLevel,
                    caster.combatStats.totalStats.criticalPotency, caster.combatStats.totalStats.hitPotency, totalStatFlatIntensity, totalStatMultiplier, caster);

            if (caster.TryInflictBeneficialStatusEffect(newAppliedSE))
                _beneficialEffectsToSelf.Add(newAppliedSE);
        }

        //Generate Used Spell Result
        UsedSpellResult usedSpellResult =
            new UsedSpellResult(this, _appliedIntensityInstances
                                , _harmfulEffectsToTarget, _harmfulEffectsToSelf
                                , _beneficialEffectsToTarget, _beneficialEffectsToSelf);

        return usedSpellResult;
    }
}
