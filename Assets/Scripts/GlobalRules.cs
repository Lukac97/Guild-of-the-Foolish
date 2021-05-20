using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRules : MonoBehaviour
{
    private static GlobalRules _instance;
    public static GlobalRules Instance { get { return _instance; } }

    public int attributePointsPerLevel;
}
