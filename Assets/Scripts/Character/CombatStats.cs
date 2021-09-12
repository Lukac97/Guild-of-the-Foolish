using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatStats
{
    [System.Serializable]
    public class StatFields
    {
        [Header("Resources")]
        public float maxHealth;
        public float maxSpellResource;
        [Header("Stats")]
        public float physicalDamage;
        public float magicalDamage;
        public float physicalResistance;
        public float magicalResistance;
        public float avoidPotency;
        public float criticalPotency;
        public float hitPotency;

        public StatFields(float initNr)
        {
            maxHealth = initNr;
            maxSpellResource = initNr;
            physicalDamage = initNr;
            magicalDamage = initNr;
            physicalResistance = initNr;
            magicalResistance = initNr;
            avoidPotency = initNr;
            criticalPotency = initNr;
            hitPotency = initNr;
        }
    }

    [Header("Resource current state")]
    public float currentHealth;
    public float currentSpellResource;

    [Space(6)]
    [Header("Stats")]
    public StatFields baseStats;

    public StatFields tmpStats;

    public StatFields totalStats;

    public void CalculateCombatStats(Attributes attributes)
    {
        float healthChangeRatio = currentHealth > 0 ? baseStats.maxHealth / currentHealth : 0;
        if (currentHealth == 0 & baseStats.maxHealth == 0)
            healthChangeRatio = 1;

        float spellResourceChangeRatio = currentSpellResource > 0 ? baseStats.maxSpellResource /currentSpellResource : 0;
        if (currentSpellResource == 0 & baseStats.maxSpellResource == 0)
            spellResourceChangeRatio = 1;

        //Initialize
        baseStats.maxHealth = 0;
        baseStats.maxSpellResource = 0;

        baseStats.physicalDamage = 0;
        baseStats.magicalDamage = 0;
        baseStats.physicalResistance = 0;
        baseStats.magicalResistance = 0;
        baseStats.avoidPotency = 0;
        baseStats.criticalPotency = 0;
        baseStats.hitPotency = 0;
        //maxHealth
        baseStats.maxHealth += Mathf.Max(0, attributes.strength) * 10.0f;
        baseStats.maxHealth += Mathf.Max(0, attributes.agility) * 6.0f;
        baseStats.maxHealth += Mathf.Max(0, attributes.intellect) * 4.0f;

        //maxSpellResource
        baseStats.maxSpellResource += Mathf.Max(0, attributes.intellect) * 20.0f;


        //----------------------------------------
        //physicalDmg
        baseStats.physicalDamage += Mathf.Max(0, attributes.strength) * 0.8f;
        baseStats.physicalDamage += Mathf.Max(0, attributes.agility) * 0.8f;

        //magicalDmg
        baseStats.magicalDamage += Mathf.Max(0, attributes.intellect) * 1.1f;

        //physicalRed
        baseStats.physicalResistance += Mathf.Max(0, attributes.strength) * 1.5f;

        //magicalRed
        baseStats.magicalResistance += Mathf.Max(0, attributes.intellect) * 0.5f;

        //avoidPotency
        baseStats.avoidPotency += Mathf.Max(0, attributes.agility) * 0.3f;
        baseStats.avoidPotency += Mathf.Max(0, attributes.luck) * 0.7f;

        //criticalPotency
        baseStats.criticalPotency += Mathf.Max(0, attributes.agility) * 0.7f;
        baseStats.criticalPotency += Mathf.Max(0, attributes.luck) * 0.3f;

        //criticalPotency
        baseStats.hitPotency += Mathf.Max(0, attributes.agility) * 0.5f;
        baseStats.hitPotency += Mathf.Max(0, attributes.luck) * 0.5f;

        if (baseStats.maxHealth > 0 & currentHealth > baseStats.maxHealth)
            currentHealth = baseStats.maxHealth;
        else
            currentHealth = baseStats.maxHealth / healthChangeRatio;

        if (baseStats.maxSpellResource > 0 & currentSpellResource > baseStats.maxSpellResource)
            currentSpellResource = baseStats.maxSpellResource;
        else
            currentSpellResource = baseStats.maxSpellResource / spellResourceChangeRatio;

        CalculateTotalCombatStats();
    }

    private void CalculateTotalCombatStats()
    {
        float healthChangeRatio = currentHealth > 0 ? baseStats.maxHealth / currentHealth : 0;
        if (currentHealth == 0 & baseStats.maxHealth == 0)
            healthChangeRatio = 1;

        float spellResourceChangeRatio = currentSpellResource > 0 ? baseStats.maxSpellResource / currentSpellResource : 0;
        if (currentSpellResource == 0 & baseStats.maxSpellResource == 0)
            spellResourceChangeRatio = 1;

        totalStats.maxHealth = baseStats.maxHealth + tmpStats.maxHealth;
        totalStats.maxSpellResource = baseStats.maxSpellResource + tmpStats.maxSpellResource;
        totalStats.physicalDamage = baseStats.physicalDamage + tmpStats.physicalDamage;
        totalStats.magicalDamage = baseStats.magicalDamage + tmpStats.magicalDamage;
        totalStats.physicalResistance = baseStats.physicalResistance + tmpStats.physicalResistance;
        totalStats.magicalResistance = baseStats.magicalResistance + tmpStats.magicalResistance;
        totalStats.avoidPotency = baseStats.avoidPotency + tmpStats.avoidPotency;
        totalStats.criticalPotency = baseStats.criticalPotency + tmpStats.criticalPotency;
        totalStats.hitPotency = baseStats.hitPotency + tmpStats.hitPotency;

        if (totalStats.maxHealth > 0 & currentHealth > totalStats.maxHealth)
            currentHealth = totalStats.maxHealth;
        else
            currentHealth = totalStats.maxHealth / healthChangeRatio;

        if (totalStats.maxSpellResource > 0 & currentSpellResource > totalStats.maxSpellResource)
            currentSpellResource = totalStats.maxSpellResource;
        else
            currentSpellResource = totalStats.maxSpellResource / spellResourceChangeRatio;
    }

    public void SetTmpCombatStats(StatFields _tmpStatsFlat, StatFields _tmpStatsMultiplier)
    {
        tmpStats.maxHealth = (baseStats.maxHealth + _tmpStatsFlat.maxHealth) * _tmpStatsMultiplier.maxHealth - baseStats.maxHealth;
        tmpStats.maxSpellResource = (baseStats.maxSpellResource + _tmpStatsFlat.maxSpellResource) * _tmpStatsMultiplier.maxSpellResource - baseStats.maxSpellResource;
        tmpStats.physicalDamage = (baseStats.physicalDamage + _tmpStatsFlat.physicalDamage) * _tmpStatsMultiplier.physicalDamage - baseStats.physicalDamage;
        tmpStats.magicalDamage = (baseStats.magicalDamage + _tmpStatsFlat.magicalDamage) * _tmpStatsMultiplier.magicalDamage - baseStats.magicalDamage;
        tmpStats.physicalResistance = (baseStats.physicalResistance + _tmpStatsFlat.physicalResistance) * _tmpStatsMultiplier.physicalResistance - baseStats.physicalResistance;
        tmpStats.magicalResistance = (baseStats.magicalResistance + _tmpStatsFlat.magicalResistance) * _tmpStatsMultiplier.magicalResistance - baseStats.magicalResistance;
        tmpStats.avoidPotency = (baseStats.avoidPotency + _tmpStatsFlat.avoidPotency) * _tmpStatsMultiplier.avoidPotency - baseStats.avoidPotency;
        tmpStats.criticalPotency = (baseStats.criticalPotency + _tmpStatsFlat.criticalPotency) * _tmpStatsMultiplier.criticalPotency - baseStats.criticalPotency;
        tmpStats.hitPotency = (baseStats.hitPotency + _tmpStatsFlat.hitPotency) * _tmpStatsMultiplier.hitPotency - baseStats.hitPotency;

        CalculateTotalCombatStats();
    }

    #region Resource Management
    public void RaiseHealth(float amount)
    {
        if (currentHealth + amount >= totalStats.maxHealth)
            currentHealth = totalStats.maxHealth;
        else
            currentHealth += amount;
    }

    public bool LowerHealth(float amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
            return true;
        }
        else
        {
            currentHealth -= amount;
            return false;
        }
    }

    public void RaiseSpellResource(float amount)
    {
        if (currentSpellResource + amount >= totalStats.maxSpellResource)
            currentSpellResource = totalStats.maxSpellResource;
        else
            currentSpellResource += amount;
    }

    public void LowerSpellResource(float amount)
    {
        if (currentSpellResource - amount <= 0)
        {
            currentSpellResource = 0;
        }
        else
        {
            currentSpellResource -= amount;
        }
    }

    public void FillResources()
    {
        currentHealth = totalStats.maxHealth;
        currentSpellResource = totalStats.maxSpellResource;
    }
    #endregion Resource Management
}
