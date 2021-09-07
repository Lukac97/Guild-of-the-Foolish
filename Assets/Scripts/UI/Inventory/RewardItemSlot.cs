using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardItemSlot : MonoBehaviour
{

    public ItemIconHandler icon;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI level;

    public void AssignItemReward(CombatWinReward.ItemReward iReward)
    {
        if (iReward == null)
        {
            icon.IconSetActive(false);
            quantity.text = "";
            level.text = "";
        }
        else
        {
            icon.IconSetActive(true);
            icon.InitItemIconHandler(iReward.item);
            if (iReward.quantity == 1)
                quantity.text = "";
            else
                quantity.text = iReward.quantity.ToString();
            level.text = iReward.item.level.ToString();
        }
    }
    
    public void AssignGoldReward(float amount)
    {
        icon.IconSetActive(false);
        quantity.text = amount.ToString();
        level.text = "";
    }

    public void AssignExperienceReward(float amount)
    {
        icon.IconSetActive(false);
        quantity.text = amount.ToString();
        level.text = "";
    }
}
