using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarController : MonoBehaviour
{
    #region /* Movement */

    private KinematicData KinematicData { get; set; }

    private Rigidbody Body { get; set; }

    private DynamicMovement Movement { get; set; }

    private float Drag { get; set; }

    private float MaxSpeed { get; set; }

    private Vector3 Goal;

    #endregion


    #region === Unity Events ===

    private void Update()
    {
        if (Vector3.Distance(this.transform.localPosition, Goal) > 1f/* Movement possible */)
        {
            this.UpdateMovement();
        }
        else
        {
            Debug.Log("devia de ter parado");
        }
    }

    #endregion

    #region === Movement Methods ===

    public void Initialize(Rigidbody Body, Vector3 GoalPosition)
    {
        this.Body = Body;
        this.Drag = 0.1f;
        this.MaxSpeed = 1f;
        this.Goal = GoalPosition;

        this.KinematicData = new KinematicData(new StaticData(this.transform.position))
        {
            velocity = Vector3.one
        };

        this.Movement = new DynamicArrive()
        {
            MaxSpeed = this.MaxSpeed,
            Target = new KinematicData(new StaticData(GoalPosition)),
            Character = this.KinematicData,
            MaxAcceleration = 1f
        };
    }

    private void UpdateMovement()
    {
        this.transform.Translate(Vector3.left * Time.deltaTime);
    }

    #endregion

}
