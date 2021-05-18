using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCombat : MonoBehaviour
{
    [Header("Resources")]
    public int maxHealth;
    public int currentHealth;
    public int maxSpellResource;
    public int currentSpellResource;

    [Header("Combat stats")]
    public CombatStats combatStats;

    [Header("Statuses")]
    public bool isInjured = false;

    private CharStats charStats;
    private CharEquipment charEquipment;

    private void Start()
    {
        charEquipment = GetComponent<CharEquipment>();
        charStats = GetComponent<CharStats>();
    }

    public void UpdateCharCombat()
    {

        CalculateCombatStats();
    }

    public void CalculateCharResources()
    {

    }

    public void CalculateCombatStats()
    {

    }

    #region Resource Management
    public void RaiseHealth(int amount)
    {
        if (currentHealth + amount >= maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }

    public void LowerHealth(int amount)
    {
        if(currentHealth - amount <= 0)
        {
            currentHealth = 0;
            isInjured = true;
        }
        else
        {
            currentHealth -= amount;
        }
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }

    public void RaiseSpellResource(int amount)
    {
        if (currentSpellResource + amount >= maxSpellResource)
            currentSpellResource = maxSpellResource;
        else
            currentSpellResource += amount;
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }

    public void LowerSpellResource(int amount)
    {
        if (currentSpellResource - amount <= 0)
        {
            currentSpellResource = 0;
            isInjured = true;
        }
        else
        {
            currentSpellResource -= amount;
        }
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }
    #endregion Resource Management


}
