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

    public EnemyMould enemyMould;

    [HideInInspector]
    public List<ItemMould> possibleItemYields;
    public List<ConsumableItemPredefined> possibleConsumableYields;

    private EnemyCombat enemyCombat;
    private void Start()
    {
        enemyCombat = GetComponent<EnemyCombat>();
    }

    public void InitEnemyStats(EnemyMould _enemyMould, int lvl, Location loc)
    {
        enemyMould = _enemyMould;
        enemyName = enemyMould.name;
        location = loc;
        creatureType = enemyMould.creatureType;
        level = lvl;
        enemyAttributes = enemyMould.CalculateAttributesByLevel(lvl, GlobalRules.attributePointsPerLevel);
        possibleItemYields = new List<ItemMould>(enemyMould.possibleItemYields);
        possibleConsumableYields = new List<ConsumableItemPredefined>(enemyMould.possibleConsumableYields);
    }
}
