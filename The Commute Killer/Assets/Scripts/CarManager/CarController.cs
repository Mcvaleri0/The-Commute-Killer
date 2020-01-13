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

    #region /* Collision */

    private float LookAheadSQR { get; set; }
    private float AngleVision { get; set; }

    private bool RightCar { get; set; }

    private int PossibleCollision { get; set; }

    #endregion

    public CarManager Manager { get; set; }
    private bool passed { get; set; }

    #region === Unity Events ===

    private void Update()
    {
        this.UpdateMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " collision enter");

        if (!this.passed)
        {
            this.passed = true;
        }
        else
        {
            this.PossibleCollision++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(this.gameObject.name + " collision exit");
        if (this.passed)
        {
            this.PossibleCollision--;
        }
    }



    #endregion

    #region === Movement Methods ===

    private void InitializeMovement(float Speed, Vector3 GoalPosition)
    {
        this.MaxSpeed = Speed;
        this.Goal = GoalPosition;

        this.Direction = Vector3.left;
    }

    private void UpdateMovement()
    {
        if (this.GoalReached())
        {
            Destroy(this.gameObject);
            this.Manager.N--;
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

    private bool GoalReached()
    {
        return Vector3.Distance(this.transform.localPosition, Goal) <= 1f;
    }

    private bool CanMove()
    {
        //return !this.DetectPossibleCollision();
        return this.PossibleCollision == 0;
    }


    #endregion

    #region === Collision Methods ===

    private void InitializeCollisionDetection(float LookAhead, float AngleVision, bool RightCar)
    {
        this.LookAheadSQR = LookAhead * LookAhead;
        this.AngleVision  = AngleVision;
        this.RightCar     = RightCar;
    }

    private bool DetectPossibleCollision()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        Vector3 diff;
        float   angle;

        foreach (GameObject npc in npcs)
        {
            diff = npc.transform.position - this.transform.position;

            if (diff.sqrMagnitude <= this.LookAheadSQR)
            {
                angle = Vector3.Angle(diff, this.Direction);

                if (this.RightCar && angle <= this.AngleVision)
                {
                    return true;
                }
                if (!this.RightCar && angle > this.AngleVision)
                {
                    return true;
                }
            }
        }

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        Vector3 carPos;
        Vector3 thisPos;

        foreach (GameObject car in cars)
        {
            carPos  = car.transform.position;
            thisPos = this.transform.position;

            carPos.y = thisPos.y = 0;
            
            diff = carPos - thisPos;

            if (diff != Vector3.zero && diff.sqrMagnitude <= 1.5 * this.LookAheadSQR)
            {
                angle = Vector3.Angle(diff, this.Direction);

                if (this.RightCar && angle == 0)
                {
                    return true;
                }
                else if (!this.RightCar && angle == 180)
                {
                    return true;
                }
            }

        }

        return false;
    }

    #endregion

    #region === Auxiliary Functions ===

    public void Initialize(float Speed, Vector3 GoalPosition, float LookAhead, float AngleVision, bool RightCar)
    {
        this.InitializeCollisionDetection(LookAhead, AngleVision, RightCar);

        this.InitializeMovement(Speed, GoalPosition);

        this.passed = false;
    }

    #endregion

}
