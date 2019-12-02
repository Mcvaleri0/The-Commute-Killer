using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private GameObject Map;

    private GameObject Victim;

    private Vector3 VictimStartPosition;
    private Vector3 VictimEndPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.Map = GameObject.Find("Map");

        this.Victim = GameObject.Find("Victim");

        this.VictimStartPosition = new Vector3(19.375f, 0.3f, -15.225f);
        this.VictimEndPosition   = new Vector3(11f, 0.3f, -45f);

        this.Victim.SetActive(false);
    }

    public bool TriggerEvent(string name)
    {
        switch(name)
        {
            case "Hydrant_0_ON":
                return this.Map.GetComponent<MapController>().BlockArc(25, 16, true);

            case "Hydrant_0_OFF":
                return this.Map.GetComponent<MapController>().BlockArc(25, 16, false);
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            this.Victim.SetActive(true);

            this.Victim.transform.position = this.VictimStartPosition;
            this.Victim.GetComponent<Agent>().GoalPosition = this.VictimEndPosition;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            this.Victim.SetActive(true);

            this.Victim.transform.position = this.VictimEndPosition;
            this.Victim.GetComponent<Agent>().GoalPosition = this.VictimStartPosition;
        }
    }
}
