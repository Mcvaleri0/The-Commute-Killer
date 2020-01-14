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
        Execute();
        this.State = 2;
    }

    override public bool CanExecute()
    {
        #region Interactable
        // Target must be an interactable
        if (this.Interactable == null) { return false; }

        #region Distance
        if(Vector3.Distance(this.Interactable.transform.position,this.Agent.transform.position) > 5f)
        {
            return false;
        }
        #endregion
        #endregion

        // Interaction must be viable
        if (!this.Interactable.CanInteract(this.Agent, Action.IDs.Use)) return false;

        return true;
    }
}
