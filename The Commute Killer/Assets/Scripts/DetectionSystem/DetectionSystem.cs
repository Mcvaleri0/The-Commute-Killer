using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public List<Zone> Zones;

    private void Start()
    {
        this.Zones = new List<Zone>();
    }

    private void Update()
    {
        
    }

    public Zone GetZone(Vector3 position)
    {
        foreach(Zone z in this.Zones)
        {
            if(z.In(position))
            {
                return z;
            }
        }

        return null;
    }
}