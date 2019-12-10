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

    override public bool CanExecute()
    {
        return true;
    }

    override public void Execute() 
    {
        this.Instrument.Animate();

        var target = this.Agent.GetInFront();

        if (target != null) 
        {
            target.Attributes[Agent.Attribute.HP] -= 10;
        }
    }
}
