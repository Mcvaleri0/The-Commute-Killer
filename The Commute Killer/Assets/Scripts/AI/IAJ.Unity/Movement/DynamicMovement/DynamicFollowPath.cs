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
        CurrentParam = Path.GetParam(Character.position, CurrentParam);

        float targetParam = CurrentParam + PathOffset;

        Target.position = Path.GetPosition(targetParam);

        return base.GetMovement();
    }
}