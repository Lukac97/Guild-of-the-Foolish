using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyOnLocation : MonoBehaviour
{
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyLevel;
    public Location.PossibleEnemy enemyMould;

    public void InitEnemyOnLocation(Location.PossibleEnemy psEn)
    {
        enemyName.text = psEn.enemyMould.name;
        enemyLevel.text = psEn.minLevel.ToString() + " - " + psEn.maxLevel.ToString();
        enemyMould = psEn;
    }

    public void OnClickAttack()
    {
        CombatEncounterController.Instance.CreateEncounter(CharactersController.Instance.characters[0].GetComponent<CharStats>(),
            enemyMould, GlobalInput.Instance.selectedEntity.GetComponent<Location>());
    }
}
