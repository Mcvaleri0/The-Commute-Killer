using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoutineAction : ScriptableObject
{
    public bool Executed { get; private set; } = false;

    public Action.IDs ActionId;

    public Agent Agent;

    public GameObject Target;

    // Time window
    public DateTime StartTime;
    public DateTime EndTime;

    // Dependencies
    public Vector3 ExecutePosition;

    public float Distance;

    public List<RoutineAction> PrecedingActions;

    // Check if action can be initiated
    public bool CanStart(DateTime currentTime, out Action action)
    {
        action = null;

        // Check that it hasn't already been executed
        if (this.Executed)
        {
            return false;
        }

        // Check time window
        if (currentTime < this.StartTime || this.EndTime < currentTime)
        {
            return false;
        }

        // If there's a position to execute the action at
        if (this.ExecutePosition != null)
        {
            var agentPos = this.Agent.GetPosition();

            // Check distance to target position
            if (Vector3.Distance(agentPos, this.ExecutePosition) > this.Distance)
            {
                return false;
            }
        }

        // If there is a Target
        if (this.Target != null)
        {
            var targeted = this.Agent.GetInFront();

            // Check if Target is in front
            if (targeted == this.Target)
            {
                return false;
            }
        }

        // Check if actions in dependency list were completed
        foreach (RoutineAction act in this.PrecedingActions)
        {
            if(act.Executed != true)
            {
                return false;
            }
        }

        // Generate the Action
        var genAction = GenerateAction();

        // Check that the Action can be executed
        if (!action.CanExecute())
        {
            return false;
        }

        action = genAction;

        return true;
    }

    // Set the Routine Action as having concluded
    public void Concluded()
    {
        this.Executed = true;
    }

    public Action GenerateAction()
    {
        return Action.GetAction(ActionId, this.Agent, this.Target);
    }
}
