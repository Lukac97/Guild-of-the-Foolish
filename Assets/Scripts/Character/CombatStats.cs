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
        maxHealth = attributes.strength * 1.0f;
        maxHealth += attributes.agility * 0.6f;
        maxHealth += attributes.intellect * 0.3f;

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
    }
    #region Resource Management
    public void RaiseHealth(int amount)
    {
        if (currentHealth + amount >= maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;
    }

    public void LowerHealth(int amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
    }

    public void RaiseSpellResource(int amount)
    {
        if (currentSpellResource + amount >= maxSpellResource)
            currentSpellResource = maxSpellResource;
        else
            currentSpellResource += amount;
    }

    public void LowerSpellResource(int amount)
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
    #endregion Resource Management
}
