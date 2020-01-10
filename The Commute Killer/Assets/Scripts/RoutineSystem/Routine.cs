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
    public DateTime IniTime;

    public DateTime EndTime;
    #endregion

    private AutonomousAgent Agent { get; set; }

    private int CurrentStep = 0;

    private int State = 0; // [ 0 - Idle | 1 - Executing ]

    public Action CurrentAction { get; private set; }
    #endregion


    #region === Routine Methods ===
    // Returns the step at which the routine is
    public int Step(DateTime currentTime)
    {
        switch(this.State)
        {
            case 0: // Idle
                // If an Action can be performed
                if(this.AdvanceRoutine(currentTime))
                {
                    this.State = 1; // Go to Executing
                }
                break;

            case 1: // Executing
                 // If the current action has been finished
                if (this.CurrentAction.Finished())
                {
                    var currentRAction = this.RoutineActions[this.CurrentStep];
                    currentRAction.Concluded(); // Mark it as concluded in the routine

                    // If no Action can be performed yet
                    if(!AdvanceRoutine(currentTime))
                    {
                        this.State = 0; // Go to Idle
                    }
                }
                break;
        }

        return this.CurrentStep;
    }


    private bool AdvanceRoutine(DateTime currentTime)
    {
        // Foreach of the remaining actions
        for (var i = this.CurrentStep + 1; i < this.RoutineActions.Count; i++)
        {
            var rAction = this.RoutineActions[i]; // Get Routine Action

            // If the action can start
            if (rAction.CanStart(currentTime, out Action action))
            {
                this.CurrentStep = i; // Advance the Routine

                this.CurrentAction = action;

                return true;
            }
        }

        return false;
    }

    // Return True if the Routine has been concluded
    public bool Finished()
    {
        if(this.CurrentStep == this.RoutineActions.Count)
        {
            return true;
        }

        return false;
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

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
