using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntensityDescription
{
    public float flatIntensity;
    public float scaleToMagicIntensity;
    public float scaleToPhysicalIntensity;

    public IntensityDescription()
    {
        flatIntensity = 0;
        scaleToMagicIntensity = 0;
        scaleToPhysicalIntensity = 0;
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

    public AppliedIntensityInstance(IntensityInstance intensityInstance, float _intensity, int _originLevel)
    {
        onSelf = intensityInstance.onSelf;
        intensityPurpose = intensityInstance.intensityPurpose;
        primaryIntensityType = intensityInstance.primaryIntensityType;
        secondaryIntensityType = intensityInstance.secondaryIntensityType;
        intensity = _intensity;
        originLevel = _originLevel;
    }

    //public AppliedIntensityInstance(bool _onSelf, IntensityPurpose _intensityPurpose,
    //    PrimaryIntensityType _primaryIntensityType, SecondaryIntensityType _secondaryIntensityType, float _intensity)
    //{
    //    onSelf = _onSelf;
    //    intensityPurpose = _intensityPurpose;
    //    primaryIntensityType = _primaryIntensityType;
    //    secondaryIntensityType = _secondaryIntensityType;
    //    intensity = _intensity;
    //}

    public AppliedIntensityInstance(AppliedIntensityInstance appliedIntensityInstance)
    {
        onSelf = appliedIntensityInstance.onSelf;
        intensityPurpose = appliedIntensityInstance.intensityPurpose;
        primaryIntensityType = appliedIntensityInstance.primaryIntensityType;
        secondaryIntensityType = appliedIntensityInstance.secondaryIntensityType;
        intensity = appliedIntensityInstance.intensity;
    }
}