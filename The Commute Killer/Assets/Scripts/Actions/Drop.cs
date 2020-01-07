using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Action
{
    private Item Item;

    public Drop(Agent agent, GameObject target) : base(agent) {
        this.ID = IDs.Drop;

        this.Targets = new List<GameObject>()
        {
            target
        };

        if(target != null) this.Item = target.GetComponent<Item>();
    }

    override public bool CanExecute()
    {
        var target = this.Targets[0];

        if (target == null || this.Item == null) { return false; } // Target must be an item

        var invInd = this.Agent.FindInInventory(this.Item);
        var inHand = this.Agent.OnHand == this.Item;

        if (invInd != -1 || inHand)
        {
            return this.Item.CanInteract(Action.IDs.Drop);
        }

        return false;
    }

    override public void Execute() 
    {
        this.Agent.OnHand.PlayDropSound();
        this.Agent.Drop(this.Agent.InventorySize);
    }
}
