using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : Action
{
    protected Interactable Interactable;

    public Interaction(Agent agent, GameObject target) : base(agent) {
        base.Targets = new List<GameObject>() 
        { 
            target
        };

        this.Instrument = agent.OnHand;

        if(target != null) this.Interactable = target.GetComponent<Interactable>();
    }

    public override void Update()
    {
        if (this.Instrument != null)
        {
            switch (this.State)
            {
                case 0: // To Start
                    this.Instrument.Animate();
                    break;

                case 1: // In Progress
                    if (this.Instrument.AnimationState == 2)
                    {
                        this.State = 2;
                    }
                    break;

                case 2: // Finished
                    this.Execute();
                    break;
            }
        }
    }

    override public void Execute() 
    {
        this.Interactable.Interact(this.ID);
    }
}
