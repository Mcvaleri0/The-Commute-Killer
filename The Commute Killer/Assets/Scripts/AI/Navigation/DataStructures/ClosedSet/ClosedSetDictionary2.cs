﻿using System.Collections.Generic;
using UnityEngine;

public class ClosedSetDictionary2 : IClosedSet
{
    private Dictionary<NavNode, SimpleUnorderedNodeList> Closed { get; set; }

    void IClosedSet.Initialize()
    {
        Closed = new Dictionary<NavNode, SimpleUnorderedNodeList>();
    }

    void IClosedSet.AddToClosed(NodeRecord nodeRecord)
    {
        Closed.TryGetValue(nodeRecord.node, out SimpleUnorderedNodeList list);

        if(list == null)
        {
            list = new SimpleUnorderedNodeList();
            Closed.Add(nodeRecord.node, list);
        }

        list.AddToClosed(nodeRecord);
    }

    void IClosedSet.RemoveFromClosed(NodeRecord nodeRecord)
    {
        Closed.TryGetValue(nodeRecord.node, out SimpleUnorderedNodeList list);

        if (list != null)
        {
            list.RemoveFromClosed(nodeRecord);
        }
    }

    //should return null if the node is not found
    NodeRecord IClosedSet.SearchInClosed(NodeRecord nodeRecord)
    {
        Closed.TryGetValue(nodeRecord.node, out SimpleUnorderedNodeList list);

        if (list != null)
        {
            return list.SearchInClosed(nodeRecord);
        }

        return null;
    }

    ICollection<NodeRecord> IClosedSet.All()
    {
        var retList = new List<NodeRecord>();

        foreach (SimpleUnorderedNodeList lst in Closed.Values)
        {
            retList.AddRange(lst.All());
        }

        return retList;
    }
}