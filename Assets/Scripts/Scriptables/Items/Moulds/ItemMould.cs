using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMould : ScriptableObject, MouldedItemsInterface
{
    [System.Serializable]
    public class ColorCustomization
    {
        [Header("0-1")]
        public float colorValueMin;
        public float colorValueMax;
        [Header("0-1")]
        public float colorSaturationRangeMin;
        public float colorSaturationRangeMax;
    }

    [System.Serializable]
    public class IconMouldWithShadow
    {
        public Sprite spritePart;
        public Sprite spritePartShadow;
    }

    [System.Serializable]
    public class SpriteChoice
    {
        public List<IconMouldWithShadow> spriteChoices;
        public ColorCustomization colorCustomization;
    }

    public string itemID = "id_of_moulded_item";

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
    public SpriteChoice primarySpriteChoices;
    public SpriteChoice secondarySpriteChoices;
    public SpriteChoice tertiarySpriteChoices;
}
