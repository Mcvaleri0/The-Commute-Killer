using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoutineAction : ScriptableObject
{
    public bool Executed { get; private set; } = false;

    public Action.IDs ActionId;

    private Agent Agent;

    private GameObject Target;

    public String AgentName;

    public String TargetName;

    // Time window
    [Range(0,23)]
    public int StartHour;
    [Range(0,59)]
    public int StartMinute;

    [Range(0,23)]
    public int EndHour;
    [Range(0,59)]
    public int EndMinute;

    // Dependencies
    public Vector3 ExecutePosition;

    public float Distance;

    public List<RoutineAction> PrecedingActions;

    public void Initialize()
    {
        if(this.AgentName != null && this.AgentName != "")
        {
            this.Agent = GameObject.Find(this.AgentName).GetComponent<Agent>();
        }

        if (this.TargetName != null && this.TargetName != "")
        {
            this.Target = GameObject.Find(this.TargetName);
        }
    }

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
        if(!InTime(currentTime))
        {
            return false;
        }

        // If there's a position to execute the action at
        if (this.ExecutePosition != null && this.ExecutePosition != Vector3.zero)
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
            if (targeted != this.Target)
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
        if (!genAction.CanExecute())
        {
            return false;
        }

        action = genAction;

        return true;
    }


    private bool InTime(DateTime currentTime)
    {
        float current = currentTime.Hour + currentTime.Minute / 60f;

        float start = this.StartHour + this.StartMinute / 60f;

        float end = this.EndHour + this.EndMinute / 60f;

        return start <= current && current < end;
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
