using System.Collections.Generic;

public class ClosedSetDictionary : IClosedSet
{
    private Dictionary<float, Dictionary<float, List<NodeRecord>>> Closed { get; set; }

    void IClosedSet.Initialize()
    {
        Closed = new Dictionary<float, Dictionary<float, List<NodeRecord>>>();
    }

    void IClosedSet.AddToClosed(NodeRecord nodeRecord)
    {
        Dictionary<float, List<NodeRecord>> hDict = null;
        Closed.TryGetValue(nodeRecord.fValue, out hDict);

        if (hDict == null)
        {
            hDict = new Dictionary<float, List<NodeRecord>>();
            Closed.Add(nodeRecord.fValue, hDict);
        }

        List<NodeRecord> list = null;
        hDict.TryGetValue(nodeRecord.hValue, out list);

        if(list == null)
        {
            list = new List<NodeRecord>();
            hDict.Add(nodeRecord.hValue, list);
        }

        list.Add(nodeRecord);
    }

    void IClosedSet.RemoveFromClosed(NodeRecord nodeRecord)
    {
        Dictionary<float, List<NodeRecord>> hDict = null;
        Closed.TryGetValue(nodeRecord.fValue, out hDict);

        if (hDict != null)
        {
            List<NodeRecord> list = null;
            hDict.TryGetValue(nodeRecord.hValue, out list);

            if (list != null)
            {
                list.Remove(nodeRecord);
            }
        }
    }

    //should return null if the node is not found
    NodeRecord IClosedSet.SearchInClosed(NodeRecord nodeRecord)
    {
        Dictionary<float, List<NodeRecord>> hDict = null;
        Closed.TryGetValue(nodeRecord.fValue, out hDict);

        if (hDict != null)
        {
            List<NodeRecord> list = null;
            hDict.TryGetValue(nodeRecord.hValue, out list);

            if (list != null)
            {
                return list.Find(n => n == nodeRecord);
            }
        }

        return null;
    }

    ICollection<NodeRecord> IClosedSet.All()
    {
        var retList = new List<NodeRecord>();

        foreach(Dictionary<float, List<NodeRecord>> hD in Closed.Values)
        {
            foreach (List<NodeRecord> lst in hD.Values)
            {
                retList.AddRange(lst);
            }
        }

        return retList;
    }
}
