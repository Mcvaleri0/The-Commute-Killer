using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : Action
{
    public Stab(Agent agent, Agent target) : base(agent) {
        base.Targets = new List<Agent>() 
        { 
            target
        };

        this.Instrument = agent.GetComponent<Agent>().OnHand;
    }

    public static bool CanExecute(GameObject agent, GameObject target)
    {
        var aPos = agent.transform.position;
        var tPos = target.transform.position;

        if (Vector3.Distance(aPos, tPos) < 1.5f)
        {
            if(agent.GetComponent<Agent>().OnHand.Type == Item.ItemType.SharpWeapon)
            {

            }
        }

        return false;
    }

    public virtual void Execute() { }

    public virtual void ChangeWorld() { }
}
