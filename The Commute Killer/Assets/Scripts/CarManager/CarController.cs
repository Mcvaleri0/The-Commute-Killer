using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarController : MonoBehaviour
{
    #region /* Movement */

    private float MaxSpeed { get; set; }

    /// <summary>
    /// This goal is a local position inside the Cars object
    /// </summary>
    private Vector3 Goal { get; set; }

    private Vector3 Direction { get; set; }

    #endregion


    #region === Unity Events ===

    private void Update()
    {
        this.UpdateMovement();
    }

    #endregion

    #region === Movement Methods ===

    public void Initialize(float Speed, Vector3 GoalPosition)
    {
        this.MaxSpeed = Speed;
        this.Goal = GoalPosition;

        this.Direction = Vector3.left;
    }

    private void UpdateMovement()
    {
        if (this.ReachGoal())
        {
            Destroy(this.gameObject);
        }
        else if (this.CanMove())
        {
            this.Move();
        }
    }

    private void Move()
    {
        this.transform.Translate(this.Direction * this.MaxSpeed * Time.deltaTime);
    }

    private bool ReachGoal()
    {
        return Vector3.Distance(this.transform.localPosition, Goal) <= 1f;
    }

    private bool CanMove()
    {
        return true;
    }


    #endregion

}
