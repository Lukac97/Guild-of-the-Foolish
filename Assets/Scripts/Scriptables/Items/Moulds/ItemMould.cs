using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMould : ScriptableObject
{
    [System.Serializable]
    public class IconPartWithShadow
    {
        public Sprite spritePart;
        public Sprite spritePartShadow;
        [Space(5)]
        [Header("0-100")]
        public Vector2 colorValueRange;
        [Header("0-100")]
        public Vector2 colorSaturationRange;
    }

    [Header("Name generation")]
    [Tooltip("'Elven' Sword of..., 'Enchanted' Axe of ...")]
    public List<string> itemNamePrefix;
    [Tooltip("Elven 'Sword' of..., Enchanted 'Axe' of ...")]
    public List<string> itemNameType;
    [Tooltip("... of 'Wisdom', ... of 'Integrity'")]
    public List<string> itemNameBonus;

    [Space(10)]

    public string itemDescription;
    [Header("Icon generation")]
    public List<IconPartWithShadow> primarySpriteChoice;
    public List<IconPartWithShadow> secondarySpriteChoice;
    public List<IconPartWithShadow> tertiarySpriteChoice;
}
