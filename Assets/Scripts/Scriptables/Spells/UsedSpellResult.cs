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

    public List<HarmfulStatusEffect> harmfulStatusEffectsToTarget;
    public List<HarmfulStatusEffect> harmfulStatusEffectsToSelf;
    public List<BeneficialStatusEffect> beneficialStatusEffectsToTarget;
    public List<BeneficialStatusEffect> beneficialStatusEffectsToSelf;

    public UsedSpellResult()
    {
        spellUsed = null;
        damageToTarget = 0;
        damageToSelf = 0;
        healingToTarget = 0;
        healingToSelf = 0;
        harmfulStatusEffectsToTarget = new List<HarmfulStatusEffect>();
        harmfulStatusEffectsToSelf = new List<HarmfulStatusEffect>();
        beneficialStatusEffectsToTarget = new List<BeneficialStatusEffect>();
        beneficialStatusEffectsToSelf = new List<BeneficialStatusEffect>();
    }

    public UsedSpellResult(CombatSpell _spellUsed
                            , float _damageToTarget, float _damageToSelf
                            , float _healingToTarget, float _healingToSelf
                            , List<HarmfulStatusEffect> _harmfulStatusEffectsToTarget
                            , List<HarmfulStatusEffect> _harmfulStatusEffectsToSelf
                            , List<BeneficialStatusEffect> _beneficialStatusEffectsToTarget
                            , List<BeneficialStatusEffect> _beneficialStatusEffectsToSelf)
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
