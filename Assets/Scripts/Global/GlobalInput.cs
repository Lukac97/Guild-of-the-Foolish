using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInput : MonoBehaviour
{
    private static GlobalInput _instance;
    public static GlobalInput Instance { get { return _instance; } }

    public GameObject selectedEntity;
    [HideInInspector]
    public ItemObject selectedItemObject;

    public delegate void ChangeSelectedEntity();
    public ChangeSelectedEntity onChangedSelectedEntity;

    public delegate void ChangeSelectedItemObject();
    public ChangeSelectedItemObject changeSelectedItemObject;

    public Location defaultLocation;

    [Space(6)]
    public List<CharClass> possibleClasses;

    [Space(3)]
    [Header("Universal colors")]
    [Space(3)]
    public Color goodColor;
    public Color badColor;

    [Space(4)]
    [Header("Combat logging")]
    public Color damageColor;
    public Color healColor;
    public Color neutralIntensityColor;
    [Space(4)]
    public Color enemyTurnColor;
    public Color characterTurnColor;
    [Space(4)]
    public Color enemyNameColor;
    public Color charNameColor;
    [Space(4)]
    public Color spellColor;
    [Space(4)]
    public Color battleWonColor;
    public Color battleLostColor;
    public Color battleTiedColor;
    [Space(4)]
    public Color defaultLogColor;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
    }

    public void setSelectedEntity(GameObject entity)
    {
        selectedEntity = entity;
        onChangedSelectedEntity.Invoke();
    }

    public static bool CheckIfSelectedCharacter()
    {
        if (GlobalInput.Instance.selectedEntity == null)
        {
            return false;
        }
        CharStats charStatsTemp;
        if (GlobalInput.Instance.selectedEntity.TryGetComponent<CharStats>(out charStatsTemp))
        {
            return true;
        }
        return false;
    }

    public static bool CheckIfSelectedLocation()
    {
        if (GlobalInput.Instance.selectedEntity == null)
        {
            return false;
        }
        Location loc;
        if (GlobalInput.Instance.selectedEntity.TryGetComponent<Location>(out loc))
        {
            return true;
        }
        return false;
    }

    public void SetSelectedItemObject(ItemObject item)
    {
        selectedItemObject = item;
        changeSelectedItemObject.Invoke();
    }
}
