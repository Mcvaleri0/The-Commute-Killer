using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Action
{
    public PickUp(Agent agent, GameObject target) : base(agent) {
        this.Targets = new List<GameObject>()
        {
            target
        };
    }

    public static bool CanExecute(GameObject agent, GameObject target)
    {
        if (agent.GetComponent<Agent>() == null) { return false; } // Action must be executed by an agent
        if (target.GetComponent<Agent>() == null) { return false; } // Target must be an agent

        var aPos = agent.transform.position;
        var tPos = target.transform.position;

        if (Vector3.Distance(aPos, tPos) < 1.5f)
        {
            return true;
        }

        return false;
    }

    override public void Execute() 
    {
        this.Agent.PickUp(this.Instrument);
    }
}
