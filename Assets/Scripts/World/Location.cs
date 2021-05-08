using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public World world;
    public string locationName;

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
