using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildMain : MonoBehaviour
{
    private static GuildMain _instance;
    public static GuildMain Instance
    {
        get
        {
            return _instance;
        }
    }

    public delegate void GuildResourceChangedDelegate();
    public GuildResourceChangedDelegate GuildResourceChanged;

    public float gold;

    private void Awake()
    {
        if(Instance == null)
        {
            _instance = this;
        }
    }

    public bool SubtractGold(float amount)
    {
        if(amount > gold)
        {
            return false;
        }
        else
        {
            gold -= amount;
            GuildResourceChanged.Invoke();
            return true;
        }
    }
    public bool AddGold(float amount)
    {
        if (amount > gold)
        {
            return false;
        }
        else
        {
            gold += amount;
            GuildResourceChanged.Invoke();
            return true;
        }
    }
}
