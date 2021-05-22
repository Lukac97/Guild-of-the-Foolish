using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatStats
{
    [Header("Resources")]
    public float maxHealth;
    public float currentHealth;
    public float maxSpellResource;
    public float currentSpellResource;

    [Space(6)]
    [Header("Stats")]
    public float physicalDamage;
    public float magicalDamage;
    public float physicalReduction;
    public float magicalReduction;
    public float avoidChance;

    public void CalculateCombatStats(Attributes attributes)
    {
        //maxHealth
        maxHealth = attributes.strength * 10.0f;
        maxHealth += attributes.agility * 6.0f;
        maxHealth += attributes.intellect * 4.0f;

        //----------------------------------------
        //physicalDmg
        physicalDamage = attributes.strength * 0.8f;
        physicalDamage += attributes.agility * 0.8f;

        //magicalDmg
        magicalDamage = attributes.intellect * 1.1f;

        //physicalRed
        physicalReduction = attributes.strength * 1.5f;

        //magicalRed
        magicalReduction = attributes.intellect * 0.5f;

        //avoidChance
        avoidChance = attributes.agility * 0.3f;
        avoidChance = attributes.luck * 0.7f;

        ////IntegrityCheck
        //if (currentHealth > maxHealth)
        //    currentHealth = maxHealth;
        //if (currentSpellResource > maxSpellResource)
        //    currentSpellResource = maxSpellResource;
    }
    #region Resource Management
    public void RaiseHealth(float amount)
    {
        if (currentHealth + amount >= maxHealth)
            currentHealth = maxHealth;
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
        if (currentSpellResource + amount >= maxSpellResource)
            currentSpellResource = maxSpellResource;
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
        currentHealth = maxHealth;
        currentSpellResource = maxSpellResource;
    }
    #endregion Resource Management
}
