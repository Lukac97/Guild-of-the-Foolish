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

    [SerializeField]
    private TextMeshProUGUI statCmpVal;


    public void SetText(string _statName, int _statValue, string postfix = "", int _statChange = 0)
    {
        statName.text = _statName;
        statValue.text = _statValue.ToString() + postfix;
        if (statCmpVal != null)
        {
            int statChange = _statValue - _statChange;
            statCmpVal.text = "(" + (statChange <= 0 ? "" : "+") + statChange.ToString() + ")";
            if (statChange > 0)
                ColorAllText(GlobalInput.Instance.goodColor);
            else if (statChange < 0)
                ColorAllText(GlobalInput.Instance.badColor);
            else
                ColorAllText(Color.white);
        }
        else
        {
            ColorAllText(Color.white);
        }
    }

    public void ColorAllText(Color colorToSet)
    {
        statName.color = colorToSet;
        statValue.color = colorToSet;
        if(statCmpVal != null)
            statCmpVal.color = colorToSet;
    }
}
