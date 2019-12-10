using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Action
{
    public Drop(Agent agent, GameObject target) : base(agent) {
        this.ID = IDs.Drop;

        this.Targets = new List<GameObject>()
        {
            target
        };
    }

    override public bool CanExecute()
    {
        var target = this.Targets[0];

        if (target == null || target.GetComponent<Item>() == null) { return false; } // Target must be an item

        var itemComp  = target.GetComponent<Item>();

        var invInd = this.Agent.FindInInventory(itemComp);
        var inHand = this.Agent.OnHand == itemComp;

        if (invInd != -1 || inHand)
        {
            return target.GetComponent<Item>().CanInteract(Action.IDs.Drop);
        }

        return false;
    }

    override public void Execute() 
    {
        this.Agent.Drop(this.Agent.InventorySize);
    }
}
