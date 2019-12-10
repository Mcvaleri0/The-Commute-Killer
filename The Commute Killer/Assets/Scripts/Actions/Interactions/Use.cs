using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : Interaction
{
    public Use(Agent agent, GameObject target) : base(agent, target) {
        this.ID = IDs.Use;

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

        if(this.Interactable.CanInteract(this.Agent, Action.IDs.Use))
        {
            return true;
        }

        return false;
    }
}
