using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string enemyName;
    public CreatureType creatureType;
    public Location location;
    [Space(5)]
    public int level;
    public Attributes enemyAttributes = new Attributes();

    private EnemyCombat enemyCombat;
    private void Start()
    {
        enemyCombat = GetComponent<EnemyCombat>();
    }

    public void InitEnemyStats(EnemyMould enemyMould, int lvl, Location loc)
    {
        enemyName = enemyMould.name;
        location = loc;
        creatureType = enemyMould.creatureType;
        level = lvl;
        enemyAttributes = enemyMould.CalculateAttributesByLevel(lvl, GlobalRules.Instance.attributePointsPerLevel);
    }
}
