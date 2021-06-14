using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharListElement : MonoBehaviour
{
    [HideInInspector]
    public CharStats linkedCharacter;
    public CharCombat linkedCharacterCombat;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textClass;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textLocation;
    public Image icon;

    [Space(6)]
    [Header("Customization")]
    public string levelPrefix;
    public Slider hpSlider;
    public Slider srSlider;

    public void LinkCharacter(CharStats newChar)
    {
        linkedCharacter = newChar;
        linkedCharacterCombat = newChar.GetComponent<CharCombat>();
        if (hpSlider != null)
            hpSlider.maxValue = 1;
        if (srSlider != null)
            srSlider.maxValue = 1;
        UpdateCharText();
        UpdateSliders();
    }

    public void UpdateCharText()
    {
        if(icon != null)
            icon.sprite = linkedCharacter.characterClass.classIcon;
        if(textName != null)
            textName.text = linkedCharacter.characterName;
        if(textLevel != null)
            textLevel.text = levelPrefix + " " + linkedCharacter.level.ToString();
        if(textClass != null)
            textClass.text = linkedCharacter.characterClass.name;
        if (textLocation != null)
        {
            if (linkedCharacter.location != null)
            {
                textLocation.text = linkedCharacter.location.locationName;
            }
            else
            {
                textLocation.text = "UNKNOWN";
            }
        }
    }

    public void UpdateSliders()
    {
        if (linkedCharacter == null | linkedCharacterCombat == null)
            return;
        if (hpSlider != null)
        {
            if (linkedCharacterCombat.combatStats.totalStats.maxHealth == 0)
                hpSlider.value = 0;
            else
                hpSlider.value = linkedCharacterCombat.combatStats.currentHealth / linkedCharacterCombat.combatStats.totalStats.maxHealth;
        }
        if (srSlider != null)
        {
            if (linkedCharacterCombat.combatStats.totalStats.maxSpellResource == 0)
                srSlider.value = 0;
            else
                srSlider.value = linkedCharacterCombat.combatStats.currentSpellResource / linkedCharacterCombat.combatStats.totalStats.maxSpellResource;
        }
    }

    public void CallChangeSelectedCharacter()
    {
        GlobalInput.Instance.setSelectedEntity(linkedCharacter.gameObject);
    }
}
