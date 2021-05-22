using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatHandler : MonoBehaviour
{

    [Space(5)]
    [Header("Combat stats")]
    public CombatStats combatStats;

    [Header("Statuses")]
    public bool isInjured = false;

    [Header("Spells")]
    public List<CombatSpell> combatSpells = new List<CombatSpell>();

    protected virtual void NotifyResourcesUpdated()
    {
    }

    public float CalculateDamageReceived(float dmg)
    {
        float finalDmg = dmg;
        //TODO: Calculate damage reduction based on combatStats

        LowerHealth(finalDmg);
        return finalDmg;
    }

    #region Resource Management
    public void RaiseHealth(float amount)
    {
        combatStats.RaiseHealth(amount);
        NotifyResourcesUpdated();
    }

    public void LowerHealth(float amount)
    {
        if (combatStats.LowerHealth(amount))
            isInjured = true;
        NotifyResourcesUpdated();
    }

    public void RaiseSpellResource(float amount)
    {
        combatStats.RaiseSpellResource(amount);
        NotifyResourcesUpdated();
    }

    public void LowerSpellResource(float amount)
    {
        combatStats.LowerSpellResource(amount);
        NotifyResourcesUpdated();
    }

    public void FillResources()
    {
        combatStats.FillResources();
        NotifyResourcesUpdated();
    }
    #endregion Resource Management

}
