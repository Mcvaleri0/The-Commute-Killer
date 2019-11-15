using UnityEngine;
using UnityEditor;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using Assets.Scripts.IAJ.Unity.Movement;



public class DynamicFollowPath : DynamicArrive
{
    public Path Path { get; set; }
    public float PathOffset { get; set; }
    public float CurrentParam { get; set; }


    public DynamicFollowPath()
    {
        CurrentParam = 0;
        DesiredTarget = new KinematicData();
    }


    public override string Name
    {
        get { return "Follow Path"; }
    }


    public override MovementOutput GetMovement()
    {
        CurrentParam = Path.GetParam(Character.Position, CurrentParam);

        float targetParam = CurrentParam + PathOffset;

        DesiredTarget.Position = Path.GetPosition(targetParam);

        return base.GetMovement();
    }
}