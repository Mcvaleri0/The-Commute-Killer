using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public enum IDs
    {
        None,
        PickUp,
        Drop,
        Stab,
        Sabotage,
        Use,
        Insert,
        Read
    }

    public IDs ID;

    protected Agent Agent { get; set; }

    protected List<Agent> Experiencers { get; set; }

    protected Item Instrument { get; set; }

    protected List<GameObject> Targets { get; set; }

    protected float Duration { get; set; }

    protected Vector3 TargetPosition { get; set; }

    public int State { get; protected set; } = 0; // [ 0 - To Start | 1 - In Progress | 2 - Finished ]


    public Action(Agent agent) {
        this.Agent = agent;
    }

    public Agent GetAgent()
    {
        return this.Agent;
    }

    public virtual void Update() { }

    public virtual bool CanExecute() { return false; }

    public virtual void Execute() { }

    public bool Finished()
    {
        return this.State == 2;
    }

    public static Action GetAction(Action.IDs id, Agent actor, GameObject target = null)
    {
        switch (id)
        {
            default:
                break;

            case Action.IDs.Sabotage:
                return new Sabotage(actor, target);

            case Action.IDs.Stab:
                return new Stab(actor, target);

            case Action.IDs.PickUp:
                return new PickUp(actor, target);

            case Action.IDs.Drop:
                return new Drop(actor, target);

            case Action.IDs.Use:
                return new Use(actor, target);

            case Action.IDs.Insert:
                return new Insert(actor, target);

            case Action.IDs.Read:
                return new Read(actor, target);
        }

        return null;
    }
}
