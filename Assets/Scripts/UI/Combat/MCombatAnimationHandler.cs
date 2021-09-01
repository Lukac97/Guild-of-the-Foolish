using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MCombatAnimationHandler : MonoBehaviour
{
    public GameObject playerCombatText;
    public GameObject enemyCombatText;

    [SerializeField] private GameObject textObjectPrefab;

    [Header("Animations")]
    [SerializeField] private Animation textForDamageAnimation;

    public void HandleTextDisplay(UsedSpellResult usedSpellResult, bool forPlayer)
    {
        HandleTextDisplay(usedSpellResult, forPlayer ? enemyCombatText : playerCombatText);
    }

    private void HandleTextDisplay(UsedSpellResult usedSpellResult, GameObject textTarget)
    {
        if (usedSpellResult == null)
            return;
        GameObject gO = Instantiate(textObjectPrefab, textTarget.transform);
        TextMeshProUGUI textContent = gO.GetComponent<TextMeshProUGUI>();
        textContent.text = usedSpellResult.spellUsed.name + "( " + usedSpellResult.appliedIntensityInstances[0].intensity + " )";
        gO.GetComponent<Animator>().Play("text_fade_scroll_down");
    }
}
