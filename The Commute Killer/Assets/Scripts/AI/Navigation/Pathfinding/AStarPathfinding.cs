using UnityEngine;

public class AStarPathfinding : PathfindingAlgorithm
{
    private int StepsSoFar = 0;

    public AStarPathfinding(NavGraph graph, IOpenSet open, IClosedSet closed, IHeuristic heuristic)
    {
        this.Graph = graph;

        base.Open   = open;
        base.Closed = closed;

        base.NodesPerFrame = uint.MaxValue; //by default we process all nodes in a single request

        this.Heuristic = heuristic;

        base.InProgress = false;
    }

    public override void InitializeSearch(Vector3 startPosition, Vector3 goalPosition)
    {
        this.StartPosition = startPosition;
        this.GoalPosition  = goalPosition;

        this.StartNode = this.Quantize(this.StartPosition);
        this.GoalNode  = this.Quantize(this.GoalPosition);

        //if it is not possible to quantize the positions and find the corresponding nodes, then we cannot proceed
        if (this.StartNode == null || this.GoalNode == null) return;

        /*
        this.OrigStartNode = ScriptableObject.CreateInstance<NavNode>();
        this.OrigStartNode.Initialize(-1, this.StartPosition);
        this.OrigStartNode.AddAdjacent(this.StartNode);

        this.OrigGoalNode = ScriptableObject.CreateInstance<NavNode>();
        this.OrigGoalNode.Initialize(-2, this.GoalPosition);
        this.GoalNode.AddAdjacent(this.OrigGoalNode);
        */

        this.InProgress = true; // Set On-Going Flag

        // Debugging
        this.TotalExploredNodes = 0;
        this.TotalProcessingTime = 0.0f;
        this.StartProcessingTime = Time.time;
        this.MaxConcurrentOpenNodes = 0;

        var initialNode = new NodeRecord
        {
            gValue = 0,
            hValue = this.Heuristic.H(this.StartNode, this.GoalNode),
            node   = this.StartNode
        };

        initialNode.fValue = AStarPathfinding.F(initialNode);

        this.Open.Initialize(); 
        this.Open.AddToOpen(initialNode);
        this.Closed.Initialize();
    }

    protected virtual void ProcessChildNode(NodeRecord parentNode, NavNode toNode)
    {
        //this is where you process a child node 
        var childNode = GenerateChildNodeRecord(parentNode, toNode);

        var inOpen   = Open.SearchInOpen(childNode);
        var inClosed = Closed.SearchInClosed(childNode);

        string step = this.StepsSoFar.ToString() + "# ";

        // If the child node is NOT in Open or Closed (Unvisited)
        if (inOpen == null && inClosed == null)
        {
            Open.AddToOpen(childNode);

            Debug.Log(step + "Opened it.");
        }

        // If child is in Open
        else if (inOpen != null)
        {
            if (inOpen.fValue > childNode.fValue)
            {
                Open.Replace(inOpen, childNode);

                Debug.Log(step + "Replaced in open.");
            }
        }

        // If child is in Closed
        else
        {
            if (inClosed.fValue > childNode.fValue)
            {
                Closed.RemoveFromClosed(inClosed);
                Open.AddToOpen(childNode);

                Debug.Log(step + "Removed from closed.");
            }
        }
    }

    public override bool Search(out GlobalPath solution, bool returnPartialSolution = false)
    {
        var openCount = Open.CountOpen();

        if (openCount == 0)
        {
            solution = null;

            return true;
        }

        if(openCount > MaxConcurrentOpenNodes)
        {
            MaxConcurrentOpenNodes = openCount;
        }

        string step = this.StepsSoFar.ToString() + "# ";

        var bestNode = Open.GetBestAndRemove();

        Debug.Log(step + "Best Node:" + bestNode.node.Id.ToString());

        if (bestNode.node == GoalNode)
        {
            solution = CalculateSolution(bestNode, false);
            TotalProcessingTime = Time.time - StartProcessingTime;
            Debug.Log(step + "Success!!!");
            return true;
        }

        Closed.AddToClosed(bestNode);

        var outConnections = bestNode.node.Adjacents.Count;

        for (int i = 0; i < outConnections; i++)
        {
            Debug.Log(step + "Connection - " + bestNode.node.Adjacents[i].Id);

            this.ProcessChildNode(bestNode, bestNode.node.Adjacents[i]);
        }

        TotalExploredNodes++;

        Debug.Log(step + "Open Nodes: " + Open.CountOpen().ToString());
        Debug.Log(step + "Total Explored: " + TotalExploredNodes);
        this.StepsSoFar++;

        solution = null;
		return false;
    }

    protected NavNode Quantize(Vector3 position)
    {
        return this.Graph.FindClosestNode(position); //FIXME?
    }

    /*
    protected void CleanUp()
    {
        //I need to remove the connections created in the initialization process
        if (this.StartNode != null)
        {
            ((NavMeshPoly)this.StartNode).RemoveConnectedPoly();
        }

        if (this.GoalNode != null)
        {
            ((NavMeshPoly)this.GoalNode).RemoveConnectedPoly();    
        }
    }
    */

    protected virtual NodeRecord GenerateChildNodeRecord(NodeRecord parent, NavNode childNode)
    {
        var childNodeRecord = new NodeRecord
        {
            node   = childNode,
            parent = parent,
            gValue = parent.gValue + (childNode.Position-parent.node.Position).magnitude,
            hValue = this.Heuristic.H(childNode, this.GoalNode)
        };

        childNodeRecord.fValue = F(childNodeRecord);

        return childNodeRecord;
    }


    protected GlobalPath CalculateSolution(NodeRecord node, bool partial)
    {
        var path = new GlobalPath
        {
            IsPartial = partial,
            Length = node.gValue
        };

        var currentNode = node;

        Vector3 pos2 = this.GoalPosition;
        path.PathPositions.Add(pos2);

        //I need to remove the first Node and the last Node because they correspond to the dummy first and last Polygons that were created by the initialization.
        //And we don't want to be forced to go to the center of the initial polygon before starting to move towards my destination.

        //skip the last node, but only if the solution is not partial (if the solution is partial, the last node does not correspond to the dummy goal polygon)
        if (!partial && currentNode.parent != null)
        {
            currentNode = currentNode.parent;
        }

        Vector3 pos1 = currentNode.node.Position;
        path.LocalPaths.Add(new LineSegmentPath(pos1, pos2));

        while (currentNode.parent != null)
        {
            path.PathNodes.Add(currentNode.node); //we need to reverse the list because this operator add elements to the end of the list
            path.NumberNodes++;

            pos2 = currentNode.node.Position;
            path.PathPositions.Add(pos2);

            if (currentNode.parent.parent == null) break; //this skips the first node
            if (currentNode.parent.parent == currentNode) break; // I f*cked up...

            pos1 = currentNode.parent.node.Position;

            path.LocalPaths.Add(new LineSegmentPath(pos1, pos2));

            currentNode = currentNode.parent;
        }

        path.PathNodes.Reverse();
        path.PathPositions.Reverse();
        path.LocalPaths.Reverse();

        return path;

    }


    public static float F(NodeRecord node)
    {
        return F(node.gValue,node.hValue);
    }


    public static float F(float g, float h)
    {
        return g + h;
    }
}
