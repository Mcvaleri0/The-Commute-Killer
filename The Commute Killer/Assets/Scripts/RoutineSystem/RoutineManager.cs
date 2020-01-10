using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoutineManager : MonoBehaviour
{
    public List<Routine> Routines; // List of a NPC's Routines

    public TimeManager TimeManager;

    private Routine CurrentRoutine;

    private int Step = 0;

    private int RoutineStep = 0;

    private Agent Agent;
    
    void Start()
    {
        this.Agent = GetComponent<Agent>();

        /* Routine1  FIXME
        Agent agent1 = new Agent();

        List<RoutineAction> routineActions = new List<RoutineAction>() {
            new RoutineAction(Action.IDs.PickUp, new DateTime(1989, 9, 6, 8, 30, 0), new DateTime(1989, 9, 6, 8, 30, 0)),
            new RoutineAction(Action.IDs.PickUp, new DateTime(1989, 9, 6, 8, 30, 0), new DateTime(1989, 9, 6, 8, 30, 0))
        };

        DateTime iniTime = new DateTime(1989, 9, 6, 8, 30, 0);
        DateTime endTime = new DateTime(1989, 9, 6, 9, 00, 0);

        var routine1 = new Routine(0, agent1, routineActions, iniTime, endTime);
        */
    }


    private void Update()
    {
        // Update Routine
        var step = this.CurrentRoutine.Step(this.TimeManager.GetCurrentTime());

        // If the step has changed
        if (step != this.RoutineStep)
        {
            this.RoutineStep = step; // Update it
        }

        // If this Routine is finished
        if (this.CurrentRoutine.Finished())
        {
            // Get next Routine
            this.CurrentRoutine = this.Routines[++this.Step];
        }
    }

    public Action NextAction()
    {
        return this.CurrentRoutine.CurrentAction;
    }
}
