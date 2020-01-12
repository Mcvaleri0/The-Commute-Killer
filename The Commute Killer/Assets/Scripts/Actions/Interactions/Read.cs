using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Read : Interaction
{
    public Read(Agent agent, GameObject target) : base(agent, target)
    {
        this.ID = IDs.Read;

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

        if (this.Interactable.CanInteract(this.Agent, Action.IDs.Read))
        {
            return true;
        }

        return false;
    }
}
