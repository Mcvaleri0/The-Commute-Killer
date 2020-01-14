using UnityEngine;
using UnityEditor;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement;



public class DynamicFollowPath : DynamicArrive
{
    public Path Path { get; set; }
    public float PathOffset { get; set; }
    public float CurrentParam { get; set; }
    private float TargetParam { get; set; }
    public NavigationManager NavManager { get; set; }


    public DynamicFollowPath()
    {
        this.CurrentParam = -1;
        this.TargetParam  = 0;
        Target = new KinematicData();
    }


    public override string Name
    {
        get { return "Follow Path"; }
    }


    public override MovementOutput GetMovement()
    {
        // The target's position is only updated when the character is near it.
        // The target's position is always the end of the current segment,
        // that means that when the position is updated the character wants to
        // follow the next segment.

        // The while is just a safe measure for when the character is also near
        // the next target and thus the new target is the second next, and so on.
        while (Target.position == Vector3.zero ||
              (Target.position - Character.position).sqrMagnitude < (this.SlowRadius*this.SlowRadius)/2)
        {
            this.CurrentParam++;

            this.TargetParam = CurrentParam + PathOffset;

            Target.position = Path.GetPosition(this.TargetParam);
        }

        return base.GetMovement();
    }


    public override bool Possible()
    {
        GlobalPath global = (GlobalPath) this.Path;

        int index = Mathf.FloorToInt(this.TargetParam);
        // FIXME: right now it is always possible to reach the goal position from 
        //        the last node but that could not be the case thanks to path 
        //        smothing because the last node and the goal position could not
        //        be part of the same cluster.
        //        Without the smoth this is not a problem because, in that case,
        //        the last node and the goal position are always in the same cluster
        if (index + 1 >= global.PathNodes.Count) return true;

        NavNode TargetNode = global.PathNodes[index + 1];

        if (this.CurrentParam != -1)
        {
            index = Mathf.FloorToInt(this.CurrentParam);
        }

        if (index >= global.PathNodes.Count) return true;

        NavNode CurrentNode = global.PathNodes[index];

        return !this.NavManager.PathBlocked(CurrentNode, TargetNode);
    }
}