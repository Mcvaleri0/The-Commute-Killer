using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class Agent : MonoBehaviour
{
    public Vector3 GoalPosition;

    public MapController Map;

    private int State = 0; //[ 0 - Stopped | 1 - Moving | 2 - Stopped at Goal ]

    private Vector3 PreviousGoalPosition;

    private MapNode[] Path;

    private int NextInd;

    private DynamicCharacter DCharacter;

    public Item OnHand;

    public List<Item> Inventory;

    // Start is called before the first frame update
    void Start()
    {
        this.PreviousGoalPosition = this.GoalPosition;

        this.DCharacter = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = 10f,
            Drag = 0.5f
        };

        this.Inventory = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(this.State)
        {
            case 0: // Stopped
                if(this.GoalPosition != null)
                {
                    InitializeMovement();

                    if(this.Path != null) this.State = 1; // Path was found -> Walk toward it
                }
                break;

            case 1: // Moving to Target
                if(MoveToTarget())
                {
                    if(Vector3.Distance(this.transform.position, this.GoalPosition) < 1f)
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
        var start = transform.position;
        var goal  = this.GoalPosition;

        if(this.Map != null)
        {
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
                MaxSpeed = 10f,
                TargetRadius = 1f,
                SlowRadius = 3f
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

    public void Equip(Item toEquip)
    {
        var ind = 0;

        foreach(Item item in this.Inventory)
        {
            if(item == toEquip)
            {
                this.Unequip();

                this.OnHand = item;

                this.Inventory.RemoveAt(ind);

                return;
            }

            ind++;
        }
    }

    public void Unequip()
    {
        if(this.OnHand != null)
        {
            this.Inventory.Add(this.OnHand);

            this.OnHand = null;
        }
    }
}
