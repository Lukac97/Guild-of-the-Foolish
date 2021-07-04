using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingleEqStat : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statName;
    [SerializeField]
    private TextMeshProUGUI statValue;

    public void SetText(string _statName, int _statValue, string postfix = "")
    {
        statName.text = _statName;
        statValue.text = _statValue.ToString() + postfix;
    }
}
