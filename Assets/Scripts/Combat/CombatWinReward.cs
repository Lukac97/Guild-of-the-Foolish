using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatWinReward
{
    public float experienceYield;
    public float goldYield;
    public List<Item> items;

    public void GenerateYield(CombatHandler enemyHandler, int outcome)
    {
        EnemyStats enemyStats = enemyHandler.GetComponent<EnemyStats>();
        if (outcome != 1)
        {
            items = new List<Item>();
            goldYield = 0;
            experienceYield = 0;
        }
        else
        {
            items = new List<Item>();
            items.Add(enemyStats. possibleItemYields[Random.Range(0, enemyStats.possibleItemYields.Count)]);
            goldYield = enemyStats.level * 10;
            experienceYield = enemyStats.level * 7;
        }
    }
}
