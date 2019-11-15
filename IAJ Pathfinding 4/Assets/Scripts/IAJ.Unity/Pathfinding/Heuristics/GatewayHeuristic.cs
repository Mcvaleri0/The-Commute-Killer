using RAIN.Navigation.Graph;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class GatewayHeuristic : IHeuristic
    {
        private ClusterGraph ClusterGr { get; set; }

        public System.Diagnostics.Stopwatch stopwatch;

        private NavigationGraphNode LastGoal { get; set; }
        public Cluster  CurrentCluster { get; set; }
        public Cluster  GoalCluster    { get; set; }
        private float[] Gate2GoalCosts { get; set; }

        public GatewayHeuristic()
        {
            this.ClusterGr = Resources.Load<ClusterGraph>("ClusterGraph");
            this.stopwatch = new System.Diagnostics.Stopwatch();
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {

            this.stopwatch.Start();
            Cluster currentCluster = ClusterGr.Quantize(node);
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
                for (var i = 0; i < currentCluster.gateways.Count; i++) {
                    Gateway outGateway = currentCluster.gateways[i];

                    var ih = EuclideanH(node.Position, outGateway.center);

                    for (var j = 0; j < this.Gate2GoalCosts.Length; j++) {
                        Gateway inGateway = this.GoalCluster.gateways[j];

                        var h = ih;
                        h += GatewayH(outGateway.id, inGateway.id);
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

        private void UpdateGoal(NavigationGraphNode node)
        {
            this.LastGoal = node;

            this.stopwatch.Start();
            this.GoalCluster = this.ClusterGr.Quantize(this.LastGoal);
            this.stopwatch.Stop();

            var gateCount = this.GoalCluster.gateways.Count;

            this.Gate2GoalCosts = new float[gateCount];

            for(var i = 0; i < gateCount; i++)
            {
                var gate = this.GoalCluster.gateways[i];

                this.Gate2GoalCosts[i] = EuclideanH(gate.center, this.LastGoal.Position);
            }
        }

        private float EuclideanH(Vector3 position1, Vector3 position2)
        {
            return Vector3.Distance(position2, position1);
        }

        private float GatewayH(int outGateId, int inGateId)
        {
            GatewayDistanceTableRow[] table = ClusterGr.gatewayDistanceTable;
            GatewayDistanceTableRow   row   = table[outGateId];
            GatewayDistanceTableEntry entry = row.entries[inGateId];

            return entry.shortestDistance;
        }
    }
}
