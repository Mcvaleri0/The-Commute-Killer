using UnityEngine;

public class GatewayHeuristic : IHeuristic
{
    private GatewayDistanceTableRow[] GatewayDistanceTable;

    private NavNode LastGoal { get; set; }

    public NavCluster  CurrentCluster { get; set; }

    public NavCluster  GoalCluster    { get; set; }

    private float[] Gate2GoalCosts { get; set; }

    public GatewayHeuristic(GatewayDistanceTableRow[] table)
    {
        this.GatewayDistanceTable = table;
    }

    public float H(NavNode node, NavNode goalNode)
    {

        NavCluster currentCluster = node.Cluster;

        this.CurrentCluster = currentCluster;

        if(currentCluster == null)
        {
            return Mathf.Infinity;
        }

        if(goalNode != this.LastGoal)
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

                if (GatewayH(outGateway.Id, outGateway.Id) == Mathf.Infinity) continue;

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

        this.GoalCluster = this.LastGoal.Cluster;

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
        GatewayDistanceTableRow   row   = this.GatewayDistanceTable[outGateId];
        GatewayDistanceTableEntry entry = row.entries[inGateId];

        return entry.ShortestDistance;
    }
}
