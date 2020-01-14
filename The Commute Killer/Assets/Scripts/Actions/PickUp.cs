using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Action
{
    private Item Item;

    public PickUp(Agent agent, GameObject target) : base(agent) {
        this.ID = IDs.PickUp;

        this.Targets = new List<GameObject>()
        {
            target
        };

        if(target != null) this.Item = target.GetComponent<Item>();
    }

    public override void Update()
    {
        Execute();
        this.State = 2;
    }

    override public bool CanExecute()
    {
        if (this.Item == null) { return false; } // Target must be an item

        #region Distance
        if (Vector3.Distance(this.Item.transform.position, this.Agent.transform.position) > 5f)
        {
            return false;
        }
        #endregion

        if (/*this.Agent.FirstFree != -1 ||*/ this.Agent.OnHand == null)
        {
            return this.Item.CanInteract(Action.IDs.PickUp);
        }

        return false;
    }

    override public void Execute() 
    {
        this.Agent.PickUp(this.Item);
        this.Item.PlayActionSound(this.ID);
    }
}
