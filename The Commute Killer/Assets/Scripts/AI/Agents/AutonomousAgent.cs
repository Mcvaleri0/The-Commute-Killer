using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class AutonomousAgent : Agent
{
    /* Movement */
    public MapController Map;

    private int State = 0; //[ 0 - Stopped | 1 - Moving | 2 - Stopped at Goal ]

    private Vector3 PreviousGoalPosition;

    private MapNode[] Path;

    private int NextInd;

    private DynamicCharacter DCharacter;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        this.Map = GameObject.Find("Map").GetComponent<MapController>();

        this.PreviousGoalPosition = this.GoalPosition;

        this.DCharacter = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = this.Attributes[Attribute.Speed],
            Drag     = 0.5f
        };
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if(this.Attributes[Attribute.HP] <= 0)
        {
            return;
        }

        MovementStateMachine();
    }

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

        if(this.Map != null)
        {
            this.Path = this.Map.GetPath(start, goal);
        }
        else
        {
            this.Map = GameObject.Find("Map").GetComponent<MapController>();

            if(this.Map == null)
            {
                return;
            }

            this.Path = this.Map.GetPath(start, goal);
        }

        if (this.Path != null)
        {
            this.NextInd = 0;

            this.DCharacter.Movement = new DynamicArrive()
            {
                Character = this.DCharacter.KinematicData,
                Target = new Assets.Scripts.IAJ.Unity.Movement.KinematicData()
                {
                    position = this.Path[0].transform.position
                },
                MaxAcceleration = 1f,
                MaxSpeed        = this.Attributes[Attribute.Speed],
                TargetRadius    = 1f,
                SlowRadius      = 3f
            };
        }
    }

    private bool MoveToTarget()
    {
        this.DCharacter.Update();

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
                            position = this.Path[this.NextInd].transform.position
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
                        position = this.GoalPosition
                    };
                }
            }
        }

        return true;
    }
}
