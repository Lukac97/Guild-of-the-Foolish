using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [System.Serializable]
    public class PossibleEnemy
    {
        public EnemyMould enemyMould;
        public int minLevel;
        public int maxLevel;
    }

    public World world;
    public string locationName;
    public List<PossibleEnemy> possibleEnemies;

    [Space(6)]
    [HideInInspector]
    public MapNode connectedNode;

    private void Awake()
    {
        world = GetComponentInParent<World>();
    }

    private void Start()
    {
        connectedNode = world.connectedMap.FindMapNodeByLocation(this);
    }

}
