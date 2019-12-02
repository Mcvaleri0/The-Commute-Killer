using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected Agent Agent { get; set; }

    protected List<Agent> Experiencers { get; set; }

    protected Item Instrument { get; set; }

    protected List<Agent> Targets { get; set; }

    protected float Duration { get; set; }

    protected Vector3 TargetPosition { get; set; }

    public Action(Agent agent) {
        this.Agent = agent;
    }

    public virtual bool CanExecute()
    {
        return false;
    }

    public virtual void Execute() { }

    public virtual void ChangeWorld() { }
}
