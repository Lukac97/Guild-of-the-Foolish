using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Enemy Mould")]
public class EnemyMould : ScriptableObject
{
    public CreatureType creatureType;
    public Attributes attributesProportion;
    public List<CombatSpell> combatSpells;
    [Header("Possible drops")]
    public List<ItemMould> possibleItemYields;
    public List<IngredientItemPredefined> possibleIngredientYields;
    public List<ConsumableItemPredefined> possibleConsumableYields;
    public Sprite enemyIcon;

    public Attributes CalculateAttributesByLevel(int level, int attributesPerLevel)
    {
        Attributes totalAttributes = new Attributes();
        int attributePtsToSpend = level * attributesPerLevel;
        if(totalAttributes.ProportionallyAssignPts(attributesProportion, attributePtsToSpend))
        {
            return totalAttributes;
        }
        Debug.Log("Calculating Attributes by level went wrong");
        return null;
    }

}