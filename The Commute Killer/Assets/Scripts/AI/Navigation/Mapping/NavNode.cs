using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NavNode : ScriptableObject
{
    public int Id;

    public Vector3 Position;

    public NavCluster Cluster;

    public List<NavNode> Adjacents;

    public void Initialize(int id, Vector3 pos)
    {
        this.Id = id;

        this.Position = pos;

        this.Adjacents = new List<NavNode>();
    }

    public bool AddAdjacent(NavNode adjacent)
    {
        foreach(NavNode n in this.Adjacents)
        {
            if(n == adjacent)
            {
                return false;
            }
        }

        this.Adjacents.Add(adjacent);

        return true;
    }

    override public bool Equals(object other)
    {
        var node = other as NavNode;

        if(node != null)
        {
            if(node.Position == this.Position)
            {
                return true;
            }
        }

        return false;
    }

    override public int GetHashCode()
    {
        return base.GetHashCode();
    }
}