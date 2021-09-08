using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMould : ScriptableObject
{
    [System.Serializable]
    public class IconMouldWithShadow
    {
        public Sprite spritePart;
        public Sprite spritePartShadow;
        [Space(5)]
        [Header("0-1")]
        public float colorValueMin;
        public float colorValueMax;
        [Header("0-1")]
        public float colorSaturationRangeMin;
        public float colorSaturationRangeMax;
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
    public List<IconMouldWithShadow> primarySpriteChoices;
    public List<IconMouldWithShadow> secondarySpriteChoices;
    public List<IconMouldWithShadow> tertiarySpriteChoices;
}
