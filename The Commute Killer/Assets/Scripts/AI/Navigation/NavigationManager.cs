using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class NavigationManager : MonoBehaviour
{
    public NavGraph Graph;

    public NavClusterGraph ClusterGraph;

    public Dictionary<int, bool> GatewayOpen { get; set; }

    private GatewayDistanceTableRow[] OGatewayDistanceTable;

    public GatewayDistanceTableRow[] GatewayDistanceTable;

    public bool DebugGraph = true;

    public bool DebugClusters = true;

    public bool DebugGateways= true;


    // Use this for initialization
    void Start()
    {
        this.GatewayOpen = new Dictionary<int, bool>();

        foreach(NavGateway gate in this.ClusterGraph.Gateways)
        {
            this.GatewayOpen.Add(gate.Id, true);

            gate.Open();
        }

        this.OGatewayDistanceTable = this.ClusterGraph.GatewayDistanceTable;

        this.GatewayDistanceTable = new GatewayDistanceTableRow[this.OGatewayDistanceTable.Length];

        for(var i = 0; i < this.OGatewayDistanceTable.Length; i++)
        {
            this.GatewayDistanceTable[i] = this.OGatewayDistanceTable[i].Clone();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public bool PathBlocked(NavNode startNode, NavNode endNode)
    {
        if (startNode.Cluster == endNode.Cluster)
        {
            return false;
        }

        var gate = FindConnectingGateway(startNode, endNode);

        if(gate == null)
        {
            return false;
        }

        return !this.GatewayOpen[gate.Id];
    }


    private NavGateway FindConnectingGateway(NavNode startNode, NavNode endNode)
    {
        NavGateway gate = null;

        var centerOfEdge = Vector3.Lerp(startNode.Position, endNode.Position, 0.5f);

        var startCluster = startNode.Cluster;
        var endCluster   = endNode.Cluster;

        var possible = new List<NavGateway>();

        foreach(NavGateway gateway in startCluster.Gateways)
        {
            if(endCluster.Gateways.Contains(gateway))
            {
                possible.Add(gateway);
            }
        }

        if(possible.Count == 1)
        {
            gate = possible[0];
        }
        else if(possible.Count > 1)
        {
            var dist = Mathf.Infinity;

            foreach(var p in possible)
            {
                var ndist = Vector3.Distance(p.Center, centerOfEdge);

                if(ndist < dist)
                {
                    dist = ndist;

                    gate = p;
                }
            }
        }

        return gate;
    }


    public void CloseGateway(int id)
    {
        this.GatewayOpen[id] = false;

        this.ClusterGraph.Gateways[id].Close();
    }


    public void OpenGateway(int id)
    {
        this.GatewayOpen[id] = true;

        this.ClusterGraph.Gateways[id].Open();
    }


    private void OnDrawGizmos()
    {
        var up = new Vector3(0, 0.5f, 0);

        if (this.DebugGraph && this.Graph != null)
        {
            foreach (NavNode n in this.Graph.Nodes)
            {
                Gizmos.color = Color.red;

                if (n.Cluster != null)
                {
                    Gizmos.color = Color.white;
                    Handles.Label(n.Position + up * 0.8f, n.Cluster.Id.ToString());
                }

                Gizmos.DrawSphere(n.Position, 0.05f);

                Handles.Label(n.Position + up, n.Id.ToString());

                

                foreach (NavNode a in n.Adjacents)
                {
                    if (a.Id > n.Id)
                    {
                        Gizmos.color = Color.white;

                        Gizmos.DrawLine(n.Position, a.Position);
                    }
                }
            }
        }

        if(this.DebugClusters && this.ClusterGraph != null)
        {
            foreach(var cluster in this.ClusterGraph.Clusters)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(cluster.Center, 0.2f);

                Handles.Label(cluster.Center + up, cluster.Id.ToString());

                Gizmos.color = Color.blue;

                foreach(var gate in cluster.Gateways)
                {
                    Gizmos.DrawLine(cluster.Center, gate.Center + up / 2);
                }
            }

            foreach(var gateway in this.ClusterGraph.Gateways)
            {
                Gizmos.color = Color.green;

                if (this.GatewayOpen != null && !this.GatewayOpen[gateway.Id]) Gizmos.color = Color.yellow;

                Gizmos.DrawSphere(gateway.Center + up / 2, 0.1f);

                Handles.Label(gateway.Center + up, gateway.Id.ToString());
            }
        }

        if(this.DebugGateways && this.ClusterGraph != null)
        {
            Gizmos.color = Color.cyan;

            foreach(var gate in this.ClusterGraph.Gateways)
            {
                foreach(var edge in gate.Edges)
                {
                    Gizmos.DrawLine(edge.Left.Position, edge.Right.Position);
                }
            }
        }
    }
}
