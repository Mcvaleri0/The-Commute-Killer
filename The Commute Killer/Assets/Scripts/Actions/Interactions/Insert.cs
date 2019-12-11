using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insert : Interaction
{
    public Insert(Agent agent, GameObject target) : base(agent, target) {
        this.ID = IDs.Insert;
    }

    override public bool CanExecute()
    {
        if(this.Interactable == null) { return false; } // Target must be an interactable

        // Agent must have a Cadaver on hand
        if (this.Agent.OnHand != null && this.Agent.OnHand.Name == "Cadaver") 
        {
            // Interactable has to allow Sabotage
            if (this.Interactable.CanInteract(this.Agent, Action.IDs.Insert))
            {
                return true;
            }
        }

        return false;
    }

    public override void Execute()
    {
        base.Execute();

        this.Agent.OnHand = null;

        Object.Destroy(this.Instrument.gameObject);
    }
}
