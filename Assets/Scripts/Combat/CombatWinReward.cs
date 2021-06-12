using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatWinReward
{
    public class ItemReward
    {
        public Item item;
        public int quantity;

    }

    public float experienceYield;
    public float goldYield;
    public List<ItemReward> items;

    public void GenerateYield(CombatHandler enemyHandler, int outcome)
    {
        EnemyStats enemyStats = enemyHandler.GetComponent<EnemyStats>();
        if (outcome != 1)
        {
            items = new List<ItemReward>();
            goldYield = 0;
            experienceYield = 0;
        }
        else
        {
            items = new List<ItemReward>();
            ItemReward tempIR = new ItemReward();
            tempIR.item = enemyStats.possibleItemYields[Random.Range(0, enemyStats.possibleItemYields.Count)];
            tempIR.quantity = 1;
            items.Add(tempIR);
            goldYield = enemyStats.level * 10;
            experienceYield = enemyStats.level * 20;
        }
    }
}
