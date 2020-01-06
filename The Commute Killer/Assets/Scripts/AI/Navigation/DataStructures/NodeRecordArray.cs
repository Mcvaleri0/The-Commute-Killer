using System;
using System.Collections.Generic;

public class NodeRecordArray : IOpenSet, IClosedSet
{
    private NodeRecord[] NodeRecords { get; set; }
    private List<NodeRecord> SpecialCaseNodes { get; set; } 
    private NodePriorityHeap Open { get; set; }

    public NodeRecordArray(List<NavNode> nodes)
    {
        //this method creates and initializes the NodeRecordArray for all nodes in the Navigation Graph
        this.NodeRecords = new NodeRecord[nodes.Count];

        foreach(NavNode node in nodes)
        {
            this.NodeRecords[node.Id] = new NodeRecord { node = node, status = NodeStatus.Unvisited };
        }

        this.SpecialCaseNodes = new List<NodeRecord>();

        this.Open = new NodePriorityHeap();
    }

    public NodeRecord GetNodeRecord(NavNode node)
    {
        //do not change this method
        //here we have the "special case" node handling
        if (node.Id == -1)
        {
            for (int i = 0; i < this.SpecialCaseNodes.Count; i++)
            {
                if (node == this.SpecialCaseNodes[i].node)
                {
                    return this.SpecialCaseNodes[i];
                }
            }
            return null;
        }
        else
        {
            return  this.NodeRecords[node.Id];
        }
    }

    public void AddSpecialCaseNode(NodeRecord node)
    {
        this.SpecialCaseNodes.Add(node);
    }

    void IOpenSet.Initialize()
    {
        this.Open.Initialize();

        //we want this to be very efficient (that's why we use for)
        for (int i = 0; i < this.NodeRecords.Length; i++)
        {
            this.NodeRecords[i].status = NodeStatus.Unvisited;
        }

        this.SpecialCaseNodes.Clear();
    }

    void IClosedSet.Initialize()
    {
        return;
    }

    //Is Unvisited
    public void AddToOpen(NodeRecord nodeRecord)
    {
        //This safe guards the use of this structure with normal A*
        NodeRecord savedRecord = NodeRecords[nodeRecord.node.Id];

        nodeRecord.status = NodeStatus.Open;

        Replace(savedRecord, nodeRecord);
        Open.AddToOpen(nodeRecord);
    }

    // Is Open
    public NodeRecord SearchInOpen(NodeRecord nodeRecord)
    {
        return Open.SearchInOpen(nodeRecord);
    }

    public void RemoveFromOpen(NodeRecord nodeRecord)
    {
        var savedRecord = NodeRecords[nodeRecord.node.Id];
        Open.RemoveFromOpen(savedRecord);
        savedRecord.status = NodeStatus.Closed;
    }

    public NodeRecord GetBestAndRemove()
    {
        var foundRecord = Open.GetBestAndRemove();
        foundRecord.status = NodeStatus.Closed;

        return foundRecord;
    }

    public NodeRecord PeekBest()
    {
        return Open.PeekBest();
    }

    // Is Closed
    public void AddToClosed(NodeRecord nodeRecord)
    {
        NodeRecord savedRecord = NodeRecords[nodeRecord.node.Id];
        nodeRecord.status = NodeStatus.Closed;
        Replace(savedRecord, nodeRecord);
    }

    public NodeRecord SearchInClosed(NodeRecord nodeRecord)
    {
        var savedRecord = NodeRecords[nodeRecord.node.Id];

        if(savedRecord.status == NodeStatus.Closed)
        {
            return savedRecord;
        }

        return null;
    }

    public void RemoveFromClosed(NodeRecord nodeRecord)
    {
        var savedRecord = NodeRecords[nodeRecord.node.Id];

        nodeRecord.status  = NodeStatus.Open;
        savedRecord.status = NodeStatus.Open;
    }

    // Utility
    public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
    {
        nodeToBeReplaced.status = nodeToReplace.status;
        nodeToBeReplaced.gValue = nodeToReplace.gValue;
        nodeToBeReplaced.hValue = nodeToReplace.hValue;
        nodeToBeReplaced.fValue = nodeToReplace.fValue;
        nodeToBeReplaced.parent = nodeToReplace.parent;
    }

    ICollection<NodeRecord> IOpenSet.All()
    {
        return Open.All();
    }

    ICollection<NodeRecord> IClosedSet.All()
    {
        List<NodeRecord> closed = new List<NodeRecord>();

        for (var i = 0; i < NodeRecords.Length; i++)
        {
            var nodeRecord = NodeRecords[i];

            if(nodeRecord.status == NodeStatus.Closed)
            {
                closed.Add(nodeRecord);
            }
        }

        return closed;
    }

    public int CountOpen()
    {
        return Open.CountOpen();
    }
}
