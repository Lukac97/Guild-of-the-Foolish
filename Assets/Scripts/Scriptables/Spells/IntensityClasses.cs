using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntensityDescription
{
    
    [Header("For intensity calculation")]

    [Tooltip("This value represents flat value per level which is added to the total intensity. (Eg.: total_intensity = flat_intensity * level).")]
    public float flatIntensity = 0;

    [Tooltip("This value represents value with which the magic intensity is multiplied and added to total intensity.")]
    public float scaleToMagicIntensity = 0;

    [Tooltip("This value represents value with which the physical intensity is multiplied and added to total intensity.")]
    public float scaleToPhysicalIntensity = 0;

    [Space(5)]
    [Header("For stat scaling (Buffs/Debuffs)")]

    [Tooltip("Enable if this effect affects stats of target (Buffs/Debuffs).")]
    public bool forStatScaling = false;

    [Tooltip("This value represents flat value per level to add to target stat")]
    public float statFlatIntensity = 0;

    [Tooltip("This value represents base value for multiplication(division) of target stat")]
    public float statMultiplier = 0;

    [Tooltip("This value represents +x on stat multiplier per MagicIntensity point")]
    public float statMultiplierScaleToMagic = 0;

    [Tooltip("This value represents +x on stat multiplier per PhysicalIntensity point")]
    public float statMultiplierScaleToPhysical = 0;


    public IntensityDescription()
    {
        flatIntensity = 0;
        scaleToMagicIntensity = 0;
        scaleToPhysicalIntensity = 0;

        forStatScaling = false;
        statFlatIntensity = 0;
        statMultiplier = 0;
        statMultiplierScaleToMagic = 0;
        statMultiplierScaleToPhysical = 0;
    }
}

[System.Serializable]
public class IntensityInstance
{
    public bool onSelf;
    public IntensityPurpose intensityPurpose;
    public PrimaryIntensityType primaryIntensityType;
    public SecondaryIntensityType secondaryIntensityType;
    public IntensityDescription intensityDescription;
}

[System.Serializable]
public class AppliedIntensityInstance
{
    public bool onSelf;
    public IntensityPurpose intensityPurpose;
    public PrimaryIntensityType primaryIntensityType;
    public SecondaryIntensityType secondaryIntensityType;
    public float intensity;
    public int originLevel;
    public float originCriticalPotency;
    public float originHitPotency;

    public float statFlatIntensity;
    public float statMultiplier;

    public bool hasBeenAvoided;
    public bool isCritical;

    public bool castWithTruestrike;
    public bool castWithPrecise;
    public bool castWithBlind;
    public bool castWithDazed;

    public AppliedIntensityInstance(IntensityInstance intensityInstance, float _intensity,
        int _originLevel, float _originCriticalPotency, float _originHitPotency, float _statFlatIntensity, float _statMultiplier,
        CombatHandler casterOfThisIntensity)
    {
        onSelf = intensityInstance.onSelf;
        intensityPurpose = intensityInstance.intensityPurpose;
        primaryIntensityType = intensityInstance.primaryIntensityType;
        secondaryIntensityType = intensityInstance.secondaryIntensityType;
        intensity = _intensity;
        originLevel = _originLevel;
        originCriticalPotency = _originCriticalPotency;
        originHitPotency = _originHitPotency;
        statFlatIntensity = _statFlatIntensity;
        statMultiplier = _statMultiplier;
        hasBeenAvoided = false;
        isCritical = false;

        castWithTruestrike = casterOfThisIntensity.hasTruestrike;
        castWithPrecise = casterOfThisIntensity.isPrecise;
        castWithBlind = casterOfThisIntensity.isBlind;
        castWithDazed = casterOfThisIntensity.isDazed;
    }

    public AppliedIntensityInstance(AppliedIntensityInstance appliedIntensityInstance)
    {
        onSelf = appliedIntensityInstance.onSelf;
        intensityPurpose = appliedIntensityInstance.intensityPurpose;
        primaryIntensityType = appliedIntensityInstance.primaryIntensityType;
        secondaryIntensityType = appliedIntensityInstance.secondaryIntensityType;
        intensity = appliedIntensityInstance.intensity;
        originCriticalPotency = appliedIntensityInstance.originCriticalPotency;
        originHitPotency = appliedIntensityInstance.originHitPotency;
        statFlatIntensity = appliedIntensityInstance.statFlatIntensity;
        statMultiplier = appliedIntensityInstance.statMultiplier;
        hasBeenAvoided = appliedIntensityInstance.hasBeenAvoided;
        isCritical = appliedIntensityInstance.isCritical;

        castWithTruestrike = appliedIntensityInstance.castWithTruestrike;
        castWithPrecise = appliedIntensityInstance.castWithPrecise;
        castWithBlind = appliedIntensityInstance.castWithBlind;
        castWithDazed = appliedIntensityInstance.castWithDazed;
    }

    public void SetValues(AppliedIntensityInstance appliedIntensityInstance)
    {
        onSelf = appliedIntensityInstance.onSelf;
        intensityPurpose = appliedIntensityInstance.intensityPurpose;
        primaryIntensityType = appliedIntensityInstance.primaryIntensityType;
        secondaryIntensityType = appliedIntensityInstance.secondaryIntensityType;
        intensity = appliedIntensityInstance.intensity;
        originCriticalPotency = appliedIntensityInstance.originCriticalPotency;
        originHitPotency = appliedIntensityInstance.originHitPotency;
        statFlatIntensity = appliedIntensityInstance.statFlatIntensity;
        statMultiplier = appliedIntensityInstance.statMultiplier;
        hasBeenAvoided = appliedIntensityInstance.hasBeenAvoided;
        isCritical = appliedIntensityInstance.isCritical;

        castWithTruestrike = appliedIntensityInstance.castWithTruestrike;
        castWithPrecise = appliedIntensityInstance.castWithPrecise;
        castWithBlind = appliedIntensityInstance.castWithBlind;
        castWithDazed = appliedIntensityInstance.castWithDazed;
    }
}