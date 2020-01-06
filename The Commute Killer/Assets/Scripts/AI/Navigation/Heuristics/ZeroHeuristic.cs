public class ZeroHeuristic : IHeuristic
{
    public float H(NavNode node, NavNode goalNode)
    {
        return 0;
    }
}
