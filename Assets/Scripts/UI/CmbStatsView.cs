using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CmbStatsView : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI spellResourceValue;
    public TextMeshProUGUI physicalDamageValue;
    public TextMeshProUGUI magicalDamageValue;
    public TextMeshProUGUI physicalResistanceValue;
    public TextMeshProUGUI magicalResistanceValue;
    public TextMeshProUGUI avoidPotencyValue;
    public TextMeshProUGUI criticalPotencyValue;

    private void Start()
    {
        CharactersController.CharactersUpdated += ChangeInformation;
        CharactersController.CharactersResourcesUpdated += ChangeInformation;
        GlobalInput.Instance.onChangedSelectedEntity += ChangeInformation;
        ChangeInformation();
    }

    public void ChangeInformation()
    {
        if(!GlobalInput.CheckIfSelectedCharacter())
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            return;
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        CharCombat charCombat = GlobalInput.Instance.selectedEntity.GetComponent<CharCombat>();
        healthValue.text = charCombat.combatStats.currentHealth.ToString("0")
            + " / " + charCombat.combatStats.baseStats.maxHealth.ToString("0");
        spellResourceValue.text = charCombat.combatStats.currentSpellResource.ToString("0")
            + " / " + charCombat.combatStats.baseStats.maxSpellResource.ToString("0");
        physicalDamageValue.text = charCombat.combatStats.baseStats.physicalDamage.ToString("0.0");
        magicalDamageValue.text = charCombat.combatStats.baseStats.magicalDamage.ToString("0.0");
        physicalResistanceValue.text = charCombat.combatStats.baseStats.physicalResistance.ToString("0.0");
        magicalResistanceValue.text = charCombat.combatStats.baseStats.magicalResistance.ToString("0.0");
        avoidPotencyValue.text = charCombat.combatStats.baseStats.avoidPotency.ToString("0.0");
        criticalPotencyValue.text = charCombat.combatStats.baseStats.criticalPotency.ToString("0.0");
    }
}
