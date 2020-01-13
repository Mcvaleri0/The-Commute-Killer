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

        this.Instrument = agent.OnHand;
    }

    public override void Update()
    {
        switch(this.State)
        {
            case 0: // To Start
                this.Instrument.Animate();
                this.State = 1;
                break;

            case 1: // In Progress
                if(this.Instrument.AnimationState == 0)
                {
                    this.Execute();
                    this.State = 2;
                }
                break;

            case 2: // Finished
                break;
        }
    }

    override public bool CanExecute()
    {
        if(this.Instrument == null)
        {
            return false;
        }

        if(!this.Instrument.EnabledActions.Contains(Action.IDs.Stab) && 
            this.Instrument.DefaultAction != Action.IDs.Stab)
        {
            return false;
        }

        return true;
    }

    override public void Execute() 
    {
        this.Instrument.AudioSource.pitch = Random.Range(0.7f, 1.3f);
        this.Instrument.PlayActionSound(this.ID);

        this.Instrument.Animate();

        var target = this.Agent.GetInFront();

        if (target != null) 
        {
            target.Attributes[Agent.Attribute.HP] -= 10;
        }
    }
}
