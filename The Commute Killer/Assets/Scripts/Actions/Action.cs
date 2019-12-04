using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected Agent Agent { get; set; }

    protected List<Agent> Experiencers { get; set; }

    protected Item Instrument { get; set; }

    protected List<GameObject> Targets { get; set; }

    protected float Duration { get; set; }

    protected Vector3 TargetPosition { get; set; }

    public int State = 0; // [ 0 - To Start | 1 - In Progress | 2 - Finished ]

    public Action(Agent agent) {
        this.Agent = agent;
    }

    public virtual void Update() { }

    public virtual bool CanExecute() { return false; }

    public virtual void Execute() { }
}
