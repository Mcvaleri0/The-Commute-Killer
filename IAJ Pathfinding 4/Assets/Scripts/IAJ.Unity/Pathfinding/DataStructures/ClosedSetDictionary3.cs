using System.Collections.Generic;
using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class ClosedSetDictionary3 : IClosedSet
    {
        private Dictionary<Vector3, NodeRecord> Closed { get; set; }

        void IClosedSet.Initialize()
        {
            Closed = new Dictionary<Vector3, NodeRecord>();
        }

        void IClosedSet.AddToClosed(NodeRecord nodeRecord)
        {
            Closed.Add(nodeRecord.node.Position, nodeRecord);
        }

        void IClosedSet.RemoveFromClosed(NodeRecord nodeRecord)
        {
            Closed.Remove(nodeRecord.node.Position);
        }

        //should return null if the node is not found
        NodeRecord IClosedSet.SearchInClosed(NodeRecord nodeRecord)
        {
            Closed.TryGetValue(nodeRecord.node.Position, out NodeRecord nd);

            return nd;
        }

        ICollection<NodeRecord> IClosedSet.All()
        {
            return Closed.Values;
        }
    }
}
