using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class Attributes
{
    public int strength;
    public int agility;
    public int luck;
    public int intellect;

    public static Attributes operator + (Attributes a, Attributes b)
    {
        Attributes res = new Attributes(a);
        res.strength += b.strength;
        res.agility += b.agility;
        res.luck += b.luck;
        res.intellect += b.intellect;

        return res;
    }
    public static Attributes operator -(Attributes a, Attributes b)
    {
        Attributes res = new Attributes(a);
        res.strength -= b.strength;
        res.agility -= b.agility;
        res.luck -= b.luck;
        res.intellect -= b.intellect;

        return res;
    }

    public Attributes()
    {
        strength = 0;
        agility = 0;
        luck = 0;
        intellect = 0;
    }


    public Attributes(Attributes _attributes)
    {
        strength = _attributes.strength;
        agility = _attributes.agility;
        luck = _attributes.luck;
        intellect = _attributes.intellect;
    }

    public Dictionary<string, int> GetAllFields()
    {
        Dictionary<string, int> dictNames = new Dictionary<string, int>();
        foreach (FieldInfo field in GetType().GetFields())
        {
            dictNames.Add(field.Name, (int)field.GetValue(this));
        }
        return dictNames;
    }

    public int SumOfAllAttributes()
    {
        return strength + agility + intellect + luck;
    }

    private int FindLowestProportion(Attributes proportion)
    {
        int lowestProportion = -1;

        if (lowestProportion == -1 | proportion.strength < lowestProportion)
            lowestProportion = proportion.strength;
        if (lowestProportion == -1 | proportion.agility < lowestProportion)
            lowestProportion = proportion.agility;
        if (lowestProportion == -1 | proportion.intellect < lowestProportion)
            lowestProportion = proportion.intellect;
        if (lowestProportion == -1 | proportion.luck < lowestProportion)
            lowestProportion = proportion.luck;
        return lowestProportion;
    }

    private int FindHighestProportion(Attributes proportion)
    {
        int highestProportion = -1;

        if (proportion.strength > 0)
        {
            if (proportion.strength > highestProportion)
                highestProportion = proportion.strength;
        }
        if (proportion.agility > 0)
        {
            if (proportion.agility > highestProportion)
                highestProportion = proportion.agility;
        }
        if (proportion.intellect > 0)
        {
            if (proportion.intellect > highestProportion)
                highestProportion = proportion.intellect;
        }
        if (proportion.luck > 0)
        {
            if (proportion.luck > highestProportion)
                highestProportion = proportion.luck;
        }
        return highestProportion;
    }

    public bool ProportionallyAssignPts(Attributes proportion, int numberOfPts)
    {
        Attributes attrs = new Attributes();
        int sumProp = proportion.SumOfAllAttributes();
        if (sumProp <= 0)
            return false;

        attrs.strength = Mathf.RoundToInt(1.0f * (proportion.strength * numberOfPts) / sumProp);
        attrs.agility = Mathf.RoundToInt(1.0f * (proportion.agility * numberOfPts) / sumProp);
        attrs.intellect = Mathf.RoundToInt(1.0f * (proportion.intellect * numberOfPts) / sumProp);
        attrs.luck = Mathf.RoundToInt(1.0f * (proportion.luck * numberOfPts) / sumProp);

        int availablePts = numberOfPts - attrs.SumOfAllAttributes();

        int highPrio = FindHighestProportion(proportion);
        if(proportion.strength == highPrio)
        {
            proportion.strength += availablePts;
        }
        else if (proportion.agility == highPrio)
        {
            proportion.agility += availablePts;
        }
        else if (proportion.intellect == highPrio)
        {
            proportion.intellect += availablePts;
        }
        else if (proportion.luck == highPrio)
        {
            proportion.luck += availablePts;
        }
        else
        {
            return false;
        }

        strength = attrs.strength;
        agility = attrs.agility;
        intellect = attrs.intellect;
        luck = attrs.luck;
        return true;
    }
}
