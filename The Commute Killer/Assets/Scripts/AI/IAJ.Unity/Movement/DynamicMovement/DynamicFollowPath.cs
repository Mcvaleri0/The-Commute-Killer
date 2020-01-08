using UnityEngine;
using UnityEditor;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement;



public class DynamicFollowPath : DynamicArrive
{
    public Path Path { get; set; }
    public float PathOffset { get; set; }
    public float CurrentParam { get; set; }


    public DynamicFollowPath()
    {
        CurrentParam = 0;
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

            float targetParam = CurrentParam + PathOffset;

            Target.position = Path.GetPosition(targetParam);

            this.CurrentParam++;
        }

        return base.GetMovement();
    }
}