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

    override public bool CanExecute(Agent agent, GameObject target)
    {
        if (target.GetComponent<Item>() == null) { return false; } // Target must be an agent

        var agentComp = agent.GetComponent<Agent>();
        var itemComp  = target.GetComponent<Item>();

        var invInd = agentComp.FindInInventory(itemComp);
        var inHand = agentComp.OnHand == itemComp;

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
