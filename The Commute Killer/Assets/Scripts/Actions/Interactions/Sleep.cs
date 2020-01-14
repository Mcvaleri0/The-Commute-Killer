using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : Interaction
{
    public Sleep(Agent agent, GameObject target) : base(agent, target)
    {
        this.ID = IDs.Sleep;

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
        if (Vector3.Distance(this.Interactable.transform.position, this.Agent.transform.position) > 5f)
        {
            return false;
        }
        #endregion
        #endregion

        // Interaction must be possible
        if (!this.Interactable.CanInteract(this.Agent, Action.IDs.Sleep)) return false;

        return true;
    }
}
