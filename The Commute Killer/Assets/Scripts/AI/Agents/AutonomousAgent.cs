using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class AutonomousAgent : Agent
{
    #region /* Movement */
    
    public MapController Map;

    private int State = 0; //[ 0 - Stopped | 1 - Moving | 2 - Stopped at Goal ]

    private MapNode[] Path;

    private int NextInd;

    private DynamicCharacter DCharacter;

    #endregion


    #region /* Movement Targets */
    private Vector3 InitialGoalPosition { get; set; }

    private Vector3 InitialPosition { get; set; }

    public bool GoalHome {  get; private set; } // True if it's current goal is the initial position

    private EventManager EventManager { get; set; }

    #endregion


    #region === Unity Events ===

    new void Start()
    {
        base.Start();

        if(GameObject.Find("Map") != null)
        {
            this.Map = GameObject.Find("Map").GetComponent<MapController>();
        }

        this.EventManager = GameObject.Find("EventManager").GetComponent<EventManager>();

        this.DCharacter = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = this.Attributes[Attribute.Speed],
            Drag = 0.5f,
            Collider = GetComponent<CharacterController>()
        };

        this.InitialGoalPosition = this.GoalPosition;
        this.InitialPosition = this.transform.position;
        this.GoalHome = false;
    }

    new void Update()
    {
        base.Update();

        if(this.Attributes[Attribute.HP] <= 0)
        {
            return;
        }

        MovementStateMachine();

        this.DCharacter.Update();
    }

    #endregion


    #region === Movement Functions ===
    private void MovementStateMachine()
    {
        switch (this.State)
        {
            case 0: // Stopped
                if (this.GoalPosition != null)
                {
                    InitializeMovement();

                    if (this.Path != null) this.State = 1; // Path was found -> Walk toward it
                }
                break;

            case 1: // Moving to Target
                if (MoveToTarget())
                {
                    if (Vector3.Distance(this.transform.position, this.GoalPosition) < 1f)
                    {
                        this.State = 0; // Path completed -> Goal Reached

                        this.gameObject.SetActive(false);

                        this.EventManager.TriggerEvent(Event.VictimAtGoal);
                    }
                }
                else
                {
                    this.State = 0; // Path was blocked -> Recalculate
                }
                break;
        }
    }

    private void InitializeMovement()
    {
        var start = this.transform.position;
        var goal  = this.GoalPosition;

        Debug.Log("AutonomousAgent -> Calculating path...");

        if(this.Map != null)
        {
            this.Path = this.Map.GetPath(start, goal);
        }
        else
        {
            if(GameObject.Find("Map") == null)
            {
                return;
            }

            this.Map = GameObject.Find("Map").GetComponent<MapController>();

            if(this.Map == null)
            {
                return;
            }

            this.Path = this.Map.GetPath(start, goal);
        }

        Debug.Log("AutonomousAgent -> path calculated!!!");

        if (this.Path != null)
        {
            this.NextInd = 0;

            this.DCharacter.Movement = new DynamicArrive()
            {
                Character = this.DCharacter.KinematicData,
                Target = new Assets.Scripts.IAJ.Unity.Movement.KinematicData()
                {
                    position = this.Path[0].transform.position,
                    velocity = new Vector3(1, 1, 1)
                },
                MaxAcceleration = 1f,
                MaxSpeed        = this.Attributes[Attribute.Speed],
                TargetRadius    = 1f,
                SlowRadius      = 3f
            };

            this.gameObject.SetActive(true);
        }
    }

    private bool MoveToTarget()
    {
        // If the path through the grid is not finished
        if (this.NextInd < this.Path.Length)
        {
            var nextNode = this.Path[this.NextInd];

            var distance = Vector3.Distance(this.transform.position, nextNode.transform.position);

            // If we are close to the current next node
            if (distance <= 0.5f)
            {
                if (this.NextInd != this.Path.Length - 1)
                {
                    // If the path to the potential next node is not blocked
                    if (!this.Map.PathBlocked(nextNode.id, this.Path[this.NextInd + 1].id))
                    {
                        this.NextInd++; // New next node

                        this.DCharacter.Movement.Target = new Assets.Scripts.IAJ.Unity.Movement.KinematicData()
                        {
                            position = this.Path[this.NextInd].transform.position,
                            velocity = new Vector3(1, 1, 1)
                        };
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    this.NextInd++;

                    this.DCharacter.Movement.Target = new Assets.Scripts.IAJ.Unity.Movement.KinematicData()
                    {
                        position = this.GoalPosition,
                        velocity = new Vector3(1, 1, 1)
                    };
                }
            }
        }

        return true;
    }

    public void ToogleGoalPosition()
    {
        if (this.GoalHome)
        {
            this.GoalPosition = this.InitialGoalPosition;
            this.GoalHome = false;
        }
        else
        {
            this.GoalPosition = this.InitialPosition;
            this.GoalHome = true;
        }

        Debug.Log("AutonomousAgent -> New Goal: " + this.GoalPosition);

        this.InitializeMovement();
    }

    #endregion
}
