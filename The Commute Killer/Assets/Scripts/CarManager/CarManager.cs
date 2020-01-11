using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class CarManager : MonoBehaviour
{
    #region /* Cars info */

    /// <summary>
    /// List of possible types of car
    /// </summary>
    public List<GameObject> Prefabs;

    /// <summary>
    /// Proper Y values for each Car type
    /// </summary>
    private float[] ProperY { get; set; }

    /// <summary>
    /// Parent to all instantiated cars
    /// </summary>
    private Transform Cars { get; set; }

    public float MaxSpeed;
    public float LookAHeadDistance;

    public float VisionAngleRightCar;
    public float VisionAngleLeftCar;

    #endregion

    #region /* Positions */
    // This region is considering that the alley is on the right side of the road

    private bool RightLane { get; set; }

    /// <summary>
    /// Position where the car is instanciated when it belongs to the right lane
    /// </summary>
    public Vector3 InitialRightPosition;

    public Vector3 GoalRightPosition;

    /// <summary>
    /// Position where the car is instanciated when it belongs to the left lane
    /// </summary>
    public Vector3 InitialLeftPosition;
    
    public Vector3 GoalLeftPosition;

    #endregion

    #region /* Time Intervals */

    private TimeManager TimeManager { get; set; }

    private DateTime TimeNextCar { get; set; }

    public double TimeInterval;

    #endregion


    #region === Unity Events ===

    private void Start()
    {
        // This values were chosen empirically
        this.ProperY = new float[] { -0.356f, -0.357f, -0.33f, -0.335f, /*-0.193f,*/ -0.164f, -0.424f };

        if (this.Prefabs.Count != this.ProperY.Length)
        {
            throw new Exception("Each car type must have a proper y value");
        }

        this.Cars = this.transform.Find("Cars");
        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.UpdateTimeNextCar(this.TimeManager.GetCurrentTime());
    }

    private void Update()
    {
        if (this.TimeToInstanciateCar())
        {
            this.NewCar();
        }
    }

    #endregion

    #region === Car Control Methods ===

    public void NewCar()
    {
        int CarTypeIndex = this.ChooseCarType();
        GameObject CarType = this.Prefabs[CarTypeIndex];

        Vector3    Position;
        Quaternion Rotation;

        this.RightLane = this.ChooseLane();
        if (this.RightLane)
        {
            Position = this.InitialRightPosition;
            Rotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            Position = this.InitialLeftPosition;
            Rotation = Quaternion.Euler(-90, 0, 180);
        }
        Position = this.CorrectPosition(CarTypeIndex, Position);

        GameObject Car = Instantiate(CarType, Position, Rotation, this.Cars);
        this.InitializeCar(Car);
    }

    private void InitializeCar(GameObject Car)
    {
        Car.tag = "Car";

        Vector3 Scale = new Vector3(60, 60, 60);
        Car.transform.localScale = Scale;

        Car.AddComponent<MeshCollider>();

        Rigidbody body   = Car.AddComponent<Rigidbody>();
        body.isKinematic = true;

        var Controller = Car.AddComponent<CarController>();

        if (this.RightLane)
        {
            Controller.Initialize(this.MaxSpeed, this.GoalRightPosition, this.LookAHeadDistance, this.VisionAngleRightCar, this.RightLane);
        }
        else
        {
            Controller.Initialize(this.MaxSpeed, this.GoalLeftPosition, this.LookAHeadDistance, this.VisionAngleLeftCar, this.RightLane);
        }
    }

    #endregion

    #region === Time Control Methods ===

    private bool TimeToInstanciateCar()
    {
        DateTime CurrentTime = this.TimeManager.GetCurrentTime();

        if (CurrentTime >= this.TimeNextCar)
        {
            this.UpdateTimeNextCar(CurrentTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateTimeNextCar(DateTime CurrentTime)
    {
        this.TimeNextCar = CurrentTime.AddMinutes(this.TimeInterval);
    }

    #endregion

    #region === Auxiliary Functions ===

    private int ChooseCarType()
    {
        return Random.Range(0, this.Prefabs.Count);
    }

    private bool ChooseLane()
    {
        return Random.Range(0f, 1f) > 0.5;
    }

    private Vector3 CorrectPosition(int CarType, Vector3 DesiredPosition)
    {
        Vector3 Position = DesiredPosition;

        float ProperY = this.ProperY[CarType];
        Position.y += ProperY;

        return Position;
    }

    #endregion

}
