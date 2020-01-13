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
    public float[] ProperY;/*-0.19f, */

    public float[] ColliderCenter;/*-0.038f,*/

    public float[] ColliderSize;


    /// <summary>
    /// Parent to all instantiated cars
    /// </summary>
    private Transform Cars { get; set; }

    public int N { get; set; }
    public int Limit;

    public float MaxSpeed;

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
        if (this.Prefabs.Count != this.ProperY.Length || 
            this.Prefabs.Count != this.ColliderCenter.Length ||
            this.Prefabs.Count != this.ColliderSize.Length)
        {
            throw new Exception("Each car type must have a proper y and collider values");
        }

        this.Cars = this.transform.Find("Cars");
        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.UpdateTimeNextCar(this.TimeManager.GetCurrentTime());
    }

    private void Update()
    {
        if (this.N < this.Limit && this.TimeToInstanciateCar())
        {
            this.NewCar();
            this.N++;
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
        this.InitializeCar(Car, CarTypeIndex);
    }

    private void InitializeCar(GameObject Car, int index)
    {
        Car.tag = "Car";

        Vector3 Scale = new Vector3(60, 60, 60);
        Car.transform.localScale = Scale;

        Car.AddComponent<MeshCollider>();

        Rigidbody body   = Car.AddComponent<Rigidbody>();
        body.isKinematic = true;

        Vector3 Position = new Vector3(this.ColliderCenter[index], 0, 0);
        Vector3 Size     = new Vector3(0.02f, 0.03f, this.ColliderSize[index]);
        CollisionDetector Detector = CollisionDetector.CreateCollisionDetector(Car.transform, Position, Size);

        AudioSource audio = Car.AddComponent<AudioSource>();
        audio.clip = Resources.Load("Audio/car_engine") as AudioClip;
        audio.spatialBlend = 1;
        audio.loop = true;
        audio.Play();

        CarController Controller = Car.AddComponent<CarController>();
        Controller.Manager = this;
        Controller.Detector = Detector;

        if (this.RightLane)
        {
            Controller.Initialize(this.MaxSpeed, this.GoalRightPosition);
        }
        else
        {
            Controller.Initialize(this.MaxSpeed, this.GoalLeftPosition);
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
        this.TimeNextCar = CurrentTime.AddSeconds(this.TimeInterval * this.TimeManager.TimeMultiplier);
    }

    #endregion

    #region === Auxiliary Functions ===

    private int ChooseCarType()
    {
        return Random.Range(0, this.Prefabs.Count);
    }

    private bool ChooseLane()
    {
        //return Random.Range(0f, 1f) > 0.5;
        return true;
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
