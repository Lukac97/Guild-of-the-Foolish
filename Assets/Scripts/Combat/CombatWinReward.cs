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
        // TODO: Create chances for certain drops in enemy mould

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

            //Equipment drop
            ItemReward tempIR = new ItemReward();
            tempIR.item = GlobalFuncs.GenerateItemFromMould(enemyStats.possibleItemYields[Random.Range(0, enemyStats.possibleItemYields.Count)], enemyStats.level);
            tempIR.quantity = 1;
            items.Add(tempIR);

            //Consumables drop
            ItemReward consumableIR = new ItemReward();
            consumableIR.item = GlobalFuncs.GenerateItemFromPredefined(enemyStats.possibleConsumableYields[Random.Range(0, enemyStats.possibleConsumableYields.Count)]);
            consumableIR.quantity = Random.Range(1, 3);
            items.Add(consumableIR);

            //Ingredients drop
            foreach(IngredientItemPredefined igItem in enemyStats.possibleIngredientYields)
            {
                ItemReward ingredientIR = new ItemReward();
                ingredientIR.item = GlobalFuncs.GenerateItemFromPredefined(igItem);
                ingredientIR.quantity = Random.Range(1, 5);
                items.Add(ingredientIR);
            }

            goldYield = enemyStats.level * 10;
            experienceYield = enemyStats.level * 20;
        }
    }
}
