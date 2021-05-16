using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CmpStat : MonoBehaviour
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI valueChange;

    public void SetText(string name, int value, string postfix = "")
    {
        statName.text = name;
        if (value > 0)
        {
            valueChange.color = GlobalInput.Instance.goodColor;
            valueChange.text = "+" + value.ToString() + postfix;
        }
        else
        {
            valueChange.color = GlobalInput.Instance.badColor;
            valueChange.text = value.ToString() + postfix;
        }
    }
}
