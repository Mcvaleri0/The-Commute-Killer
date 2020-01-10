using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class AutonomousAgent : Agent
{
    #region /* Pathfinding */
    private PathfindingManager PathfindingM;

    private GlobalPath Path;
    #endregion


    #region /* Movement */
    private int MovementState = 0; //[ 0 - Stopped | 1 - Moving | 2 - Stopped at Goal ]

    private DynamicCharacter DCharacter;

    public float speed;

    public bool Target = false;

    #region /* Movement Targets */
    private Vector3 InitialGoalPosition { get; set; }

    private Vector3 InitialPosition { get; set; }

    public bool GoalHome { get; private set; } // True if it's current goal is the initial position

    private EventManager EventManager { get; set; }
    #endregion
    #endregion


    #region/* Routine */
    private int RoutineState = 0; //[ 0 - Idle | 1 - Acting ]

    public RoutineManager RoutineM;

    public Action CurrentAction { get; private set; }
    #endregion


    #region === Unity Events ===
    new void Start()
    {
        base.Start();

        this.PathfindingM = GetComponent<PathfindingManager>();

        this.RoutineM = GetComponent<RoutineManager>();

        this.DCharacter = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = this.Attributes[Attribute.Speed],
            Drag = this.Attributes[Attribute.Drag],
            Controller = GetComponent<CharacterController>()
        };

        this.EventManager = GameObject.Find("EventManager").GetComponent<EventManager>();

        this.InitialGoalPosition = this.GoalPosition;
        this.InitialPosition     = this.transform.position;
        this.GoalHome = false;
    }

    new void Update()
    {
        base.Update();

        if(this.Attributes[Attribute.HP] <= 0)
        {
            return;
        }

        RoutineStateMachine();

        MovementStateMachine();
    }
    #endregion


    #region === Movement Methods ===
    private void MovementStateMachine()
    {
        switch (this.MovementState)
        {
            case 0: // Stopped
                if (this.GoalPosition != null && Vector3.Distance(this.transform.position, this.GoalPosition) >= 1f)
                {
                    InitializeMovement();

                    this.MovementState = 1;
                }
                break;

            case 1: // Looking for Path
                var solution = this.PathfindingM.GetCurrentSolution();

                if(solution != null)
                {
                    this.Path = solution;

                    this.DCharacter.Movement = new DynamicFollowPath()
                    {
                        Path = this.Path,
                        MaxSpeed = this.Attributes[Attribute.Speed],
                        MaxAcceleration = this.Attributes[Attribute.Accelaration],
                        PathOffset = 1f,
                        PathManager = this.Pathfinding.NavManager
                    };

                    this.MovementState = 2;
                }
                break;

            case 2: // Moving to Target
                if (this.Path == null) {
                    if (Vector3.Distance(this.transform.position, this.GoalPosition) < 1f)
                    {
                        this.MovementState = 0;
                    }

                    break;
                }

                MoveToTarget();

                if (Vector3.Distance(this.transform.position, this.GoalPosition) < 1f)
                {
                    this.MovementState = 0; // Path completed -> Goal Reached

                    if (this.Target)
                    {
                        this.gameObject.SetActive(false);

                        this.EventManager.TriggerEvent(Event.VictimAtGoal);
                    }

                    else
                    {
                        this.gameObject.GetComponent<Animator>().SetBool("isIdling", true);
                    }
                }
                break;

            case 3:
                break;
        }
    }

    private void InitializeMovement()
    {
        var start = this.transform.position;
        var goal  = this.GoalPosition;

        this.PathfindingM.InitializePathFinding(start, goal);
    }

    private bool MoveToTarget()
    {
        if (this.DCharacter.MovementPossible())
        {
            this.DCharacter.Update();
        }
        else
        {
            this.InitializeMovement();
            this.MovementState = 1;
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

        this.InitializeMovement();
    }
    #endregion


    #region === Routine Methods ===
    private void RoutineStateMachine()
    {
        switch(this.RoutineState)
        {
            case 0: // Idle
                // Get next Action
                var nextAction = this.RoutineM.NextAction();

                // If there is an Action
                if (nextAction != null)
                {
                    this.RoutineState = 1; // Go to Executing

                    this.CurrentAction = nextAction;

                    ExecuteAction(this.CurrentAction);
                }
                break;

            case 1: // Acting

                // If the current Action is finished
                if(this.CurrentAction.Finished())
                {
                    this.RoutineState = 0; // Go to Idle
                }
                break;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if(this.Path != null)
        {
            Gizmos.color = Color.blue;

            Vector3 prev = Vector3.zero;

            foreach(Vector3 p in this.Path.PathPositions)
            {
                if(prev != Vector3.zero)
                {
                    Gizmos.DrawLine(prev, p);
                }

                prev = p;
            }

            Gizmos.DrawWireSphere(this.DCharacter.Movement.Target.position, 0.2f);
        }

        Gizmos.DrawSphere(this.GoalPosition, 0.25f);
    }
}
