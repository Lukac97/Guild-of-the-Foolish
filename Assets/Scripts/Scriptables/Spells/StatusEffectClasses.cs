using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{
    public int turnDuration;
    [Tooltip("Values for percantages are between 0-1")]
    public IntensityInstance intensity;

    public StatusEffect()
    {
        turnDuration = 0;
        intensity = new IntensityInstance();
    }
}

[System.Serializable]
public class HarmfulStatusEffect : StatusEffect
{
    public HarmfulStatusEffectType statusEffectType;

    public HarmfulStatusEffect(HarmfulStatusEffect statusEffect)
    {
        turnDuration = statusEffect.turnDuration;
        intensity = statusEffect.intensity;
        statusEffectType = statusEffect.statusEffectType;
    }
}

[System.Serializable]
public class BeneficialStatusEffect : StatusEffect
{
    public BeneficialStatusEffectType statusEffectType;

    public BeneficialStatusEffect(BeneficialStatusEffect statusEffect)
    {
        turnDuration = statusEffect.turnDuration;
        intensity = statusEffect.intensity;
        statusEffectType = statusEffect.statusEffectType;
    }
}
