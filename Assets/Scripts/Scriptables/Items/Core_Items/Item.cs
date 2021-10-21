using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemID;
    [Space(15)]
    public string itemName;
    public string itemDescription;
    public List<IconPartWithShadow> itemIcon;
    public int level;

    public bool isMoulded = false;

}
