using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathfindingManager : MonoBehaviour {

    // Properties
    #region Navigation
    private Vector3 StartPosition;

	private Vector3 EndPosition;

    private NavigationManager NavManager;
    #endregion


    #region Pathfinding
    public PathfindingAlgorithm PathFinding;

    private GlobalPath CurrentSolution;

    public PAlgorithm Algorithm = PAlgorithm.AStar;

    public OpenSet   Open   = OpenSet.SimpleUnorderedList;
    public ClosedSet Closed = ClosedSet.SimpleUnorderedList;

    public Heuristic H = Heuristic.Zero;

    private bool SettingsChanged = false;

    #region Enums
    public enum Heuristic
    {
        Zero,
        Euclidean,
        Gateway
    }

    public enum PAlgorithm
    {
        AStar,
        NodeArrayAStar
    }

    public enum ClosedSet
    {
        SimpleUnorderedList,
        Dictionary,
        NodeRecordArray
    }

    public enum OpenSet
    {
        SimpleUnorderedList,
        NodePriorityHeap,
        NodeRecordArray
    }
    #endregion
    #endregion


    // MonoBehaviour Methods
    public void Start ()
	{
        // Get Scene's Navigation Manager
        this.NavManager = GameObject.Find("NavigationManager").GetComponent<NavigationManager>();

        // Default
        this.PathFinding = GetPathfinding(this.Algorithm, this.Open, this.Closed, this.H);
    }


    public void Update () 
    {
        //call the pathfinding method if the user specified a new goal
        if (this.PathFinding.InProgress)
        {
            var finished = this.PathFinding.Search(out this.CurrentSolution);

            if (finished)
            {
                if (this.CurrentSolution != null) Debug.Log("Success - " + this.PathFinding.TotalExploredNodes);

                this.PathFinding.InProgress = false;
            }

            return;
        }

        if(this.SettingsChanged)
        {
            this.SettingsChanged = false;

            this.PathFinding = GetPathfinding(this.Algorithm, this.Open, this.Closed, this.H);
        }
	}


    // Pathfinding Methods
    public void InitializePathFinding(Vector3 p1, Vector3 p2)
    {
        this.StartPosition = p1;
        this.EndPosition   = p2;

        this.CurrentSolution = null;

        this.PathFinding.InitializeSearch(this.StartPosition, this.EndPosition);
    }


    public GlobalPath GetCurrentSolution()
    {
        return this.CurrentSolution;
    }


    public GlobalPath GetCurrentSmoothSolution()
    {
        if(this.CurrentSolution != null)
        {
            return SmoothPath(this.StartPosition, this.CurrentSolution);
        }

        return null;
    }


    protected GlobalPath SmoothPath(Vector3 position, GlobalPath actual)
    {
        var smoothedPath = new GlobalPath();
        smoothedPath.PathPositions.Add(position);
        smoothedPath.PathPositions.AddRange(actual.PathPositions);

        smoothedPath.PathNodes.AddRange(actual.PathNodes);

        var i = 0;
        var j = 2;

        while (i + j < smoothedPath.PathPositions.Count)
        {
            var pos   = smoothedPath.PathPositions[i];
            var reach = smoothedPath.PathPositions[i + j];

            // Find the furthest node we can reach from current
            while(Walkable(pos, reach))
            {
                j++;

                if(i + j < smoothedPath.PathNodes.Count)
                {
                    reach = smoothedPath.PathPositions[i + j];
                }
                else
                {
                    break;
                }
            }

            j--;

            // Remove all nodes inbetween
            for (var k = 1; k < j; k++)
            {
                smoothedPath.PathNodes.RemoveAt(i + 1);
                smoothedPath.PathPositions.RemoveAt(i + 2);
            }

            // Add that path segment
            smoothedPath.LocalPaths.Add(new LineSegmentPath(pos, smoothedPath.PathPositions[i + 1]));

            i++;
            j = 2;
        }

        var count = smoothedPath.PathPositions.Count;

        smoothedPath.LocalPaths.Add(new LineSegmentPath(smoothedPath.PathPositions[count - 2], smoothedPath.PathPositions[count - 1]));

        return smoothedPath;
    }

    protected bool Walkable(Vector3 p1, Vector3 p2)
    {
        Vector3 direction = p2 - p1;
        return !Physics.Raycast(p1, direction, direction.magnitude);
    }


    #region AUX
    public void ChangeSettings(PAlgorithm p, OpenSet open, ClosedSet closed, Heuristic h)
    {
        if (this.Algorithm != p)
        {
            this.Algorithm = p;

            this.SettingsChanged = true;
        }

        if (this.Open != open)
        {
            this.Open = open;

            this.SettingsChanged = true;
        }

        if (this.Closed != closed)
        {
            this.Closed = closed;

            this.SettingsChanged = true;
        }

        if (this.H != h)
        {
            this.H = h;

            this.SettingsChanged = true;
        }
    }

    private IHeuristic GetHeuristic(Heuristic heuristic)
    {
        switch(heuristic)
        {
            default:
            case Heuristic.Zero:
                return new ZeroHeuristic();

            case Heuristic.Euclidean:
                return new EuclideanDistanceHeuristic();

            case Heuristic.Gateway:
                return new GatewayHeuristic(this.NavManager.GatewayDistanceTable);
        }
    }

    private IOpenSet GetOpenSet(OpenSet openSet)
    {
        switch(openSet)
        {
            default:
            case OpenSet.SimpleUnorderedList:
                return new SimpleUnorderedNodeList();

            case OpenSet.NodePriorityHeap:
                return new NodePriorityHeap();

            case OpenSet.NodeRecordArray:
                return new NodeRecordArray(new List<NavNode>(this.NavManager.Graph.Nodes));
        }
    }

    private IClosedSet GetClosedSet(ClosedSet closedSet)
    {
        switch (closedSet)
        {
            default:
            case ClosedSet.SimpleUnorderedList:
                return new SimpleUnorderedNodeList();

            case ClosedSet.Dictionary:
                return new ClosedSetDictionary();

            case ClosedSet.NodeRecordArray:
                return null; // It is supposed to be the same object for the Open and Closed sets
        }
    }

    private PathfindingAlgorithm GetPathfinding(PAlgorithm p, OpenSet open, ClosedSet closed, Heuristic h)
    {
        // Open Set
        var openSet = GetOpenSet(open);

        // Closed Set
        IClosedSet closedSet;

        if(closed != ClosedSet.NodeRecordArray)
        {
            closedSet = GetClosedSet(closed);
        }
        else
        {
            closedSet = (IClosedSet) openSet;
        }

        var heuristic = GetHeuristic(h);

        switch(p)
        {
            default:
            case PAlgorithm.AStar:
                return new AStarPathfinding(this.NavManager.Graph, openSet, closedSet, heuristic);

            case PAlgorithm.NodeArrayAStar:
                return new NodeArrayAStarPathFinding(this.NavManager.Graph, heuristic);
        }
    }
    #endregion


    #region Debugging
    public void OnDrawGizmos()
    {
        if (true)
        {
            if(this.PathFinding != null)
            {
                Gizmos.color = Color.red;

                var start = this.PathFinding.StartNode;

                if(start != null)
                {
                    Gizmos.DrawSphere(start.Position, 0.1f);
                }

                var goal = this.PathFinding.GoalNode;

                if (goal != null)
                {
                    Gizmos.DrawSphere(goal.Position, 0.1f);
                }
            }

            //draw the current Solution Path if any (for debug purposes)
            if (this.CurrentSolution != null)
            {
                var previousPosition = this.StartPosition;
                foreach (var pathPosition in this.CurrentSolution.PathPositions)
                {
                    Debug.DrawLine(previousPosition, pathPosition, Color.red);
                    previousPosition = pathPosition;
                }
            }

            //draw the nodes in Open and Closed Sets
            if (this.PathFinding != null)
            {

                if (this.PathFinding.Open != null)
                {
                    foreach (var nodeRecord in this.PathFinding.Open.All())
                    {
                        Gizmos.color = Mathf.CorrelatedColorTemperatureToRGB(nodeRecord.fValue);
                        Gizmos.DrawSphere(nodeRecord.node.Position, 0.1f);
                    }
                }

                /*
                Gizmos.color = Color.black;

                if (this.PathFinding.Closed != null)
                {
                    foreach (var nodeRecord in this.PathFinding.Closed.All())
                    {
                        Gizmos.DrawSphere(nodeRecord.node.Position, 0.1f);
                    }
                }
                */
            }

        }
    }
    #endregion
}
