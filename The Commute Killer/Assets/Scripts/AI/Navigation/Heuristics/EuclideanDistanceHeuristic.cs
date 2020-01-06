using UnityEngine;

public class EuclideanDistanceHeuristic : IHeuristic
{
    public float H(NavNode node, NavNode goalNode)
    {
        return Vector3.Distance(goalNode.Position, node.Position);
    }
}
