using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : Action
{
    public Stab(Agent agent, GameObject target) : base(agent) {
        this.ID = IDs.Stab;

        base.Targets = new List<GameObject>() 
        { 
            target
        };

        this.Instrument = agent.GetComponent<Agent>().OnHand;
    }

    public override void Update()
    {
        switch(this.State)
        {
            case 0: // To Start
                this.Instrument.Animate();
                break;

            case 1: // In Progress
                if(this.Instrument.AnimationState == 2)
                {
                    this.State = 2;
                }
                break;

            case 2: // Finished
                this.Execute();
                break;
        }
    }

    override public bool CanExecute(Agent agent, GameObject target)
    {
        if(target.GetComponent<Agent>() == null) { return false; } // Target must be an agent

        var aPos = agent.transform.position;
        var tPos = target.transform.position;

        // Agents must be at least this close
        if (Vector3.Distance(aPos, tPos) < 1.5f)
        {
            // Agent must have a Sharp Weapon on hand
            if(agent.GetComponent<Agent>().OnHand.Types.FindIndex(x => x == Item.ItemType.SharpWeapon) != -1)
            {
                return true;
            }
        }

        return false;
    }

    override public void Execute() 
    {
        var target = this.Targets[0].GetComponent<Agent>();

        target.Attributes[Agent.Attribute.HP] -= 10;
    }
}
