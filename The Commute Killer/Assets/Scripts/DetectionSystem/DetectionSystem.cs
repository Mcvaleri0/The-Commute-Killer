using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    private List<NavZone> Zones;

    private Player Player;

    public NavZone PlayerZone;

    private void Start()
    {
        var zoneObjects = GameObject.FindGameObjectsWithTag("Zone");

        this.Zones = new List<NavZone>();

        foreach(var zone in zoneObjects)
        {
            this.Zones.Add(zone.GetComponent<NavZone>());
        }

        this.Player = GameObject.Find("PlayerCharacter").GetComponent<Player>();
    }

    private void Update()
    {
        this.PlayerZone = GetZone(this.Player.transform.position);
    }

    public void TryToTriggerGameOver()
    {
        if(this.PlayerZone.AwarenessLevel == NavZone.Awareness.Surveiled)
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
            return;
        }
    }

    public NavZone GetZone(Vector3 position)
    {
        NavZone zone = null;

        var min = Mathf.Infinity;

        foreach(NavZone z in this.Zones)
        {
            var yDist = z.In(position);

            if (yDist < min)
            {
                zone = z;

                min = yDist;
            }
        }

        return zone;
    }
}