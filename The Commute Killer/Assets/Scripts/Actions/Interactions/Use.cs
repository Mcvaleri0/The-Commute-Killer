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

    override public bool CanExecute(Agent agent, GameObject target)
    {
        if(target.GetComponent<Interactable>() == null) { return false; } // Target must be an interactable

        var aPos = agent.transform.position;
        var tPos = target.transform.position;

        // Agents must be at least this close
        if (Vector3.Distance(aPos, tPos) < 1.5f)
        {
            var interactable = target.GetComponent<Interactable>();

            if(interactable.CanInteract(agent.GetComponent<Agent>(), Action.IDs.Use))
            {
                return true;
            }
        }

        return false;
    }
}
