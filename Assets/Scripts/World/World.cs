using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    [System.Serializable]
    public class Connection
    {
        public Location loc1;
        public Location loc2;

        public Connection(GameObject g1, GameObject g2)
        {
            loc1 = g1.GetComponent<Location>();
            loc2 = g2.GetComponent<Location>();
        }

        public Connection(Location l1, Location l2)
        {
            loc1 = l1;
            loc2 = l2;
        }

        public Location GetConnected(Location origin)
        {
            if (origin == loc1)
            {
                return loc2;
            }
            else if (origin == loc2)
            {
                return loc1;
            }
            else
            {
                return null;
            }
        }
    }

    public GameObject locations;
    public MapMain connectedMap;
    [Space(6)]
    public List<Connection> nodeConnections;

    public List<Location> GetConnections(Location originLocation)
    {
        List<Location> connectedLocations = new List<Location>();
        foreach (Connection connection in nodeConnections)
        {
            Location loc = connection.GetConnected(originLocation);
            if (loc != null)
            {
                connectedLocations.Add(loc);
            }
        }
        return connectedLocations;
    }
}
