using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Routine : ScriptableObject
{
    #region /* Attributes */
    public int Id;

    public List<RoutineAction> RoutineActions;

    #region /* Time */
    public int StartHour;
    public int StartMinute;

    public int EndHour;
    public int EndMinute;
    #endregion

    private int State = 0; // [ 0 - Idle | 1 - Executing ]

    private RoutineAction CurrentRoutineAction;

    public Action CurrentAction { get; private set; }
    #endregion


    public void Initialize()
    {
        #region Init Time Interval
        var sMinute = 59;
        var sHour   = 23;

        var eMinute = 59;
        var eHour   = 23;

        foreach(var a in RoutineActions)
        {
            if (a == null)
            {
                this.RoutineActions.Remove(a);
                continue;
            }

            if(a.StartHour < sHour)
            {
                sHour   = a.StartHour;
                sMinute = a.StartMinute;
            }
            else if(a.StartHour == sHour)
            {
                if(a.StartMinute < sMinute)
                {
                    sMinute = a.StartMinute;
                }
            }

            if (a.EndHour < eHour)
            {
                eHour   = a.EndHour;
                eMinute = a.EndMinute;
            }
            else if (a.EndHour == eHour)
            {
                if (a.EndMinute < eMinute)
                {
                    eMinute = a.EndMinute;
                }
            }
        }

        this.StartHour   = sHour;
        this.StartMinute = sMinute;

        //this.EndHour   = eHour;
        //this.EndMinute = eMinute;
        #endregion

        foreach(var rAction in this.RoutineActions)
        {
            rAction.Initialize();
        }
    }


    #region === Routine Methods ===
    // Returns the step at which the routine is
    public void Step(DateTime currentTime)
    {
        switch(this.State)
        {
            case 0: // Idle
                // If an Action can be performed
                if(DetermineAction(currentTime))
                {
                    this.State = 1; // Go to Executing
                }
                break;

            case 1: // Executing
                 // If the current action has been finished
                if (this.CurrentAction.Finished())
                {
                    this.CurrentRoutineAction.Concluded(); // Mark it as concluded in the routine

                    this.CurrentRoutineAction = null;
                    this.CurrentAction        = null;

                    this.State = 0; // Go to Idle
                }
                break;
        }
    }

    // Returns True if a Routine Action can be Initiated
    private bool DetermineAction(DateTime currentTime)
    {
        // Foreach of the Actions
        foreach (var rAction in this.RoutineActions)
        {
            // If the action can be initiated
            if(rAction.CanStart(currentTime, out Action action))
            {
                this.CurrentAction = action;
                this.CurrentRoutineAction = rAction;

                return true;
            }
        }

        return false;
    }

    // Return True if the Routine can begin
    public bool CanBegin(DateTime currentTime)
    {
        float current = currentTime.Hour + currentTime.Minute / 60f;

        float start = this.StartHour + this.StartMinute / 60f;

        float end = this.EndHour + this.EndMinute / 60f;

        return start <= current && current < end;
    }

    // Return True if the Routine has been concluded
    public bool Finished(DateTime currentTime)
    {
        float current = currentTime.Hour + currentTime.Minute / 60f;

        float end = this.EndHour + this.EndMinute / 60f;

        return end < current;
    }
    #endregion


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = obj as Routine;

        return other.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
