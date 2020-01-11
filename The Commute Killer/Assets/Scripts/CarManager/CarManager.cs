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

    #endregion

    #region /* Positions */
    // This region is considering that the alley is on the right side of the road

    /// <summary>
    /// Position where the car is instanciated when it belongs to the right lane
    /// </summary>
    public Vector3 InitialRightPosition;

    /// <summary>
    /// Position where the car is instanciated when it belongs to the left lane
    /// </summary>
    public Vector3 InitialLeftPosition;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
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

        Vector3 Position = this.CorrectPosition(CarTypeIndex, this.InitialRightPosition);

        Quaternion Rotation = Quaternion.Euler(-90, 0, 0);

        GameObject Car = Instantiate(CarType, Position, Rotation, this.Cars);
        this.InitializeCar(Car);
    }

    private void InitializeCar(GameObject Car)
    {
        Vector3 Scale = new Vector3(60, 60, 60);
        Car.transform.localScale = Scale;

        Car.AddComponent<MeshCollider>();

        Rigidbody body   = Car.AddComponent<Rigidbody>();
        body.isKinematic = true;

        var Controller  = Car.AddComponent<CarController>();
        Controller.Initialize(this.InitialLeftPosition);
    }

    #endregion

    #region === Auxiliary Functions ===

    private int ChooseCarType()
    {
        //int CarTypeIndex = Mathf.FloorToInt(Random.Range(0, this.Prefabs.Count-1));

        return 0;
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
