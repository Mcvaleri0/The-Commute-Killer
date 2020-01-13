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

    private int CurrentDay = 1;

    private void Awake()
    {
        this.ActiveRoutines = new List<Routine>();

        for (var i = 0; i < this.Routines.Count; i++)
        {
            this.Routines[i].Initialize();
        }
    }

    void Start()
    {
        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.CurrentDay = this.TimeManager.GetCurrentTime().Day;
    }

    // Update
    public Action Update()
    {
        var currentTime = this.TimeManager.GetCurrentTime();

        if(currentTime.Day != this.CurrentDay)
        {
            this.CurrentDay = currentTime.Day;

            foreach(var routine in this.Routines)
            {
                foreach(var action in routine.RoutineActions)
                {
                    action.Executed = false;
                }
            }
        }

        if(this.CurrentAction == null)
        {
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

            // Check for routines that have concluded
            for (var i = 0; i < this.ActiveRoutines.Count; i++)
            {

                // If a Routine has been concluded
                if (this.ActiveRoutines[i].Finished(currentTime))
                {
                    this.ActiveRoutines.RemoveAt(i); // Remove it from the active list
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

                    this.CurrentAction = routine.CurrentAction;

                    break;
                }
            }
        }
        else
        {
            // If Action has Finished
            if(this.CurrentAction.Finished())
            {
                this.CurrentAction = null;
            }
        }

        

        return this.CurrentAction;
    }
}
