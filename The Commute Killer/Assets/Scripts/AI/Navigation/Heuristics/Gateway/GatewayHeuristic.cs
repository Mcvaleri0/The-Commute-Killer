using UnityEngine;

public class GatewayHeuristic : IHeuristic
{
    private NavClusterGraph ClusterGr { get; set; }

    public System.Diagnostics.Stopwatch stopwatch;

    private NavNode LastGoal { get; set; }
    public NavCluster  CurrentCluster { get; set; }
    public NavCluster  GoalCluster    { get; set; }
    private float[] Gate2GoalCosts { get; set; }

    public GatewayHeuristic()
    {
        this.ClusterGr = Resources.Load<NavClusterGraph>("ClusterGraph");
        this.stopwatch = new System.Diagnostics.Stopwatch();
    }

    public GatewayHeuristic(NavClusterGraph graph)
    {
        this.ClusterGr = graph;
        this.stopwatch = new System.Diagnostics.Stopwatch();
    }

    public float H(NavNode node, NavNode goalNode)
    {

        this.stopwatch.Start();
        NavCluster currentCluster = ClusterGr.Quantize(node);
        this.stopwatch.Stop();

        this.CurrentCluster    = currentCluster;

        if(currentCluster == null)
        {
            return Mathf.Infinity;
        }

        if(true)
        {
            UpdateGoal(goalNode);
        }

        var min = Mathf.Infinity;

        if (currentCluster.Equals(this.GoalCluster))
        {
            return EuclideanH(node.Position, this.LastGoal.Position);
        }
        else
        {
            for (var i = 0; i < currentCluster.Gateways.Count; i++) {
                NavGateway outGateway = currentCluster.Gateways[i];

                var ih = EuclideanH(node.Position, outGateway.Center);

                for (var j = 0; j < this.Gate2GoalCosts.Length; j++) {
                    NavGateway inGateway = this.GoalCluster.Gateways[j];

                    var h = ih;
                    h += GatewayH(outGateway.Id, inGateway.Id);
                    h += this.Gate2GoalCosts[j];

                    if(h < min)
                    {
                        min = h;
                    }
                }
            }

            return min;
        }
    }

    private void UpdateGoal(NavNode node)
    {
        this.LastGoal = node;

        this.stopwatch.Start();
        this.GoalCluster = this.ClusterGr.Quantize(this.LastGoal);
        this.stopwatch.Stop();

        var gateCount = this.GoalCluster.Gateways.Count;

        this.Gate2GoalCosts = new float[gateCount];

        for(var i = 0; i < gateCount; i++)
        {
            var gate = this.GoalCluster.Gateways[i];

            this.Gate2GoalCosts[i] = EuclideanH(gate.Center, this.LastGoal.Position);
        }
    }

    private float EuclideanH(Vector3 position1, Vector3 position2)
    {
        return Vector3.Distance(position2, position1);
    }

    private float GatewayH(int outGateId, int inGateId)
    {
        GatewayDistanceTableRow[] table = ClusterGr.GatewayDistanceTable;
        GatewayDistanceTableRow   row   = table[outGateId];
        GatewayDistanceTableEntry entry = row.entries[inGateId];

        return entry.ShortestDistance;
    }
}
