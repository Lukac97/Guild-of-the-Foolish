using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumableEffect
{
    public int halfCycleDuration;
    public int intensity;

    public ConsumableEffect()
    {
        halfCycleDuration = 0;
        intensity = 0;
    }
}

[System.Serializable]
public class BeneficialConsumableEffect :  ConsumableEffect
{
    public BeneficialConsumableEffectType consumableEffectType;

    public BeneficialConsumableEffect(BeneficialConsumableEffect _beneficialConsumableEffect)
    {
        halfCycleDuration = _beneficialConsumableEffect.halfCycleDuration;
        intensity = _beneficialConsumableEffect.intensity;
        consumableEffectType = _beneficialConsumableEffect.consumableEffectType;
    }
}

[System.Serializable]
public class HarmfulConsumableEffect : ConsumableEffect
{
    public HarmfulConsumableEffectType consumableEffectType;

    public HarmfulConsumableEffect(HarmfulConsumableEffect _harmfulConsumableEffect)
    {
        halfCycleDuration = _harmfulConsumableEffect.halfCycleDuration;
        intensity = _harmfulConsumableEffect.intensity;
        consumableEffectType = _harmfulConsumableEffect.consumableEffectType;
    }
}
