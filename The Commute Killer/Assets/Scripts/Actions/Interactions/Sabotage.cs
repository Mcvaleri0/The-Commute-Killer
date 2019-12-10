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
        var target = this.Targets[0];

        if(target == null || target.GetComponent<Interactable>() == null) { return false; } // Target must be an interactable

        // Agent must have a Wrench on hand
        if (this.Agent.OnHand != null && this.Agent.OnHand.Name == "Wrench") 
        {
            var interactable = target.GetComponent<Interactable>();

            // Interactable has to allow Sabotage
            if (interactable.CanInteract(this.Agent, Action.IDs.Sabotage))
            {
                return true;
            }
        }

        return false;
    }
}
