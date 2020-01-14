using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Read : Interaction
{
    public Read(Agent agent, GameObject target) : base(agent, target)
    {
        this.ID = IDs.Read;

        this.Instrument = null;
    }

    public override void Update()
    {
        Execute();
        this.State = 2;
    }

    override public bool CanExecute()
    {
        // Target must be an interactable
        if (this.Interactable == null) { return false; }

        #region Distance
        if (Vector3.Distance(this.Interactable.transform.position, this.Agent.transform.position) > 5f)
        {
            return false;
        }
        #endregion

        if (this.Interactable.CanInteract(this.Agent, Action.IDs.Read))
        {
            return true;
        }

        return false;
    }
}
