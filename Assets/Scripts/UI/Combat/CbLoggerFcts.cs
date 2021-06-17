using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CbLoggerFcts
{
    public static void ColorLogBackground(GameObject logObj, bool isEnemyTurn)
    {
        Image image = logObj.GetComponent<Image>();
        if (isEnemyTurn)
            image.color = GlobalInput.Instance.enemyTurnColor;
        else
            image.color = GlobalInput.Instance.characterTurnColor;
    }

    public static void ColorLogFinishedBackground(GameObject logObj, int outcome)
    {
        Image image = logObj.GetComponent<Image>();
        if (outcome == 1)
            image.color = GlobalInput.Instance.battleWonColor;
        else if (outcome == 0)
            image.color = GlobalInput.Instance.battleLostColor;
        else
            image.color = GlobalInput.Instance.battleTiedColor;
    }

    public static void ColorLogDefault(GameObject logObj)
    {
        Image image = logObj.GetComponent<Image>();
        image.color = GlobalInput.Instance.defaultLogColor;
    }
}
