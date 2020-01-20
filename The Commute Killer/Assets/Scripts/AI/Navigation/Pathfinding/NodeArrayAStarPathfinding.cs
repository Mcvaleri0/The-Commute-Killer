using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeArrayAStarPathFinding : AStarPathfinding
{
    protected NodeRecordArray NodeRecordArray { get; set; }
    public NodeArrayAStarPathFinding(NavGraph graph, IHeuristic heuristic) : base(graph,null,null,heuristic)
    {
        this.NodeRecordArray = new NodeRecordArray(graph.Nodes.ToList());

        this.Open   = this.NodeRecordArray;
        this.Closed = this.NodeRecordArray;
    }

    protected override void ProcessChildNode(NodeRecord bestNode, NavNode toNode)
    {
        var childNode = toNode;

        var childNodeRecord = this.NodeRecordArray.GetNodeRecord(childNode);

        if (childNodeRecord == null)
        {
            //this piece of code is used just because of the special start nodes and goal nodes added to the RAIN Navigation graph when a new search is performed.
            //Since these special goals were not in the original navigation graph, they will not be stored in the NodeRecordArray and we will have to add them
            //to a special structure
            //it's ok if you don't understand this, this is a hack and not part of the NodeArrayA* algorithm, just do NOT CHANGE THIS, or your algorithm will not work
            childNodeRecord = new NodeRecord
            {
                node = childNode,
                parent = bestNode,
                status = NodeStatus.Unvisited
            };
            this.NodeRecordArray.AddSpecialCaseNode(childNodeRecord);
        }
            
        // Calculate Values for the node and update it
        float g = bestNode.gValue + (childNode.Position - bestNode.node.Position).magnitude;
        float h = base.Heuristic.H(childNode, base.GoalNode);
        float f = g + h;

        // If the node is unvisited
        if (childNodeRecord.status == NodeStatus.Unvisited)
        {
            childNodeRecord.Update(bestNode, g, h, f);
            childNodeRecord.status = NodeStatus.Open;
            this.NodeRecordArray.AddToOpen(childNodeRecord);
            this.TotalOpenedNodes++;
        }
        // If it is Open or Closed and the new found F is better
        else if(childNodeRecord.fValue > f)
        {
            childNodeRecord.Update(bestNode, g, h, f);

            childNodeRecord.status = NodeStatus.Open;

            if (childNodeRecord.status == NodeStatus.Closed)
            {
                this.NodeRecordArray.AddToOpen(childNodeRecord);
                this.TotalOpenedNodes++;
            }
        }
    }
}
