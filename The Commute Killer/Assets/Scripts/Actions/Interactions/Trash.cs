using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : Interaction
{
    public Trash(Agent agent, GameObject target) : base(agent, target) {
        this.ID = IDs.Trash;
    }

    override public bool CanExecute()
    {
        if(this.Interactable == null) { return false; } // Target must be an interactable

        #region Distance
        if (Vector3.Distance(this.Interactable.transform.position, this.Agent.transform.position) > 2f)
        {
            return false;
        }
        #endregion

        // Agent must have a Cadaver on hand
        if (this.Agent.OnHand != null && this.Agent.OnHand.Name == "TrashBag") 
        {
            // Interactable has to allow Sabotage
            if (this.Interactable.CanInteract(this.Agent, Action.IDs.Trash))
            {
                return true;
            }
        }

        return false;
    }

    public override void Execute()
    {
        base.Execute();

        if(this.Agent.OnHand.Name == "TrashBag")
        {
            var hasVicitim = this.Instrument.GetComponent<TrashBag>().GetContainsVictim();
            if (hasVicitim)
            {
                GameObject.Find("EventManager").GetComponent<EventManager>().TriggerEvent(Event.VictimAtDumpster);
            }
        }

        this.Agent.OnHand = null;
        Object.Destroy(this.Instrument.gameObject);
    }
}
