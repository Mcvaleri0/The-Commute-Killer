using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabotage : Interaction
{
    public Sabotage(Agent agent, GameObject target) : base(agent, target) {
        this.ID = IDs.Sabotage;
    }

    override public bool CanExecute(Agent agent, GameObject target)
    {
        if(target.GetComponent<Interactable>() == null) { return false; } // Target must be an interactable
        
        var agentComp = agent.GetComponent<Agent>();

        // Agent must have a Wrench on hand
        if (agentComp.OnHand != null && agentComp.OnHand.Name == "Wrench") 
        {
            var interactable = target.GetComponent<Interactable>();

            // Interactable has to allow Sabotage
            if (interactable.CanInteract(agent.GetComponent<Agent>(), Action.IDs.Sabotage))
            {
                return true;
            }
        }

        return false;
    }
}
