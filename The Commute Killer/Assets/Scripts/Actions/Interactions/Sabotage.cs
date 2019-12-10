using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabotage : Interaction
{
    public Sabotage(Agent agent, GameObject target) : base(agent, target) {
        this.ID = IDs.Sabotage;
    }

    override public bool CanExecute()
    {
        if(this.Interactable == null) { return false; } // Target must be an interactable

        // Agent must have a Wrench on hand
        if (this.Agent.OnHand != null && this.Agent.OnHand.Name == "Wrench") 
        {
            // Interactable has to allow Sabotage
            if (this.Interactable.CanInteract(this.Agent, Action.IDs.Sabotage))
            {
                return true;
            }
        }

        return false;
    }
}
