using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Action
{
    public PickUp(Agent agent, GameObject target) : base(agent) {
        this.ID = IDs.PickUp;

        this.Targets = new List<GameObject>()
        {
            target
        };
    }

    override public bool CanExecute(Agent agent, GameObject target)
    {
        if (target.GetComponent<Item>() == null) { return false; } // Target must be an item

        var agentComp = agent.GetComponent<Agent>();

        if (agentComp.FirstFree != -1 || agentComp.OnHand == null)
        {
            return target.GetComponent<Item>().CanInteract(Action.IDs.PickUp);
        }

        return false;
    }

    override public void Execute() 
    {
        this.Agent.PickUp(this.Targets[0].GetComponent<Item>());
    }
}
