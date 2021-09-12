using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedCharInfo : MonoBehaviour
{
    [SerializeField] private GameObject moveButtonHolder;
    [SerializeField] private GameObject characterInfoHolder;
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("Sliders")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider srSlider;
    [Space(3)]
    [SerializeField] private TextMeshProUGUI charCurrentHp;
    [SerializeField] private TextMeshProUGUI charMaxHp;
    [SerializeField] private TextMeshProUGUI charCurrentSr;
    [SerializeField] private TextMeshProUGUI charMaxSr;

    private CharStats selectedCharStats;

    private void Awake()
    {
        CharactersController.CharactersResourcesUpdated += UpdateSelectedCharSliders;
    }

    private void Start()
    {
        GlobalInput.Instance.onChangedSelectedEntity += ShowSelectedCharacterInfo;
        ShowSelectedCharacterInfo();
    }

    public void ShowSelectedCharacterInfo()
    {
        if (!GlobalInput.CheckIfSelectedCharacter())
        {
            selectedCharStats = null;
        }
        else
        {
            selectedCharStats = GlobalInput.Instance.selectedEntity.GetComponent<CharStats>();
        }

        if(selectedCharStats != null)
            characterName.text = selectedCharStats.characterName;
        else
            characterName.text = "";

        EnableMoveButton();
        UpdateSelectedCharSliders();
    }

    private void EnableMoveButton()
    {
        if (selectedCharStats != null)
        {
            moveButtonHolder.SetActive(true);
        }
        else
        {
            moveButtonHolder.SetActive(false);
        }
    }

    public void UpdateSelectedCharSliders()
    {
        if (selectedCharStats == null)
        {
            characterInfoHolder.SetActive(false);
            return;
        }
        else
        {
            characterInfoHolder.SetActive(true);
        }
        CharCombat charCombat = selectedCharStats.GetComponent<CharCombat>();

        // Number setting
        if (charCurrentHp != null)
        {
            charCurrentHp.text = charCombat.combatStats.currentHealth.ToString("N0");
        }
        if (charMaxHp != null)
        {
            charMaxHp.text = charCombat.combatStats.totalStats.maxHealth.ToString("N0");
        }
        if (charCurrentSr != null)
        {
            charCurrentSr.text = charCombat.combatStats.currentSpellResource.ToString("N0");
        }
        if (charMaxSr != null)
        {
            charMaxSr.text = charCombat.combatStats.totalStats.maxSpellResource.ToString("N0");
        }
        if (hpSlider != null)
        {
            if (charCombat.combatStats.totalStats.maxHealth == 0)
                hpSlider.value = 0;
            else
                hpSlider.value = charCombat.combatStats.currentHealth / charCombat.combatStats.totalStats.maxHealth;
        }
        if (srSlider != null)
        {
            if (charCombat.combatStats.totalStats.maxSpellResource == 0)
                srSlider.value = 0;
            else
                srSlider.value = charCombat.combatStats.currentSpellResource / charCombat.combatStats.totalStats.maxSpellResource;
        }
    }

}
