using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCombat : MonoBehaviour
{
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
        combatStats.RaiseHealth(amount);
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }

    public void LowerHealth(int amount)
    {
        combatStats.LowerHealth(amount);
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }

    public void RaiseSpellResource(int amount)
    {
        combatStats.RaiseSpellResource(amount);
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }

    public void LowerSpellResource(int amount)
    {
        combatStats.LowerSpellResource(amount);
        CharactersController.Instance.CharactersResourcesUpdated.Invoke();
    }
    #endregion Resource Management


}
