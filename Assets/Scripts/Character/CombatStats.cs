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
    public float physicalResistance;
    public float magicalResistance;
    public float avoidChance;

    public void CalculateCombatStats(Attributes attributes)
    {
        float healthChangeRatio = currentHealth > 0 ? maxHealth/currentHealth : 0;
        if (currentHealth == 0 & maxHealth == 0)
            healthChangeRatio = 1;

        float spellResourceChangeRation = currentSpellResource > 0 ? maxSpellResource/currentSpellResource : 0;
        if (currentSpellResource == 0 & maxSpellResource == 0)
            spellResourceChangeRation = 1;
        //maxHealth
        maxHealth = attributes.strength * 10.0f;
        maxHealth += attributes.agility * 6.0f;
        maxHealth += attributes.intellect * 4.0f;

        //maxSpellResource
        maxSpellResource = attributes.intellect * 20.0f;


        //----------------------------------------
        //physicalDmg
        physicalDamage = attributes.strength * 0.8f;
        physicalDamage += attributes.agility * 0.8f;

        //magicalDmg
        magicalDamage = attributes.intellect * 1.1f;

        //physicalRed
        physicalResistance = attributes.strength * 1.5f;

        //magicalRed
        magicalResistance = attributes.intellect * 0.5f;

        //avoidChance
        avoidChance = attributes.agility * 0.3f;
        avoidChance = attributes.luck * 0.7f;

        if (maxHealth > 0 & currentHealth > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth = maxHealth / healthChangeRatio;

        if (maxSpellResource > 0 & currentSpellResource > maxSpellResource)
            currentSpellResource = maxSpellResource;
        else
            currentSpellResource = maxSpellResource / spellResourceChangeRation;

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
