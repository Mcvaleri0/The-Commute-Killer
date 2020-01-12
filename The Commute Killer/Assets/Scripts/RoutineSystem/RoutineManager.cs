using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoutineManager : MonoBehaviour
{
    public List<Routine> Routines; // List of a NPC's Routines

    private List<Routine> ActiveRoutines; // List of currently Active Routines

    private TimeManager TimeManager;

    public Routine CurrentRoutine { get; private set; }

    public Action CurrentAction { get; private set; }
    
    void Start()
    {
        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.ActiveRoutines = new List<Routine>();

        foreach(var routine in this.Routines)
        {
            routine.Initialize();
        }
    }

    // Return the next Action to be Executed
    public Action NextAction()
    {
        var currentTime = this.TimeManager.GetCurrentTime();

        // Check for routines that can begin
        foreach (var routine in this.Routines)
        {
            // If a Routine can begin
            if (routine.CanBegin(currentTime) && !this.ActiveRoutines.Contains(routine))
            {
                this.ActiveRoutines.Add(Instantiate(routine)); // Add it to the active list
                continue;
            }
        }

        foreach(var routine in this.ActiveRoutines)
        {
            // If a Routine has been concluded
            if (routine.Finished(currentTime))
            {
                this.ActiveRoutines.Remove(routine); // Remove it from the active list
                continue;
            }
        }

        // Foreach Active Routine
        foreach (var routine in this.ActiveRoutines)
        {
            routine.Step(currentTime); // Poll the Routine at Current Time

            // If an Action from the routine can be executed
            if (routine.CurrentAction != null)
            {
                this.CurrentRoutine = routine;

                this.CurrentAction  = routine.CurrentAction;

                break;
            }
        }

        return this.CurrentAction;
    }
}
