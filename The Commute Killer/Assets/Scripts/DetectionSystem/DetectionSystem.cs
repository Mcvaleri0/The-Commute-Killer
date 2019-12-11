using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public List<Zone> Zones;

    private Player Player;

    private List<Action.IDs> BannedActions;

    public Zone PlayerZone;

    private void Start()
    {
        this.Player = GameObject.Find("PlayerCharacter").GetComponent<Player>();
    }

    private void Update()
    {
        this.PlayerZone = GetZone(this.Player.transform.position);
    }

    public void TryToTriggerGameOver()
    {
        if(this.PlayerZone.AwarenessLevel == Zone.Awareness.Surveiled)
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
            return;
        }
    }

    public Zone GetZone(Vector3 position)
    {
        Zone zone = null;

        var min = Mathf.Infinity;

        foreach(Zone z in this.Zones)
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