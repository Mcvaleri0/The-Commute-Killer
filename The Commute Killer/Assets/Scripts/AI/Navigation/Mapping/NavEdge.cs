using UnityEngine;
using UnityEditor;

public class NavEdge : ScriptableObject
{
    public NavNode Left;

    public NavNode Right;

    public bool Connected;

    public void Initialize(NavNode left, NavNode right)
    {
        this.Left = left;

        this.Right = right;
    }


    #region Methods
    public void Connect()
    {
        if (this.Connected) return;

        this.Left.AddAdjacent(this.Right);
        this.Right.AddAdjacent(this.Left);

        this.Connected = true;
    }

    public void Disconnect()
    {
        if (!this.Connected) return;

        this.Left.RemoveAdjacent(this.Right);
        this.Right.RemoveAdjacent(this.Left);

        this.Connected = false;
    }
    #endregion


    #region Operators
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var oEdge = (NavEdge) obj;

        return this.Left == oEdge.Left && this.Right == oEdge.Right;
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}