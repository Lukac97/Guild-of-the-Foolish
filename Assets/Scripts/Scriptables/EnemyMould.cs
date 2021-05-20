using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMould : ScriptableObject
{
    public CreatureType creatureType;
    public Attributes attributesProportion;

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
