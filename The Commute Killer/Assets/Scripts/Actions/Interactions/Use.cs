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
        if (target == null || target.GetComponent<Interactable>() == null) { return false; }

        var interactable = target.GetComponent<Interactable>();

        if(interactable.CanInteract(this.Agent.GetComponent<Agent>(), Action.IDs.Use))
        {
            return true;
        }

        return false;
    }
}
