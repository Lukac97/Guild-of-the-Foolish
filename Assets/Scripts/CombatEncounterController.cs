using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounterController : MonoBehaviour
{
    private static CombatEncounterController _instance;
    public static CombatEncounterController Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject combatEncounterPrefab;
    public GameObject combatEncountersPanel;

    public List<CombatEncounter> combatEncounters;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        combatEncounters = new List<CombatEncounter>();
    }

    private void Start()
    {
        ClearEncounters();
    }

    public bool CreateEncounter(CharStats charStats, Location.PossibleEnemy enemyFromLoc, Location loc)
    {
        if (charStats.location != loc)
            return false;
        GameObject gO = Instantiate(combatEncounterPrefab, combatEncountersPanel.transform);
        gO.GetComponent<CombatEncounter>().InitiateCombat(charStats, enemyFromLoc, loc);
        return true;
    }

    public void DestroyEncounter(CombatEncounter cbEnc)
    {
        combatEncounters.Remove(cbEnc);
        Destroy(cbEnc.gameObject);
    }

    public void ClearEncounters()
    {
        foreach(Transform child in combatEncountersPanel.transform)
        {
            Destroy(child.gameObject);
        }
        combatEncounters.Clear();
    }
}
