using System;
public enum NodeStatus
{
    Unvisited,
    Open,
    Closed
}

public class NodeRecord  : IComparable<NodeRecord>
{
    public NavNode node;

    public NodeRecord parent;

    public float gValue;
    public float hValue;
    public float fValue;

    public NodeStatus status;

    //used only for the GoalBounds
    public int StartNodeOutConnectionIndex;

    public void Update(NodeRecord p, float g, float h, float f)
    {
        this.parent = p;
        this.gValue = g;
        this.hValue = h;
        this.fValue = f;
    }

    public int CompareTo(NodeRecord other)
    {
        /*var cF = this.fValue.CompareTo(other.fValue);

        if(cF != 0)
        {
            return cF;
        }

        return this.hValue.CompareTo(other.hValue);*/
        var cF = this.hValue.CompareTo(other.hValue);

        if (cF != 0)
        {
            return cF;
        }

        return this.fValue.CompareTo(other.fValue);
    }

    //two node records are equal if they refer to the same node
    public override bool Equals(object obj)
    {
        var target = obj as NodeRecord;
        if (target != null) return this.node == target.node;
        else return false;
    }

    public override int GetHashCode()
    {
        return this.node.GetHashCode();
    }
}
