using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : Interaction
{
    public Sleep(Agent agent, GameObject target) : base(agent, target)
    {
        this.ID = IDs.Sleep;

        this.Instrument = null;
    }

    public override void Update()
    {

    }

    override public bool CanExecute()
    {
        var target = this.Targets[0];

        // Target must be an interactable
        if (this.Interactable == null) { return false; }

        if (this.Interactable.CanInteract(this.Agent, Action.IDs.Sleep))
        {
            return true;
        }

        return false;
    }
}
