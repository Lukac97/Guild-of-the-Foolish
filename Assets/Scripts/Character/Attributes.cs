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
}
