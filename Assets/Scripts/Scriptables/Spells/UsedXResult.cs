using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedSpellResult
{
    public CombatSpell spellUsed;

    public float damageToTarget;
    public float damageToSelf;
    public float healingToTarget;
    public float healingToSelf;

    public List<AppliedStatusEffect> harmfulStatusEffectsToTarget;
    public List<AppliedStatusEffect> harmfulStatusEffectsToSelf;
    public List<AppliedStatusEffect> beneficialStatusEffectsToTarget;
    public List<AppliedStatusEffect> beneficialStatusEffectsToSelf;

    public UsedSpellResult()
    {
        spellUsed = null;
        damageToTarget = 0;
        damageToSelf = 0;
        healingToTarget = 0;
        healingToSelf = 0;
        harmfulStatusEffectsToTarget = new List<AppliedStatusEffect>();
        harmfulStatusEffectsToSelf = new List<AppliedStatusEffect>();
        beneficialStatusEffectsToTarget = new List<AppliedStatusEffect>();
        beneficialStatusEffectsToSelf = new List<AppliedStatusEffect>();
    }

    public UsedSpellResult(CombatSpell _spellUsed
                            , float _damageToTarget, float _damageToSelf
                            , float _healingToTarget, float _healingToSelf
                            , List<AppliedStatusEffect> _harmfulStatusEffectsToTarget
                            , List<AppliedStatusEffect> _harmfulStatusEffectsToSelf
                            , List<AppliedStatusEffect> _beneficialStatusEffectsToTarget
                            , List<AppliedStatusEffect> _beneficialStatusEffectsToSelf)
    {
        spellUsed = _spellUsed;
        damageToTarget = _damageToTarget;
        damageToSelf = _damageToSelf;
        healingToTarget = _healingToTarget;
        healingToSelf = _healingToSelf;
        harmfulStatusEffectsToTarget = _harmfulStatusEffectsToTarget;
        harmfulStatusEffectsToSelf = _harmfulStatusEffectsToSelf;
        beneficialStatusEffectsToTarget = _beneficialStatusEffectsToTarget;
        beneficialStatusEffectsToSelf = _beneficialStatusEffectsToSelf;
    }
}

public class AppliedStatusEffect
{
    public StatusEffect statusEffect;
    public float intensityToReceive;
    public int turnsLeft;

    public AppliedStatusEffect()
    {
        statusEffect = null;
        intensityToReceive = 0;
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
        intensityToReceive = 0;
        turnsLeft = _statusEffect.turnDuration;
    }

    public AppliedStatusEffect(StatusEffect _statusEffect, float _intensityToReceive)
    {
        if (_statusEffect.GetType() == typeof(BeneficialStatusEffect))
            statusEffect = new BeneficialStatusEffect((BeneficialStatusEffect)_statusEffect);
        else if (_statusEffect.GetType() == typeof(HarmfulStatusEffect))
            statusEffect = new HarmfulStatusEffect((HarmfulStatusEffect)_statusEffect);
        else
            statusEffect = null;
        intensityToReceive = _intensityToReceive;
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
        intensityToReceive = _appliedStatusEffect.intensityToReceive;
        turnsLeft = _appliedStatusEffect.statusEffect.turnDuration;
    }

}

