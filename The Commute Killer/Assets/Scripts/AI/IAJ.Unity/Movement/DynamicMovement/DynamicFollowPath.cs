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
    public NavigationManager PathManager { get; set; }


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


    public new bool Possible()
    {
        GlobalPath global = (GlobalPath)this.Path;

        int index = Mathf.FloorToInt(this.CurrentParam);
        NavNode CurrentNode = global.PathNodes[index];

        index = Mathf.FloorToInt(this.TargetParam);
        NavNode TargetNode = global.PathNodes[index];

        return !this.PathManager.PathBlocked(CurrentNode, TargetNode);
    }
}