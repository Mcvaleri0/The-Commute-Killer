using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfindingAlgorithm
{
    #region Navigation
    public NavGraph Graph { get; protected set; }

    //protected NavNode OrigGoalNode;

    public NavNode GoalNode { get; protected set; }

    //protected NavNode OrigStartNode;

    public NavNode StartNode { get; protected set; }
    #endregion

    #region Search
    public Vector3 StartPosition { get; protected set; }

    public Vector3 GoalPosition { get; protected set; }

    public IHeuristic Heuristic { get; protected set; }

    public bool InProgress { get; set; } // FIXME: This should have a protected set

    public IOpenSet Open { get; protected set; }

    public IClosedSet Closed { get; protected set; }
    #endregion


    // Constraints
    public uint NodesPerFrame;

    #region Debug
    public float TotalProcessingTime { get; protected set; }

    public int TotalExploredNodes { get; protected set; }

    public int MaxConcurrentOpenNodes { get; protected set; }

    protected float StartProcessingTime;
    #endregion

    public abstract void InitializeSearch(Vector3 startPosition, Vector3 goalPosition);

    public abstract bool Search(out GlobalPath solution, bool returnPartialSolution = false);
}
