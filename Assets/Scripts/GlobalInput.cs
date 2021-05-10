using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInput : MonoBehaviour
{
    private static GlobalInput _instance;
    public static GlobalInput Instance { get { return _instance; } }

    [HideInInspector]
    public GameObject selectedEntity;
    [HideInInspector]
    public ItemObject selectedItemObject;

    public delegate void ChangeSelectedCharacter();
    public ChangeSelectedCharacter changeSelectedEntity;

    public delegate void ChangeSelectedItemObject();
    public ChangeSelectedItemObject changeSelectedItemObject;

    public Location defaultLocation;

    [Space(6)]
    public List<CharClass> possibleClasses;

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
        changeSelectedEntity.Invoke();
    }

    public bool CheckIfSelectedCharacter()
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

    public bool CheckIfSelectedLocation()
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
