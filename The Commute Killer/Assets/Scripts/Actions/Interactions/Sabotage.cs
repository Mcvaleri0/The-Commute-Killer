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
        #region Interactable
        if (this.Interactable == null) { return false; } // Target must be an interactable

        #region Distance
        if (Vector3.Distance(this.Interactable.transform.position, this.Agent.transform.position) > 5f)
        {
            return false;
        }
        #endregion
        #endregion

        #region Instrument
        if (this.Instrument == null) { return false; } // There must be an instrument

        // Instrument must enable Sabotage
        if (!this.Instrument.EnabledActions.Contains(Action.IDs.Sabotage)) return false;
        #endregion

        // Interactable has to allow Sabotage
        if (!this.Interactable.CanInteract(this.Agent, Action.IDs.Sabotage)) return false;

        return true;
    }

    public override void Execute()
    {
        base.Execute();
        this.Instrument.PlayActionSound(this.ID);
    }
}
