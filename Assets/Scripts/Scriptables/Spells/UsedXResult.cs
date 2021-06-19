using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedSpellResult
{
    public CombatSpell spellUsed;

    public List<AppliedIntensityInstance> appliedIntensityInstances;

    public List<AppliedStatusEffect> harmfulStatusEffectsToTarget;
    public List<AppliedStatusEffect> harmfulStatusEffectsToSelf;
    public List<AppliedStatusEffect> beneficialStatusEffectsToTarget;
    public List<AppliedStatusEffect> beneficialStatusEffectsToSelf;

    public bool notEnoughSpellResource;

    public UsedSpellResult()
    {
        spellUsed = null;
        appliedIntensityInstances = new List<AppliedIntensityInstance>();
        harmfulStatusEffectsToTarget = new List<AppliedStatusEffect>();
        harmfulStatusEffectsToSelf = new List<AppliedStatusEffect>();
        beneficialStatusEffectsToTarget = new List<AppliedStatusEffect>();
        beneficialStatusEffectsToSelf = new List<AppliedStatusEffect>();
        notEnoughSpellResource = false;
    }

    public UsedSpellResult(CombatSpell _spellUsed
                            , List<AppliedIntensityInstance> _appliedIntensityInstances
                            , List<AppliedStatusEffect> _harmfulStatusEffectsToTarget
                            , List<AppliedStatusEffect> _harmfulStatusEffectsToSelf
                            , List<AppliedStatusEffect> _beneficialStatusEffectsToTarget
                            , List<AppliedStatusEffect> _beneficialStatusEffectsToSelf)
    {
        spellUsed = _spellUsed;
        appliedIntensityInstances = _appliedIntensityInstances;
        harmfulStatusEffectsToTarget = _harmfulStatusEffectsToTarget;
        harmfulStatusEffectsToSelf = _harmfulStatusEffectsToSelf;
        beneficialStatusEffectsToTarget = _beneficialStatusEffectsToTarget;
        beneficialStatusEffectsToSelf = _beneficialStatusEffectsToSelf;
        notEnoughSpellResource = false;
    }
}

public class AppliedStatusEffect
{
    public StatusEffect statusEffect;
    public AppliedIntensityInstance intensityToReceive;
    public int turnsLeft;

    public AppliedStatusEffect()
    {
        statusEffect = null;
        intensityToReceive = null;
        turnsLeft = 0;
    }

    public AppliedStatusEffect(StatusEffect _statusEffect)
    {
        if (_statusEffect.GetType() == typeof(BeneficialStatusEffect))
            statusEffect = new BeneficialStatusEffect((BeneficialStatusEffect)_statusEffect);
        else if (_statusEffect.GetType() == typeof(HarmfulStatusEffect))
            statusEffect = new HarmfulStatusEffect((HarmfulStatusEffect)_statusEffect);
        else
            statusEffect = null;
        intensityToReceive = null;
        turnsLeft = _statusEffect.turnDuration;
    }

    public AppliedStatusEffect(StatusEffect _statusEffect, AppliedIntensityInstance _intensityToReceive)
    {
        if (_statusEffect.GetType() == typeof(BeneficialStatusEffect))
            statusEffect = new BeneficialStatusEffect((BeneficialStatusEffect)_statusEffect);
        else if (_statusEffect.GetType() == typeof(HarmfulStatusEffect))
            statusEffect = new HarmfulStatusEffect((HarmfulStatusEffect)_statusEffect);
        else
            statusEffect = null;
        intensityToReceive = new AppliedIntensityInstance(_intensityToReceive);
        turnsLeft = _statusEffect.turnDuration;
    }

    public AppliedStatusEffect(AppliedStatusEffect _appliedStatusEffect)
    {
        if (_appliedStatusEffect.statusEffect.GetType() == typeof(BeneficialStatusEffect))
            statusEffect = new BeneficialStatusEffect((BeneficialStatusEffect)_appliedStatusEffect.statusEffect);
        else if (_appliedStatusEffect.statusEffect.GetType() == typeof(HarmfulStatusEffect))
            statusEffect = new HarmfulStatusEffect((HarmfulStatusEffect)_appliedStatusEffect.statusEffect);
        else
            statusEffect = null;
        intensityToReceive = new AppliedIntensityInstance(_appliedStatusEffect.intensityToReceive);
        turnsLeft = _appliedStatusEffect.statusEffect.turnDuration;
    }

}

