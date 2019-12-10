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

    override public bool CanExecute()
    {
        var target = Targets[0];

        if (target == null || target.GetComponent<Item>() == null) { return false; } // Target must be an item

        if (this.Agent.FirstFree != -1 || this.Agent.OnHand == null)
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
