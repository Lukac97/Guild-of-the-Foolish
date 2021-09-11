using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalTime : MonoBehaviour
{
    private static GlobalTime _instance;
    public static GlobalTime Instance
    {
        get
        {
            return _instance;
        }
    }

    public static List<GlobalAction> globalActions = new List<GlobalAction>();
    public static int halfCycleNumber = 0;

    [SerializeField] private TextMeshProUGUI cycleNrTxt;
    [SerializeField] private TextMeshProUGUI dayNightToggle;

    public delegate void HalfCycleDelegate();
    public static HalfCycleDelegate OnNextHalfCycle;

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
        OnNextHalfCycle += UpdateGlobalTimeText;
    }

    private void Start()
    {
        UpdateGlobalTimeText();
    }

    public void OnClickNextHalfCycle()
    {
        List<GlobalAction> newGlobalActions = new List<GlobalAction>(globalActions);
        foreach(GlobalAction gA in globalActions)
        {
            if(gA.OnHalfCycleChanged())
            {
                newGlobalActions.Remove(gA);
            }
        }
        globalActions = new List<GlobalAction>(newGlobalActions);
        halfCycleNumber++;
        if (OnNextHalfCycle != null)
            OnNextHalfCycle.Invoke();
    }

    private void UpdateGlobalTimeText()
    {
        cycleNrTxt.text = Mathf.FloorToInt(halfCycleNumber / 2).ToString();
        dayNightToggle.text = halfCycleNumber % 2 > 0 ? "N" : "D";
    }

    public static void CreateCombatEncounterAction(CombatEncounter cbE)
    {
        CombatEncounterAction newAction = new CombatEncounterAction(cbE, 1);
        globalActions.Add(newAction);
        CheckIfActionIsInstant(newAction);
    }

    public static void CreateCharacterMoveAction(CharStats charStats, Location loc)
    {
        CharacterMoveAction newAction = new CharacterMoveAction(charStats, loc, 1);
        globalActions.Add(newAction);
        CheckIfActionIsInstant(newAction);
    }

    private static void CheckIfActionIsInstant(GlobalAction gA)
    {
        if(gA.CheckIfActionFinished())
        {
            globalActions.Remove(gA);
        }
    }

}
