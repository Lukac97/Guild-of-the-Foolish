using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MCombatAnimationHandler : MonoBehaviour
{
    public GameObject alertText;

    public GameObject playerCombatText;
    public GameObject enemyCombatText;

    [SerializeField] private GameObject textObjectPrefab;
    [SerializeField] private GameObject alertObjectPrefab;

    private List<GameObject> activeCombatTextList = new List<GameObject>();
    private List<GameObject> activeAlertTextList = new List<GameObject>();


    public void CombatTextAnimationEnded(GameObject gO)
    {
        activeCombatTextList.Remove(gO);
        Destroy(gO);
    }

    public void AlertTextAnimationEnded(GameObject gO)
    {
        activeAlertTextList.Remove(gO);
        Destroy(gO);
    }

    public void HandleTextDisplay(UsedSpellResult usedSpellResult, bool forPlayer)
    {
        HandleTextDisplay(usedSpellResult, forPlayer ? enemyCombatText : playerCombatText);
    }

    private void HandleTextDisplay(UsedSpellResult usedSpellResult, GameObject textTarget)
    {
        // TODO: Implement logic for displaying all sorts of information (Statuses, Applied statuses, Damage/Healing etc)
        if (usedSpellResult == null)
            return;
        GameObject gO = Instantiate(textObjectPrefab, textTarget.transform);
        activeCombatTextList.Add(gO);
        TextMeshProUGUI textContent = gO.GetComponent<TextMeshProUGUI>();
        textContent.text = usedSpellResult.spellUsed.name + "( " + usedSpellResult.appliedIntensityInstances[0].intensity + " )";
        gO.GetComponent<Animator>().Play("text_fade_scroll_down");
    }

    public void NotEnoughResourceAlert()
    {
        AlertDisplay("Not enough resource!");
    }

    public void OnCooldownAlert()
    {
        AlertDisplay("Spell is on cooldown!");
    }

    private void AlertDisplay(string alertString)
    {
        if (alertString == "")
            return;
        GameObject gO = Instantiate(alertObjectPrefab, alertText.transform);
        activeAlertTextList.Add(gO);
        TextMeshProUGUI textContent = gO.GetComponent<TextMeshProUGUI>();
        textContent.text = alertString;
        gO.GetComponent<Animator>().Play("alert_fade");
    }
}
