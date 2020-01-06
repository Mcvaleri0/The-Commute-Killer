using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathfindingManager : MonoBehaviour {

    // Properties
    #region Navigation
    private Vector3 StartPosition;

	private Vector3 EndPosition;

    private NavigationManager NavManager;

	private NavGraph Graph;

    private NavClusterGraph ClusterGraph;
    #endregion

    #region Aux    
    private bool Draw;
    #endregion

    #region Pathfinding
    public PathfindingAlgorithm PathFinding;

    private GlobalPath CurrentSolution;

    private struct SettingsStruct
    {
        public PAlgorithm pathalgorithm;

        public OpenSet   openset;
        public ClosedSet closedset;

        public Heuristic heuristic;

        public bool Changed;
    }

    private SettingsStruct Settings;

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

        // Get Scene's NavGraph
        this.Graph = this.NavManager.Graph;

        // Get Scene's NavClusterGraph
        this.ClusterGraph = this.NavManager.ClusterGraph;

        this.Settings = new SettingsStruct()
        {
            pathalgorithm = PAlgorithm.AStar,
            openset       = OpenSet.SimpleUnorderedList,
            closedset     = ClosedSet.SimpleUnorderedList,
            heuristic     = Heuristic.Zero,
            Changed       = false
        };

        // Default
        this.PathFinding = GetPathfinding(PAlgorithm.AStar, OpenSet.SimpleUnorderedList, ClosedSet.SimpleUnorderedList, Heuristic.Zero);

        this.Draw = false; // Debugging
    }


    public void Update () 
    {
        //call the pathfinding method if the user specified a new goal
        if (this.PathFinding.InProgress)
        {
            var finished = this.PathFinding.Search(out this.CurrentSolution);

            if (finished)
            {
                this.PathFinding.InProgress = false;
                this.Draw = false;
            }

            return;
        }

        if(this.Settings.Changed)
        {
            this.Settings.Changed = false;

            this.PathFinding = GetPathfinding(this.Settings.pathalgorithm, this.Settings.openset, this.Settings.closedset, this.Settings.heuristic);
        }
	}


    // Pathfinding Methods
    public void InitializePathFinding(Vector3 p1, Vector3 p2)
    {
        this.StartPosition = p1;
        this.EndPosition = p2;

        this.CurrentSolution = null;
        this.Draw = true;

        this.PathFinding.InitializeSearch(this.StartPosition, this.EndPosition);
    }


    public GlobalPath GetCurrentSolution()
    {
        return this.CurrentSolution;
    }


    public void ChangeSettings(PAlgorithm p, OpenSet open, ClosedSet closed, Heuristic h)
    {
        if(this.Settings.pathalgorithm != p)
        {
            this.Settings.pathalgorithm = p;

            this.Settings.Changed = true;
        }

        if (this.Settings.openset != open)
        {
            this.Settings.openset = open;

            this.Settings.Changed = true;
        }

        if (this.Settings.closedset != closed)
        {
            this.Settings.closedset = closed;

            this.Settings.Changed = true;
        }

        if (this.Settings.heuristic != h)
        {
            this.Settings.heuristic = h;

            this.Settings.Changed = true;
        }
    }


    #region AUX
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
                return new GatewayHeuristic(this.ClusterGraph);
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
                return new NodeRecordArray(new List<NavNode>(this.Graph.Nodes));
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
        IClosedSet closedSet = null;

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
                return new AStarPathfinding(this.Graph, openSet, closedSet, heuristic);

            case PAlgorithm.NodeArrayAStar:
                return new NodeArrayAStarPathFinding(this.Graph, heuristic);
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
