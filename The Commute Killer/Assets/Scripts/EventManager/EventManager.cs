using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private MapController Map;

    private DetectionSystem Detection;

    private GameObject Victim;

    private Vector3 VictimStartPosition;

    private Vector3 VictimEndPosition;

    private TimeManager TimeManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.Map = GameObject.Find("Map").GetComponent<MapController>();

        this.Detection = GameObject.Find("DetectionSystem").GetComponent<DetectionSystem>();

        this.Victim = GameObject.Find("Victim");

        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.VictimStartPosition = new Vector3(19.375f, 0.3f, -15.225f);
        this.VictimEndPosition   = new Vector3(11f, 0.3f, -45f);
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
            //FIXME: estes dois primeiros sao para sair. estes eventos ja nao deviam de existir
            case Event.VictimStartEnd:
                this.VictimMovement(this.VictimStartPosition, this.VictimEndPosition);
                return true;

            case Event.VictimEndStart:
                this.VictimMovement(this.VictimEndPosition, this.VictimStartPosition);
                return true;

            case Event.VictimAtGoal:
                this.VictimAtGoal();
                return true;

            case Event.Hydrant_ON:
                this.Map.BlockArc(35, 46, true);
                return true;

            case Event.Hydrant_OFF:
                this.Map.BlockArc(35, 46, false);
                return true;

            case Event.Hydrant_1_ON:
                this.Map.BlockArc(1, 38, true);
                return true;

            case Event.Hydrant_1_OFF:
                this.Map.BlockArc(1, 38, false);
                return true;

            case Event.InteractibleGardenGate_1_Close:
                this.Map.BlockArc(32, 33, true);
                return true;

            case Event.InteractibleGardenGate_1_Open:
                this.Map.BlockArc(32, 33, false);
                return true;

            case Event.InteractibleGardenGate_Close:
                this.Map.BlockArc(34, 35, true);
                return true;
            
            case Event.InteractibleGardenGate_Open:
                this.Map.BlockArc(34, 35, false);
                return true;

            case Event.Killed:
                this.Detection.TryToTriggerGameOver();
                return true;

            case Event.VictimAtDumpster:
                GameObject.Find("LevelManager").GetComponent<LevelManager>().Win();
                return true;
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
