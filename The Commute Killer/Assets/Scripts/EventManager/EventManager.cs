﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private GameObject Map;

    private GameObject Victim;

    private Vector3 VictimStartPosition;
    private Vector3 VictimEndPosition;

    private TimeManager TimeManager { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        this.Map = GameObject.Find("Map");

        this.Victim = GameObject.Find("Victim");

        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.VictimStartPosition = new Vector3(19.375f, 0.3f, -15.225f);
        this.VictimEndPosition   = new Vector3(11f, 0.3f, -45f);

        //this.Victim.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.TriggerEvent(Event.VictimStartEnd);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            this.TriggerEvent(Event.VictimEndStart);
        }
    }


    public bool TriggerEvent(Event e)
    {
        switch(e)
        {
            case Event.Hydrant_0_ON:
                return this.Map.GetComponent<MapController>().BlockArc(25, 16, true);

            case Event.Hydrant_0_OFF:
                return this.Map.GetComponent<MapController>().BlockArc(25, 16, false);

            case Event.VictimStartEnd:
                this.VictimMovement(this.VictimStartPosition, this.VictimEndPosition);
                return true;

            case Event.VictimEndStart:
                this.VictimMovement(this.VictimEndPosition, this.VictimStartPosition);
                return true;

            case Event.VictimAtGoal:
                this.VictimAtGoal();
                break;
        }

        return false;
    }


    public void VictimMovement(Vector3 start, Vector3 end)
    {
        this.Victim.SetActive(true);

        this.Victim.transform.position = start;
        this.Victim.GetComponent<Agent>().GoalPosition = end;
    }


    // Updates what is needed in the game after the victim reaches his target location
    private void VictimAtGoal()
    {
        // Sets the alarm for the victim to change positons again
        this.TimeManager.UpdateTimeForVictimToMove();
    }
}