using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Action
{

    public Move(Agent agent, Vector3 targetPosition) : base(agent) {
        this.ID = IDs.Move;

        this.TargetPosition = targetPosition;
    }

    public override void Update()
    {
        var agent = (AutonomousAgent)this.Agent;

        switch(this.State)
        {
            case 0: // To Start
                agent.GoalPosition = this.TargetPosition;
                this.State = 1;
                break;

            case 1: // In Progress
                if(agent.GoalPosition.Equals(Vector3.positiveInfinity))
                {
                    this.State = 2;
                }
                break;

            case 2: // Finished
                break;
        }
    }

    override public bool CanExecute()
    {
        return true;
    }

    override public void Execute() 
    {
        Update();
    }
}
